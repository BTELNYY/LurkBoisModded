using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Base;
using PlayerRoles;
using LurkBoisModded.Managers;
using LurkBoisModded.Base.CustomItems;

namespace LurkBoisModded.Subclasses
{
    public class CiDemoman : Subclass
    {
        public override string FileName => "ci_demoman";

        public override string SubclassNiceName => "CI Demoman";

        public override string SubclassDescription => "You can drop remote explosives (coins) and detonate them with the C4 Detonator (radio).";

        public override RoleTypeId Role => RoleTypeId.ChaosMarauder;

        public override string ClassColor => "green";

        public override Dictionary<ItemDefinition, short> SpawnItems => new Dictionary<ItemDefinition, short>()
        {
            [new ItemDefinition(CustomItemType.C4Detonator)] = 1,
            [new ItemDefinition(CustomItemType.C4Charge)] = 2,
            [new ItemDefinition(ItemType.Painkillers)] = -1,
        };

        public CiDemoman()
        {

        }
    }
}
