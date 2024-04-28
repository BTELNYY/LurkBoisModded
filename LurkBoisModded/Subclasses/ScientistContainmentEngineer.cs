using LurkBoisModded.Base;
using MapGeneration;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class ScientistContainmentEngineer : Subclass
    {
        public override string FileName => "scientist_containment_engineer";

        public override RoleTypeId Role => RoleTypeId.Scientist;

        public override string ClassColor => "#FFFF7C";

        public override string SubclassNiceName => "Scientist Containment Engineer";

        public override string SubclassDescription => "Use your better keycard to help the guards deal with the SCPs. Your subclass cannot escape!";

        public override bool AllowEscape => false;

        public override string EscapeFailMessage => "You cannot escape, you must help the surviving foundation forces!";

        public override Dictionary<ItemDefinition, short> SpawnItems => new Dictionary<ItemDefinition, short>() 
        {
            [new ItemDefinition(ItemType.KeycardContainmentEngineer)] = 1,
            [new ItemDefinition(ItemType.KeycardScientist)] = -1,
            [new ItemDefinition(ItemType.GunCOM18)] = 1,
        };

        public override List<RoomName> SpawnRooms => new List<RoomName>() 
        {
            RoomName.EzCollapsedTunnel,
            RoomName.EzEvacShelter,
            RoomName.EzRedroom,
            RoomName.EzOfficeLarge,
            RoomName.EzOfficeSmall,
            RoomName.EzOfficeStoried
        };
    }
}
