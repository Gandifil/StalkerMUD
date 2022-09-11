using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalkerMUD.Common;
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
        private readonly IRepository<Item> _items;
        private readonly IRepository<User> _users;
        private readonly IRepository<ShopPoint> _shopPoints;

        public PlayerController(IHttpContextAccessor httpContextAccessor, IRepository<User> users, IRepository<ShopPoint> shopPoints, IRepository<Item> items)
        {
            _httpContextAccessor = httpContextAccessor;
            _users = users;
            _shopPoints = shopPoints;
            _items = items;
        }

        [HttpGet]
        public async Task<PlayerResponse> Get()
        {
            int id = GetUserId();
            var user = await _users.GetAsync(id);
            var player = user.Player;
            return new PlayerResponse()
            {
                Name = user.Name,
                AttributeFreePoints = player.AttributeFreePoints,
                Attributes = player.Attributes,
                MaxHP = 10 * player.Attributes[AttributeType.Health] 
                + (await _items.GetAsync(player.SelectedSuitId ?? 0))?.Health ?? 0,
                Resistance = (await _items.GetAsync(player.SelectedSuitId ?? 0))?.Resistance ?? 0,
                CritPercent = player.Attributes[AttributeType.WeakExploit] * 2,
                CritFactor = 2.0f + 0.1f * player.Attributes[AttributeType.WeakExploit],
            };
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

                var item = await _items.GetAsync(shopPoint.ItemId);
                switch (item.Type)
                {
                    case Common.ItemType.Weapon:
                        user.Player.SelectedWeaponId = item.Id;
                        break;
                    case Common.ItemType.Suit:
                        user.Player.SelectedSuitId = item.Id;
                        break;
                    default:
                        break;
                }
                await _users.UpdateAsync(user);
            }
            else throw new ArgumentOutOfRangeException();
        }

        [HttpPost]
        [Route("upgrade")]
        public async Task Buy([FromBody] UpgradeRequest upgradeRequest)
        {
            int userId = GetUserId();
            var user = await _users.GetAsync(userId);
            var player = user.Player;
            if (player.AttributeFreePoints > 0)
            {
                player.AttributeFreePoints--;
                player.Attributes[upgradeRequest.Attribute]++;
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
