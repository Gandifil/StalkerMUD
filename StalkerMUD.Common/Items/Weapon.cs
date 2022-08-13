using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Common.Items
{
    public abstract class Weapon: Item
    {
        public abstract int Damage { get; }

        public override string Description => $"{Name} - урон: {Damage}, кол-во выстрелов: {ShootTimes}";

        public virtual int ShootTimes => 1;
    }
}
