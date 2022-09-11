using StalkerMUD.Common.Models;
using StalkerMUD.Server.Data;
using StalkerMUD.Server.Entities;

namespace StalkerMUD.Server.Services
{
    public interface IShopsService
    {
        Task<List<ShopPointResponse>> GetAllShopPointsAsync();
    }

    public class ShopsService : IShopsService
    {
        private readonly IRepository<Item> _items;

        private readonly IRepository<ShopPoint> _shopPoints;

        public ShopsService(IRepository<Item> items, IRepository<ShopPoint> shopPoints)
        {
            _items = items;
            _shopPoints = shopPoints;
        }

        public async Task<List<ShopPointResponse>> GetAllShopPointsAsync()
        {
            var points = await _shopPoints.GetAllAsync();
            return points.Select(x => new ShopPointResponse()
            {
                Id = x.Id,
                ItemId = x.ItemId,
                Name = _items.GetAsync(x.ItemId).Result.Name,
                Cost = x.Cost,
            }).ToList();
        }
    }
}
