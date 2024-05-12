using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Enums;
using PluginAPI.Events;
using PluginAPI.Core.Attributes;
using MEC;
using PluginAPI.Core;
using Interactables.Interobjects.DoorUtils;
using LurkBoisModded.Extensions;
using CustomPlayerEffects;
using Interactables.Interobjects;
using System.Security.Policy;

namespace LurkBoisModded.EventHandlers.Map
{
    [EventHandler]
    public class HCZDeconHandler
    {
        [PluginEvent(ServerEventType.RoundStart)]
        public void RoundStart(RoundStartEvent ev)
        {
            Timing.CallDelayed(Config.CurrentConfig.HCZDecontaminationConfig.SequenceStartTime, () => 
            {
                BeginSequence();
            });
        }

        private void BeginSequence()
        {
            Cassie.Message("Danger Heavy Containment Zone Decontamination in T minus 3 minutes");
            Broadcast.Singleton.RpcAddElement("HCZ Is going to get decontaminated in 3 minutes", 15, Broadcast.BroadcastFlags.Normal);
            Timing.CallDelayed(120f, () => 
            {
                HczOneMinWarning();
            });
        }

        private void HczOneMinWarning()
        {
            Cassie.Message("Danger Heavy Containment Zone Decontamination in T minus 1 minute");
            Broadcast.Singleton.RpcAddElement("HCZ Is going to get decontaminated in 1 minute", 15, Broadcast.BroadcastFlags.Normal);
            Timing.CallDelayed(30f, () =>
            {
                float duration = Cassie.CalculateDuration("Danger Heavy Containment Zone Decontamination in T minus 30 seconds, Please evacuate immediately");
                Cassie.Message("Danger Heavy Containment Zone Decontamination in T minus 30 seconds, Please evacuate immediately");
                Broadcast.Singleton.RpcAddElement("HCZ Is going to get decontaminated in 30 seconds, RUN!", 30, Broadcast.BroadcastFlags.Normal);
                DoorVariant[] allDoorsToOpen = DoorVariant.AllDoors.Where(x => x.IsInZone(MapGeneration.FacilityZone.HeavyContainment)).ToArray();
                foreach (DoorVariant door in allDoorsToOpen)
                {
                    DoorLockReason activeLocks = (DoorLockReason)door.ActiveLocks;
                    if (activeLocks.HasFlag(DoorLockReason.SpecialDoorFeature))
                    {
                        continue;
                    }
                    if (door is ElevatorDoor)
                    {
                        continue;
                    }
                    door.SetDoorState(DoorState.Open);
                    door.ServerChangeLock(DoorLockReason.DecontEvacuate, true);
                }
            });
            Timing.CallDelayed(60f, () =>
            {
                Cassie.Message("Heavy Containment Zone is locked down and ready for decontamination");
                Broadcast.Singleton.RpcAddElement("HCZ is being decontaminated.", 15, Broadcast.BroadcastFlags.Normal);
                DoorVariant[] allDoorsToOpen = DoorVariant.AllDoors.Where(x => x.IsInZone(MapGeneration.FacilityZone.HeavyContainment)).ToArray();
                foreach (DoorVariant door in allDoorsToOpen)
                {
                    DoorLockReason activeLocks = (DoorLockReason)door.ActiveLocks;
                    if (activeLocks.HasFlag(DoorLockReason.SpecialDoorFeature))
                    {
                        continue;
                    }
                    door.ServerChangeLock(DoorLockReason.DecontEvacuate, false);
                    door.ServerChangeLock(DoorLockReason.DecontLockdown, true);
                    door.SetDoorState(DoorState.Close);
                }
                KillPlayers();
                UnlockHCZ();
            });
        }

        private void UnlockHCZ()
        {
            Timing.CallDelayed(Config.CurrentConfig.HCZDecontaminationConfig.Length, () =>
            {
                DoorVariant[] allDoorsToOpen = DoorVariant.AllDoors.Where(x => x.IsInZone(MapGeneration.FacilityZone.HeavyContainment)).ToArray();
                foreach (DoorVariant door in allDoorsToOpen)
                {
                    DoorLockReason activeLocks = (DoorLockReason)door.ActiveLocks;
                    if (activeLocks.HasFlag(DoorLockReason.SpecialDoorFeature))
                    {
                        continue;
                    }
                    door.ServerChangeLock(DoorLockReason.DecontLockdown, false);
                }
                Cassie.Message("Heavy Containment Zone Decontamination Complete");
                Broadcast.Singleton.RpcAddElement("HCZ is now unlocked.", 15, Broadcast.BroadcastFlags.Normal);
            });
        }

        private void KillPlayers()
        {
            Timing.CallPeriodically(Config.CurrentConfig.HCZDecontaminationConfig.Length, 1f, () => 
            {
                foreach(ReferenceHub hub in ReferenceHub.AllHubs.Where(x => x.authManager.InstanceMode == CentralAuth.ClientInstanceMode.ReadyClient && x.roleManager.CurrentRole.RoleTypeId != PlayerRoles.RoleTypeId.Scp079))
                {
                    Player p = Player.Get(hub);
                    if(p.Zone == MapGeneration.FacilityZone.HeavyContainment)
                    {
                        p.EffectsManager.ChangeState<Decontaminating>(1, 2f, false);
                    }
                }
            });
        }
    }
}
