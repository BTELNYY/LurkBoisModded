using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Base;
using PlayerRoles;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Subclasses
{
    public class CiDemoman : Subclass
    {
        public override string FileName => "ci_demoman";

        public override string SubclassNiceName => "CI Demoman";

        public override string SubclassDescription => "You can drop remote explosives (radios) and detonate them with your noclip key (left alt) while holding another radio!";

        public override RoleTypeId Role => RoleTypeId.ChaosMarauder;

        public override string ClassColor => "green";

        public override List<AbilityType> Abilities => new List<AbilityType>() { AbilityType.RemoteExplosive };

        public override Dictionary<ItemType, short> SpawnItems => new Dictionary<ItemType, short>() 
        {
            [ItemType.Radio] = 2,
        };

        public CiDemoman()
        {

        }
    }
}
