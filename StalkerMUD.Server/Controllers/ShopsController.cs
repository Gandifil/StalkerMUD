using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalkerMUD.Common.Models;
using StalkerMUD.Server.Models;
using StalkerMUD.Server.Services;

namespace StalkerMUD.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ShopsController : ControllerBase
    {
        private readonly IShopsService _shops;

        public ShopsController(IShopsService shops)
        {
            _shops = shops;
        }

        [HttpGet]
        //[Route("")]
        public Task<List<ShopPointResponse>> Get()
        {
            return _shops.GetAllShopPointsAsync();
        }
    }
}
