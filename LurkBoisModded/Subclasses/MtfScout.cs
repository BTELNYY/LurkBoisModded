using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LurkBoisModded.Managers;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class MtfScout : Subclass
    {
        public override string FileName => "mtf_scout";

        public override string SubclassNiceName => "MTF Scout";

        public override string SubclassDescription => "Use your noclip key (left alt by default) to gain a temporary speed boost at the cost of max health reduction.";
        public override string ClassColor => "blue";

        public override RoleTypeId Role => RoleTypeId.NtfPrivate;

        public override RoleTypeId TargetRole => RoleTypeId.NtfPrivate;

        public override bool TargetRoleUsed => true;

        public override List<AbilityType> Abilities => new List<AbilityType>() 
        {
            AbilityType.Scout    
        };

        public MtfScout()
        {

        }
    }
}
