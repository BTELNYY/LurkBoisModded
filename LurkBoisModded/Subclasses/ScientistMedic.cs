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
    public class ScientistMedic : Subclass
    {
        public override string FileName => "scientist_medic";

        public override RoleTypeId Role => RoleTypeId.Scientist;

        public override string ClassColor => "#FFFF7C";

        public override string SubclassNiceName => "Scientist Medic";

        public override string SubclassDescription => "Press your noclip key (left alt by default) while looking at someone and holding a healing item to heal that player!";

        public override List<AbilityType> Abilities => new List<AbilityType>() 
        {
            AbilityType.MedicAbility,
        };

        public override int NumberOfRandomItems => 2;

        public override Dictionary<ItemType, short> RandomItems => new Dictionary<ItemType, short>() 
        {
            [ItemType.Medkit] = 1,
            [ItemType.Painkillers] = 1,
            [ItemType.Adrenaline] = 1,
        };
    }
}
