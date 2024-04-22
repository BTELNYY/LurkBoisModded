using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Extensions
{
    public static class RoomExtensions
    {
        public static ElevatorDoor[] GetElevatorDoors(this RoomIdentifier roomIdentifier)
        {
            if(roomIdentifier == null)
            {
                return new ElevatorDoor[] { };
            }
            IEnumerable<RoomLightController> enumerable = from x in RoomLightController.Instances
                                                          where x.Room == roomIdentifier
            select x;

            HashSet<DoorVariant> doors;
            if (!DoorVariant.DoorsByRoom.TryGetValue(roomIdentifier, out doors))
            {
                return new ElevatorDoor[] { };
            }
            List<ElevatorDoor> elevDoors = new List<ElevatorDoor>();
            foreach(DoorVariant door in doors)
            {
                if(door is ElevatorDoor)
                {
                    elevDoors.Add(door as ElevatorDoor);
                }
            }
            return elevDoors.ToArray();
        }
    }
}
