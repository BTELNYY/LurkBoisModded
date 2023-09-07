using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using MapGeneration;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class GuardZoneManager : Subclass
    {
        public override RoleTypeId Role => RoleTypeId.FacilityGuard;

        public override string SubclassNiceName => "Zone Manager";

        public override string FileName => "guard_zone_manager";

        public override string SubclassDescription => "Use your keycard to help scientists escape into Heavy!";

        public override string ClassColor => "#5B6370";

        public override Dictionary<ItemType, ushort> SpawnItems => new Dictionary<ItemType, ushort>()
        {
            [ItemType.Radio] = 1,
            [ItemType.KeycardZoneManager] = 1,
            [ItemType.ArmorLight] = 1,
            [ItemType.Medkit] = 1,
        };

        public override bool ClearInventoryOnSpawn => true;

        public override bool AllowKeycardDoors => true;

        public override List<RoomName> SpawnRooms => new List<RoomName>() 
        {
            RoomName.LczCheckpointA, 
            RoomName.LczCheckpointB
        };
    }
}
