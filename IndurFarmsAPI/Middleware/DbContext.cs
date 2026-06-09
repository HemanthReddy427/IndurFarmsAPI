using IndurFarmsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IndurFarmsAPI.Middleware
{
        public class IndurFarmsDbContext : DbContext
        {
            public IndurFarmsDbContext(DbContextOptions<IndurFarmsDbContext> options)
                : base(options)
            {
            }

            public DbSet<Users> Users { get; set; }
            public DbSet<Address> Addresses { get; set; }
            public DbSet<Cart> Carts { get; set; }
            public DbSet<CartItems> CartItems { get; set; }
            public DbSet<Products> Products { get; set; }
            public DbSet<Orders> Orders { get; set; }
            public DbSet<OrderItems> OrderItems { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                // User -> Address (1:N)
                modelBuilder.Entity<Address>()
                    .HasOne(a => a.User)
                    .WithMany(u => u.Addresses)
                    .HasForeignKey(a => a.UserId);

                // User -> Cart (1:1)
                modelBuilder.Entity<Cart>()
                    .HasOne(c => c.User)
                    .WithOne(u => u.Cart)
                    .HasForeignKey<Cart>(c => c.UserId);

                // Cart -> CartItems (1:N)
                modelBuilder.Entity<CartItems>()
                    .HasOne(ci => ci.Cart)
                    .WithMany(c => c.Items)
                    .HasForeignKey(ci => ci.CartId);

                // Product -> CartItems (1:N)
                modelBuilder.Entity<CartItems>()
                    .HasOne(ci => ci.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(ci => ci.ProductId);

                // User -> Orders (1:N)
                modelBuilder.Entity<Orders>()
                    .HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId);

                // Orders -> OrderItems (1:N)
                modelBuilder.Entity<OrderItems>()
                    .HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId);

                // Product -> OrderItems (1:N)
                modelBuilder.Entity<OrderItems>()
                    .HasOne(oi => oi.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(oi => oi.ProductId);
            }
        }
}
