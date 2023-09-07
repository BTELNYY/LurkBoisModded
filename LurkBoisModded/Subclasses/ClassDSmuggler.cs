using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class ClassDSmuggler : Subclass
    {
        public override string ClassColor => "orange";

        public override string FileName => "classd_smuggler";

        public override string SubclassNiceName => "Class D Smuggler";

        public override string SubclassDescription => "You spawn with random items, its good to have deep pockets.";

        public override Dictionary<ItemType, ushort> RandomItems => new Dictionary<ItemType, ushort>() 
        {
            [ItemType.Ammo9x19] = 10,
            [ItemType.KeycardJanitor] = 1,
            [ItemType.Flashlight] = 1,
            [ItemType.KeycardScientist] = 1,
            [ItemType.Painkillers] = 1,
            [ItemType.Medkit] = 1,
        };

        public override int NumberOfRandomItems => 2;
    }
}
