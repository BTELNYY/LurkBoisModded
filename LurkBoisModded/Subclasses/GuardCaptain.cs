using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class GuardCaptain : Subclass
    {
        public override RoleTypeId Role => RoleTypeId.FacilityGuard;

        public override string SubclassNiceName => "Guard Captain";

        public override string FileName => "guard_captain";

        public override string SubclassDescription => "As the leader of the guards you are more equipped for combat then the others, you can also use your ability key (noclip key) to inspire your peers.";

        public override bool ClearInventoryOnSpawn => true;

        public override string ClassColor => "#5B6370";

        public override Dictionary<ItemDefinition, short> SpawnItems => new Dictionary<ItemDefinition, short>() 
        {
            [new ItemDefinition(ItemType.Ammo9x19)] = 60,
            [new ItemDefinition(ItemType.KeycardMTFPrivate)] = 1,
            [new ItemDefinition(ItemType.GunCrossvec)] = 1,
            [new ItemDefinition(ItemType.Medkit)] = 1,
            [new ItemDefinition(ItemType.Radio)] = 1,
            [new ItemDefinition(ItemType.ArmorCombat)] = 1,
        };
    }
}
