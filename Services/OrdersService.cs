using IndurFarmsAPI.DTO_s;
using IndurFarmsAPI.Enums;
using IndurFarmsAPI.IRepositories;
using IndurFarmsAPI.IServices;
using IndurFarmsAPI.Middleware;
using IndurFarmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IndurFarmsAPI.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IndurFarmsDbContext _indurFarmsDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICartRepository _cartRepository;
        public OrdersService(IOrdersRepository ordersRepository,
            IndurFarmsDbContext indurFarmsDbContext,
            ICurrentUserService currentUserService,
            ICartRepository cartRepository)
        {
            _ordersRepository = ordersRepository;
            _indurFarmsDbContext = indurFarmsDbContext;
            _currentUserService = currentUserService;
            _cartRepository = cartRepository;
        }

        public async Task<PlaceOrderResponseDto> PlaceOrderAsync(int addressId)
        {
            using var transaction = await _indurFarmsDbContext.Database.BeginTransactionAsync();
            try
            {
                CurrentUserDto currentUser = await _currentUserService.GetCurrentUserAsync();
                var userCart = await _cartRepository.GetUserCartItemsAsync(currentUser.UserId);
                if (!userCart.Any())
                {
                    throw new Exception("Cart is empty.");
                }
                var productIds = userCart.Select(p => p.ProductId).ToList();
                var products = await _indurFarmsDbContext.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();
                decimal total = 0;
                foreach (var cartItem in userCart)
                {
                    Products product = products.First(p => p.Id == cartItem.ProductId);
                    if (product.Stock < cartItem.Quantity)
                    {
                        throw new Exception($"{product.Name} has only {product.Stock} stock left");
                    }
                }
                foreach (var cartItem in userCart)
                {
                    Products product = products.First(p => p.Id == cartItem.ProductId);
                    product.Stock -= cartItem.Quantity;
                    total += product.Price * cartItem.Quantity;
                }
                Orders orders = new Orders
                {
                    UserId = currentUser.UserId,
                    AddressId = addressId,
                    TotalAmount = total,
                    PaymentStatus = (int)PaymentStatus.Pending,
                    OrderStatus = (int)OrderStatus.Pending,
                    CreatedDate = DateTime.UtcNow,
                };
                Orders order = await _ordersRepository.CreateOrderAsync(orders);
                ICollection<OrderItems> orderItems = userCart.Select(ord =>
                {
                    var product = products.First(p => p.Id == ord.ProductId);
                    return new OrderItems
                    {
                        OrderId = order.Id,
                        ProductId = ord.ProductId,
                        Quantity = ord.Quantity,
                        Price = product.Price,
                    };
                }).ToList();
                await _ordersRepository.CreateOrderItemsAsync(orderItems);
                await _ordersRepository.ClearCartAsync(userCart);
                await transaction.CommitAsync();
                return new PlaceOrderResponseDto
                {
                    OrderId = order.Id,
                    TotalAmount = total,
                    OrderStatus = ((OrderStatus)order.OrderStatus).ToString(),
                    PaymentStatus = ((PaymentStatus)order.PaymentStatus).ToString(),
                    OrderDate = order.CreatedDate
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ICollection<PlaceOrderResponseDto>> GetAllUserOrders()
        {
            CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
            ICollection<Orders> orders = await _ordersRepository.GetAllOrdersForUser(currentUserDto.UserId);
            ICollection<PlaceOrderResponseDto> placeOrderResponseDtos = orders.Select(order => new PlaceOrderResponseDto
            {
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                OrderDate = order.CreatedDate,
                OrderStatus = ((OrderStatus)order.OrderStatus).ToString(),
                PaymentStatus = ((PaymentStatus)order.PaymentStatus).ToString()
            }).ToList();
            return placeOrderResponseDtos;
        }

        public async Task<ICollection<OrderItemResponseDto>> GetOrderItemsAsync(int orderId)
        {
            ICollection<OrderItems> orderItems =  await _ordersRepository.GetOrderItemsAsync(orderId);
            ICollection<OrderItemResponseDto> orderItemResponse = orderItems.Select(ordItem => new OrderItemResponseDto
            {
                ProductId = ordItem.ProductId,
                ProductName = ordItem.Product.Name,
                ImageUrl = ordItem.Product.ImageURL,
                Quantity = ordItem.Quantity,
                Price = ordItem.Price
            }).ToList();
            return orderItemResponse;
        }

        public async Task CancelOrder(int orderId)
        {
            using var transaction = await _indurFarmsDbContext.Database.BeginTransactionAsync();
            try
            {
                CurrentUserDto currentUserDto = await _currentUserService.GetCurrentUserAsync();
                Orders? order = await _indurFarmsDbContext.Orders.FirstOrDefaultAsync(ord => ord.Id == orderId && ord.UserId == currentUserDto.UserId);
                if (order == null)
                {
                    throw new Exception("Order does not exists");
                }
                if ((order.OrderStatus == (int)OrderStatus.Pending || order.OrderStatus == (int)OrderStatus.Confirmed) && 
                    order.OrderStatus != (int)OrderStatus.Cancelled)
                {
                    await _ordersRepository.UpdateOrderStatus(order, (int)OrderStatus.Cancelled);
                }
                else
                {
                    throw new Exception($"Order cannot be cancelled as it is in {((OrderStatus)order.OrderStatus)}");
                }
                ICollection<OrderItems> orderItems = await _ordersRepository.GetOrderItemsAsync(orderId);
                List<int> productIds = orderItems.Select(o => o.ProductId).ToList();
                ICollection<Products> products = await _indurFarmsDbContext.Products.Where(prod =>
                                                                productIds.Contains(prod.Id)).ToListAsync();
                foreach (var orderItem in orderItems)
                {
                    Products? product = products.FirstOrDefault(prod => prod.Id == orderItem.ProductId);
                    product.Stock += orderItem.Quantity;
                }
                await _indurFarmsDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ICollection<Orders>> GetOrdersAsync()
        {
            return await _ordersRepository.GetAllOrdersAsync();
        }

        public async Task UpdateOrderStatus(int orderId, int status)
        {
            Orders? order = await _ordersRepository.GetOrderByOrderId(orderId);
            await _ordersRepository.UpdateOrderStatus(order!, status);
        }
    }
}
