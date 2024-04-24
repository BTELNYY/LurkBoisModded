using CustomPlayerEffects;
using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using PlayerRoles;
using PlayerRoles.Spectating;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public override string SubclassDescription => "You are one buff person, and it paid off! You can run slightly faster than everyone else, and you can take more of a beating.";

        public override List<EffectDefinition> SpawnEffects => new List<EffectDefinition>() 
        {
            new EffectDefinition()
            {
                Name = nameof(MovementBoost),
                Duration = 0,
                Intensity = 5,
            },
        };

        public override string ClassColor => "#5B6370";

        public override Dictionary<ItemType, short> SpawnItems => new Dictionary<ItemType, short>() 
        {
            [ItemType.Adrenaline] = 1,
            [ItemType.Ammo556x45] = 20,
        };
    }
}
