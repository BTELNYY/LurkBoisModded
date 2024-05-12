using Interactables.Interobjects.DoorUtils;
using MapGeneration;
using MEC;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Effects;
using UnityEngine;

namespace LurkBoisModded.EventHandlers.Map
{
    [EventHandler]
    public class RadiationHandler
    {
        //Controls the intensity of the effect.
        public static byte CurrentMode = 0;
        //Used to find if the player is still within the HCZ_Warhead room
        public static DoorVariant NukeDoor = null;

        //Controls checking for where the player is
        public float HigherBound = 0f;
        public float LowerBound = 0f;

        [PluginEvent(ServerEventType.WarheadStop)]
        public void OnWarheadCancelled(WarheadStopEvent ev)
        {
            if (AlphaWarheadController.TimeUntilDetonation > 80)
            {
                CurrentMode = 0;
            }
            if (AlphaWarheadController.TimeUntilDetonation < 80 && AlphaWarheadController.TimeUntilDetonation > 50)
            {
                CurrentMode = 1;
            }
            if (AlphaWarheadController.TimeUntilDetonation < 50 && AlphaWarheadController.TimeUntilDetonation > 30)
            {
                CurrentMode = 2;
            }
            if (AlphaWarheadController.TimeUntilDetonation < 30)
            {
                CurrentMode = 3;
            }
        }

        [PluginEvent(ServerEventType.RoundStart)]
        public void OnRoundStart(RoundStartEvent ev)
        {
            //Reset vars
            CurrentMode = 0;
            //Find the door
            List<RoomIdentifier> searchResults = RoomIdUtils.FindRooms(RoomName.HczWarhead, FacilityZone.HeavyContainment, RoomShape.Undefined).ToList();
            RoomIdentifier warheadRoom = searchResults.First();
            HashSet<DoorVariant> warheadDoors = DoorVariant.DoorsByRoom[warheadRoom];
            DoorVariant armoryDoor = warheadDoors.Where(x => x.TryGetComponent<DoorNametagExtension>(out DoorNametagExtension name) && name.GetName == "NUKE_ARMORY").FirstOrDefault();
            if (armoryDoor == null)
            {
                Log.Error("Can't find NUKE ARMORY!");
            }
            NukeDoor = armoryDoor;
            //Basically making sure the player is within these bounds
            HigherBound = armoryDoor.transform.localPosition.y + 10f;
            LowerBound = armoryDoor.transform.localPosition.y - 10f;
            //Log.Debug("Starting Loop!");
            Timing.CallPeriodically(float.MaxValue, Plugin.GetConfig().RadiationConfig.CheckInterval, () =>
            {
                CheckPlayers();
            });
        }

        private void CheckPlayers()
        {
            //Log.Debug("Updating Players!");
            HashSet<ReferenceHub> affectedPlayers = new HashSet<ReferenceHub>();
            foreach (ReferenceHub hub in ReferenceHub.AllHubs)
            {
                RoomIdentifier room = RoomIdUtils.FindRooms(RoomName.HczWarhead, FacilityZone.HeavyContainment, RoomShape.Undefined).First();
                if (RoomIdUtils.IsWithinRoomBoundaries(room, hub.gameObject.transform.position))
                {
                    //Log.Debug("Player in nuke room: " + hub.nicknameSync.DisplayName);
                }
                else
                {
                    continue;
                }
                Player target = Player.Get(hub);
                if (!target.ReferenceHub.playerEffectsController.TryGetEffect<Radiation>(out Radiation playerEffect))
                {
                    continue;   
                }
                if (hub.transform.position.y > -999f && target.Zone == FacilityZone.HeavyContainment)
                {
                    playerEffect.CurrentExposure += 1;
                }
                else
                {
                    playerEffect.CurrentExposure -= 1;
                }
            }
        }
    }
}
