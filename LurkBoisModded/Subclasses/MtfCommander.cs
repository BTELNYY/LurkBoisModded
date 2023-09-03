using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class MtfCommander : SubclassBase
    {
        public override string FileName => "mtf_commander";

        public override string SubclassNiceName => "MTF Commander";

        public override string SubclassDescription => "Use your noclip key (left alt by default) to provide a buff to nearby teammates!";

        public override RoleTypeId Role => RoleTypeId.NtfCaptain;

        public override List<AbilityType> Abilities => new List<AbilityType>() { AbilityType.Inspire };

        public MtfCommander()
        {

        }
    }
}
