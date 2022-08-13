using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Common.Items
{
    public abstract class AidKit: Item
    {
        public abstract int HealCount { get; }

        public override string Description => $"{Name} - лечит {HealCount}";
    }

    public class CasualAidKit : AidKit
    {
        public override int HealCount => 20;

        public override string Name => "Аптечка";
    }

    public class CombatAidKit : AidKit
    {
        public override int HealCount => 20;

        public override string Name => "Аптечка";
    }

    public class ScienseAidKit : AidKit
    {
        public override int HealCount => 20;

        public override string Name => "Аптечка";
    }
}
