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

        public override bool ClearInventoryOnSpawn => true;

        public override string ClassColor => "#5B6370";

        public override Dictionary<ItemType, short> SpawnItems => new Dictionary<ItemType, short>() 
        {
            [ItemType.Ammo9x19] = 80,
            [ItemType.KeycardMTFPrivate] = 1,
            [ItemType.GunCrossvec] = 30,
            [ItemType.Medkit] = 1,
            [ItemType.Radio] = 1,
            [ItemType.ArmorCombat] = 1,
        };
    }
}
