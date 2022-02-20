using StalkerMUD.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Client.Network
{
    internal class MockServer : IServer
    {
        public async Task<IEnumerable<ShopPoint>> SelectShopPoints()
        {
            return new List<ShopPoint>()
            {
                new ShopPoint
                {
                    Id = 0,
                    Cost = 5000,
                }
            };
        }
    }
}
