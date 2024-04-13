using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using Interactables.Interobjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Extensions
{
    public static class DoorExtensions
    {
        public static void SetDoorState(this DoorVariant door, DoorState state)
        {
            if (state == DoorState.Open)
            {
                door.NetworkTargetState = true;
            }
            else
            {
                door.NetworkTargetState = false;
            }
            if (door is CheckpointDoor)
            {
                CheckpointDoor checkPointDoor = (CheckpointDoor)door;
                AccessTools.Method(typeof(CheckpointDoor), "UpdateSequence").Invoke(checkPointDoor, null);
            }
        }
        public static void SetDoorState(this DoorVariant door, DoorState state, bool locked, DoorLockReason reason = DoorLockReason.AdminCommand)
        {
            if (state == DoorState.Open)
            {
                door.TargetState = true;
            }
            else
            {
                door.TargetState = false;
            }
            door.ServerChangeLock(reason, locked);
            if (door is CheckpointDoor)
            {
                CheckpointDoor checkPointDoor = (CheckpointDoor)door;
                AccessTools.Method(typeof(CheckpointDoor), "UpdateSequence").Invoke(checkPointDoor, null);
            }
        }
    }
}
