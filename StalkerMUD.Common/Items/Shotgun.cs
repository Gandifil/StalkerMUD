using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StalkerMUD.Common.Items
{
    public class Shotgun : Weapon
    {
        public override string Name => "Дробовик";

        public override int Damage => throw new NotImplementedException();
    }
}
