using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IndurFarmsAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[cart]")]
    public class CartController : ControllerBase
    {
        private readonly 
        public CartController() { }
    }
}
