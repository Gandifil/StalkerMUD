using StalkerMUD.Common;
using StalkerMUD.Common.Models;
using StalkerMUD.Server.Data;
using StalkerMUD.Server.Entities;

namespace StalkerMUD.Server.Services
{
    public interface IFightParamatersCalculator
    {
        Task<FightParametersResponse> GetForAsync(User user);

        Task<FightParametersResponse> GetForAsync(Mob mob);
    }

    public class FightParamatersCalculator : IFightParamatersCalculator
    {
        private readonly IRepository<Item> _items;

        public FightParamatersCalculator(IRepository<Item> items)
        {
            _items = items;
        }

        public async Task<FightParametersResponse> GetForAsync(User user)
        {
            var player = user.Player;
            var weaponDamage = player.SelectedWeaponId.HasValue
                ? (await _items.GetAsync(player.SelectedWeaponId.Value)).Damage
                : 1;
            return new FightParametersResponse()
            {
                Name = user.Name,
                Attributes = player.Attributes.Data,
                MaxHP = 10 * player.Attributes.Data[AttributeType.Health]
                + ((await _items.GetAsync(player.SelectedSuitId ?? 0))?.Health ?? 0),
                Resistance = (await _items.GetAsync(player.SelectedSuitId ?? 0))?.Resistance ?? 0,
                CritPercent = player.Attributes.Data[AttributeType.WeakExploit] * 2,
                CritFactor = 2.0f + 0.1f * player.Attributes.Data[AttributeType.WeakExploit],
                Damage = weaponDamage,
            };
        }

        public async Task<FightParametersResponse> GetForAsync(Mob mob)
        {
            return new FightParametersResponse()
            {
                Name = mob.Name,
                Attributes = mob.Attributes.Data,
                MaxHP = 10 * mob.Attributes.Data[AttributeType.Health],
                Resistance = 0,
                CritPercent = mob.Attributes.Data[AttributeType.WeakExploit] * 2,
                CritFactor = 2.0f + 0.1f * mob.Attributes.Data[AttributeType.WeakExploit],
                Damage = 2,
            };
        }
    }
}
