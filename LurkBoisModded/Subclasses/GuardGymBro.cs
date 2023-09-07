using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class GuardGymBro : Subclass
    {
        public override RoleTypeId Role => RoleTypeId.FacilityGuard;

        public override string SubclassNiceName => "Guard Gym Bro";

        public override float MaxHealth => 110f;

        public override string FileName => "guard_gym_bro";

        public override string SubclassDescription => "As leader of the guards, press your Noclip Key (default left alt) to inspire those around you!";

        public override List<AbilityType> Abilities => new List<AbilityType>() 
        {
            AbilityType.Inspire,
        };

        public override string ClassColor => "#5B6370";

        public override Dictionary<ItemType, ushort> SpawnItems => new Dictionary<ItemType, ushort>() 
        {
            [ItemType.Adrenaline] = 1,
            [ItemType.Ammo556x45] = 20,
        };
    }
}
