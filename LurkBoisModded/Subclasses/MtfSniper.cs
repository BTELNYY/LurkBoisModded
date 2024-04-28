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

        public override Dictionary<ItemDefinition, short> SpawnItems => new Dictionary<ItemDefinition, short>() 
        {
            [new ItemDefinition(ItemType.ArmorCombat)] = 1,
            [new ItemDefinition(ItemType.GunCOM18)] = 1,
            [new ItemDefinition(ItemType.Ammo556x45)] = 80,
            [new ItemDefinition(ItemType.Medkit)] = 1,
            [new ItemDefinition(ItemType.Radio)] = 1,
            [new ItemDefinition(ItemType.Ammo9x19)] = 50,
            [new ItemDefinition(ItemType.KeycardMTFOperative)] = 1,
            [new ItemDefinition(CustomItemType.SniperE11, new FirearmDefinition(1, 0, InventorySystem.Items.Firearms.FirearmStatusFlags.None, true))] = 1,
        };
    }
}
