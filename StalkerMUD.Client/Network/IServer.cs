using StalkerMUD.Common.Models;

namespace StalkerMUD.Client.Network
{
    internal interface IServer
    {
        Task<IEnumerable<ShopPoint>> SelectShopPoints();
    }
}
