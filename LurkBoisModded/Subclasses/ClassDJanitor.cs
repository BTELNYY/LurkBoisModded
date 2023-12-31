﻿using LurkBoisModded.Base;
using MapGeneration;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class ClassDJanitor : Subclass
    {
        public override string FileName => "classd_janitor";

        public override string ClassColor => "orange";

        public override string SubclassNiceName => "D Class Janitor";

        public override string SubclassDescription => "Maybe cleaning wasn't so bad after all. You have a few items, use them!";

        public override RoleTypeId Role => RoleTypeId.ClassD;

        public override Dictionary<ItemType, ushort> SpawnItems => new Dictionary<ItemType, ushort>() 
        {
            [ItemType.KeycardJanitor] = 1
        };

        public override Dictionary<ItemType, ushort> RandomItems => new Dictionary<ItemType, ushort>() 
        {
            [ItemType.Flashlight] = 1,
            [ItemType.Coin] = 2,
            [ItemType.Painkillers] = 1,
        };

        public override int NumberOfRandomItems => 1;

        public override List<RoomName> SpawnRooms => new List<RoomName>() 
        {
            RoomName.LczClassDSpawn,
            RoomName.LczComputerRoom,
            RoomName.Lcz173,
            RoomName.Lcz330,
            RoomName.LczAirlock,
            RoomName.LczArmory,
        };
    }
}
