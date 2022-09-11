using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalkerMUD.Common.Models;
using StalkerMUD.Server.Data;
using StalkerMUD.Server.Entities;

namespace StalkerMUD.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _users;
        private readonly IRepository<ShopPoint> _shopPoints;

        public PlayerController(IHttpContextAccessor httpContextAccessor, IRepository<User> users, IRepository<ShopPoint> shopPoints)
        {
            _httpContextAccessor = httpContextAccessor;
            _users = users;
            _shopPoints = shopPoints;
        }

        [HttpGet]
        [Route("money")]
        public async Task<int> GetMoney()
        {
            int id = GetUserId();
            var user = await _users.GetAsync(id);
            return user.Player.Money;
        }

        [HttpPost]
        [Route("buy")]
        public async Task Buy([FromBody] BuyRequest buyRequest)
        {
            int userId = GetUserId();
            var user = await _users.GetAsync(userId);
            var shopPoint = await _shopPoints.GetAsync(buyRequest.ShopPointId);
            if (user.Player.Money >= shopPoint.Cost)
            {
                user.Player.Money -= shopPoint.Cost;
                user.Player.AddItem(shopPoint.ItemId);
                await _users.UpdateAsync(user);
            }
            else throw new ArgumentOutOfRangeException();
        }

        private int GetUserId()
        {
            return Convert.ToInt32(_httpContextAccessor.HttpContext?.User.Identities.First().Claims.First(x => x.Type == "id").Value);
        }
    }
}
