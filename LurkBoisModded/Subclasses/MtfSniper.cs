using LurkBoisModded.Base;
using LurkBoisModded.Base.CustomItems;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class MtfSniper : Subclass
    {
        public override string FileName => "mtf_sniper";

        public override RoleTypeId Role => RoleTypeId.NtfSergeant;

        public override string ClassColor => "blue";

        public override string SubclassNiceName => "MTF Sniper";

        public override string SubclassDescription => "Using your powerful sniper you can do a lot of damage, be careful however, for it takes time to reload a weapon of that size.";

        public override bool ClearInventoryOnSpawn => true;

        public override Dictionary<ItemType, short> SpawnItems => new Dictionary<ItemType, short>() 
        {
            [ItemType.ArmorCombat] = 1,
            [ItemType.GunCOM18] = 18,
            [ItemType.Ammo556x45] = 80,
            [ItemType.Medkit] = 1,
            [ItemType.Radio] = 1,
            [ItemType.Ammo9x19] = 50,
            [ItemType.KeycardMTFOperative] = 1,
        };

        public override Dictionary<CustomItemType, short> CustomItems => new Dictionary<CustomItemType, short>() 
        {
            [CustomItemType.SniperE11] = 1,
        };
    }
}
