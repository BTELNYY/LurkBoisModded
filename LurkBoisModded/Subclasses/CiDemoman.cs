using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Base;
using PlayerRoles;

namespace LurkBoisModded.Subclasses
{
    public class CiDemoman : SubclassBase
    {
        public override string FileName => "ci_demoman";

        public override string SubclassNiceName => "CI Demoman";

        public override string SubclassDescription => "You can drop remote explosives (radios) and detonate them with your noclip key (left alt) while holding another radio!";

        public override RoleTypeId Role => RoleTypeId.ChaosRifleman;

        public override string ClassColor => "green";

        public override List<AbilityType> Abilities => new List<AbilityType>() { AbilityType.RemoteExplosive };

        public override Dictionary<ItemType, ushort> SpawnItems => new Dictionary<ItemType, ushort>() 
        {
            [ItemType.Radio] = 2,
        };

        public CiDemoman()
        {

        }
    }
}
