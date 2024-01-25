using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using MEC;
using UnityEngine;
using Interactables.Interobjects.DoorUtils;
using System.Linq;
using MapGeneration;
using System.Collections.Generic;
using Mirror;

namespace LurkBoisModded.EventHandlers.Map
{
    [EventHandler]
    public class DoorSpawnHandler
    {
        [PluginEvent(ServerEventType.MapGenerated)]
        public void OnRoundStart(MapGeneratedEvent ev)
        {
            Timing.CallDelayed(0.1f, () => 
            {
                Log.Info("Spawning Additional Doors!");
                if (Plugin.GetConfig().FacilityConfig.SpawnScp939Door)
                {
                    SpawnScp939Door();
                }
                if (Plugin.GetConfig().FacilityConfig.ModifyGr18)
                {
                    ModifyGr18Inner();
                }
            });
        }

        void ModifyGr18Inner()
        {
            if(!Utility.TryGetDoorByName("GR18_INNER", out DoorVariant door))
            {
                Log.Warning("Failed to find GR18_INNER.");
                return;
            }
            door.RequiredPermissions.RequiredPermissions.SetAll(false);
            foreach(KeycardPermissions perm in Plugin.GetConfig().FacilityConfig.Gr18KeycardPermissions)
            {
                door.RequiredPermissions.RequiredPermissions |= perm;
            }
            Log.Info("Modified GR18_INNER!");
        }

        void SpawnScp939Door()
        {
            if (!RoomIdUtils.TryFindRoom(RoomName.Hcz939, FacilityZone.HeavyContainment, RoomShape.Undefined, out RoomIdentifier room))
            {
                Log.Error("Failed to find Hcz939!");
                return;
            }
            List<DoorVariant> doorsIn939Room = DoorVariant.DoorsByRoom[room].ToList();
            DoorVariant targetDoor = null;
            foreach (DoorVariant door in doorsIn939Room)
            {
                if (!door.TryGetComponent<DoorNametagExtension>(out var nametag))
                {
                    continue;
                }
                else
                {
                    if (nametag.GetName == "939_CRYO")
                    {
                        targetDoor = door;
                    }
                }
            }
            if (targetDoor == null)
            {
                Log.Error("Failed to locate door.");
                return;
            }
            Vector3 newScale = targetDoor.transform.localScale;
            newScale.y *= 1.45f;
            newScale.z *= 2f;
            newScale.x *= 1.75f;
            DoorVariant result = Utility.CreateDoor(targetDoor.transform.position, targetDoor.transform.rotation, newScale, DoorType.LCZ, Plugin.GetConfig().FacilityConfig.Scp939DoorKeycardRequirements );
            if (Plugin.GetConfig().FacilityConfig.Scp939DoorDefaultOpen)
            {
                result.SetDoorState(DoorState.Open);
            }
            DoorNametagExtension extension = result.gameObject.AddComponent<DoorNametagExtension>();
            extension.UpdateName("939_CRYO");
            NetworkServer.Destroy(targetDoor.gameObject);
            Log.Info("Spawned SCP 939's CC Door");
        }
    }
}
