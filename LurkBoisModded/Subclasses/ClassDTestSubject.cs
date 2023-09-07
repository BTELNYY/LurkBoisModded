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
    public class ClassDTestSubject : Subclass
    {
        public override string ClassColor => "orange";

        public override string FileName => "classd_test_subject";

        public override string SubclassNiceName => "Class D Test Subject";

        public override string SubclassDescription => "You were likely going to die, but the containment breach saved your life. Run!";

        public override List<RoomName> SpawnRooms => new List<RoomName>() 
        {
            RoomName.Lcz173,
            RoomName.Lcz914,
            RoomName.Lcz330,
            RoomName.LczGlassroom,
        };

        public override Dictionary<ItemType, ushort> RandomItems => new Dictionary<ItemType, ushort>() 
        {
            [ItemType.Flashlight] = 1,
            [ItemType.Coin] = 1,
            [ItemType.ArmorLight] = 1,
        };

        public override int NumberOfRandomItems => 1;
    }
}
