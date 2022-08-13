using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Common
{
    public static class Attributes
    {

        public static Dictionary<AttributeType, string> Names { get; }
        = new()
        {
            { AttributeType.Health, "Здоровье" },
            { AttributeType.WeaponLevel, "Владение оружием" },
            { AttributeType.Accuracy, "Точность" },
            { AttributeType.Dodge, "Уклонение" },
            { AttributeType.WeakExploit, "Знание противников" },
            { AttributeType.Speed, "Скорость" },
        };
    }
}
