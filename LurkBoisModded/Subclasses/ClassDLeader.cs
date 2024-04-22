using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class ClassDLeader : Subclass
    {
        public override string ClassColor => "orange";

        public override string FileName => "classd_leader";

        public override string SubclassNiceName => "Class D Leader";

        public override string SubclassDescription => "Press your noclip key (default left alt) to inspire your fellow test subjects!";

        public override List<AbilityType> Abilities => new List<AbilityType>() 
        {
            AbilityType.Inspire,
        };
    }
}
