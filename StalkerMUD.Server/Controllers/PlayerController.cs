using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalkerMUD.Common;
using StalkerMUD.Common.Models;
using StalkerMUD.Server.Data;
using StalkerMUD.Server.Entities;
using StalkerMUD.Server.Services;

namespace StalkerMUD.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PlayerController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _users;
        private readonly IRepository<Item> _items;
        private readonly IRepository<ShopPoint> _shopPoints;
        private readonly IFightParamatersCalculator _fightParamatersCalculator;

        public PlayerController(IHttpContextAccessor httpContextAccessor, IRepository<User> users, IRepository<ShopPoint> shopPoints, IFightParamatersCalculator fightParamatersCalculator = null, IRepository<Item> items = null)
        {
            _httpContextAccessor = httpContextAccessor;
            _users = users;
            _shopPoints = shopPoints;
            _fightParamatersCalculator = fightParamatersCalculator;
            _items = items;
        }

        [HttpGet]
        public async Task<PlayerResponse> Get()
        {
            int id = GetUserId();
            var user = await _users.GetAsync(id);
            var parameters = await _fightParamatersCalculator.GetForAsync(user);
            return new PlayerResponse()
            {
                AttributeFreePoints = user.Player.AttributeFreePoints,
                Name = parameters.Name,
                Attributes = parameters.Attributes,
                MaxHP = parameters.MaxHP,
                Resistance = parameters.Resistance,
                CritPercent = parameters.CritPercent,
                CritFactor = parameters.CritFactor,
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
        public async Task UpgradeAsync([FromBody] UpgradeRequest upgradeRequest)
        {
            int userId = GetUserId();
            var user = await _users.GetAsync(userId);
            var player = user.Player;
            if (player.AttributeFreePoints > 0)
            {
                player.AttributeFreePoints--;
                player.Attributes.Data[upgradeRequest.Attribute]++;
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
