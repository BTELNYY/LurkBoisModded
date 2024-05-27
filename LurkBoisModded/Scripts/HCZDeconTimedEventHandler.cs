using Interactables.Interobjects.DoorUtils;
using Interactables.Interobjects;
using LurkBoisModded.Base;
using LurkBoisModded.Extensions;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomPlayerEffects;
using System.Collections.ObjectModel;
using LurkBoisModded.Effects;

namespace LurkBoisModded.Scripts
{
    public class HCZDeconTimedEventHandler : TimedEventHandler
    {
        public override ReadOnlyCollection<TimedEvent> Events => new ReadOnlyCollection<TimedEvent>(new TimedEvent[]
        {
            new TimedEvent(Config.CurrentConfig.HCZDecontaminationConfig.SequenceStartTime, 60f, () =>
            {
                Cassie.Message("Danger Heavy Containment Zone Decontamination in T minus 2 minutes", isSubtitles: true);
            }),
            new TimedEvent(30f, () =>
            {
                Cassie.Message("Danger Heavy Containment Zone Decontamination in T minus 1 minute", isSubtitles: true);
            }),
            new TimedEvent(30f, () =>
            {
                Cassie.Message("Danger Heavy Containment Zone Decontamination in T minus 30 seconds, Please evacuate immediately", isSubtitles: true);
                //Broadcast.Singleton.RpcAddElement("HCZ Is going to get decontaminated in 30 seconds, RUN!", 30, Broadcast.BroadcastFlags.Normal);
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
            }),
            new TimedEvent(0f, () => 
            {
                Cassie.Message("Heavy Containment Zone is locked down and ready for decontamination", isSubtitles: true);
                //Broadcast.Singleton.RpcAddElement("HCZ is being decontaminated.", 15, Broadcast.BroadcastFlags.Normal);
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
            }),
            new TimedEvent(Config.CurrentConfig.HCZDecontaminationConfig.Length, true, () => 
            {
                foreach(ReferenceHub hub in ReferenceHub.AllHubs.Where(x => x.authManager.InstanceMode == CentralAuth.ClientInstanceMode.ReadyClient && x.roleManager.CurrentRole.RoleTypeId != PlayerRoles.RoleTypeId.Scp079))
                {
                    Player p = Player.Get(hub);
                    if(!p.IsAlive)
                    {
                        continue;
                    }
                    if(p.Zone == MapGeneration.FacilityZone.HeavyContainment)
                    {
                        p.EffectsManager.EnableEffect<FakeDecontaminating>(0, false);
                    }
                    else
                    {
                        p.EffectsManager.DisableEffect<FakeDecontaminating>();
                    }
                }
            }),
            new TimedEvent(0f, () => 
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
                Cassie.Message("Heavy Containment Zone Decontamination Complete", isSubtitles: true);
            })
        });
    }
}
