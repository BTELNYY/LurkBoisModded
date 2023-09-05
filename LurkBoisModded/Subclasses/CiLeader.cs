using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using LurkBoisModded.Managers;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class CiLeader : SubclassBase
    {
        public override string FileName => "ci_leader";
        public override RoleTypeId Role => RoleTypeId.ChaosRepressor;
        public override string SubclassNiceName => "CI Leader";
        public override string SubclassDescription => "Press the noclip key (left alt) to use your war cry ability! it will grant all teammates within a certain range a damage reduction boost! You also have higher max health then normal.";
        public override List<AbilityType> Abilities => new List<AbilityType>() { AbilityType.WarCry };
        public override string ClassColor => "green";
        public override float MaxHealth => 125f;
    }
}
