using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core;
using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PlayerRoles.PlayableScps.Scp079;
using Interactables.Interobjects;
using LurkBoisModded.Extensions;
using Interactables.Interobjects.DoorUtils;

namespace LurkBoisModded.EventHandlers.Scp079
{
    [EventHandler]
    public class Scp079Handler
    {
        [PluginEvent(ServerEventType.GeneratorActivated)]
        public void OnGeneratorActivated(GeneratorActivatedEvent ev)
        {
            if(Config.CurrentConfig.Scp079Config.Scp079LoseConnectionOnGeneratorActivation)
            {
                List<Player> Scp079Players = Utility.GetPlayersByRole(PlayerRoles.RoleTypeId.Scp079);
                foreach(Player player in Scp079Players)
                {
                    Scp079Role role = player.RoleBase as Scp079Role;
                    if(role == null)
                    {
                        continue;
                    }
                    if(!role.SubroutineModule.TryGetSubroutine<Scp079LostSignalHandler>(out var handler))
                    {
                        continue;
                    }
                    handler.ServerLoseSignal(Config.CurrentConfig.Scp079Config.Scp079ConnectionLostFromGenLength);
                }
            }
        }

        [PluginEvent(ServerEventType.Scp079LockdownRoom)]
        public void OnLockdown(Scp079LockdownRoomEvent ev)
        {
            if (!Config.CurrentConfig.Scp079Config.Scp079CanLockElevators)
            {
                return;
            }
            Scp079Role role = ev.Player.RoleBase as Scp079Role;
            if (role == null)
            {
                return;
            }
            if (!role.SubroutineModule.TryGetSubroutine<Scp079AuxManager>(out var handler))
            {
                return;
            }
            ElevatorDoor[] doors = ev.Room.GetElevatorDoors();
            if (doors.IsEmpty())
            {
                return;
            }
            foreach(var door in doors)
            {
                DoorLockMode mode = DoorLockUtils.GetMode((DoorLockReason)door.NetworkActiveLocks);
                DoorLockReason reason = (DoorLockReason)door.NetworkActiveLocks;
                if (reason.HasFlag(DoorLockReason.Warhead) || reason.HasFlag(DoorLockReason.DecontLockdown) || reason.HasFlag(DoorLockReason.AdminCommand))
                {
                    continue;
                }
                door.ServerChangeLock(DoorLockReason.Lockdown079, true);
                door.UnlockLater(13f, DoorLockReason.Lockdown079);
                handler.CurrentAux -= 5f;
            }
        }

        [PluginEvent(ServerEventType.Scp079CancelRoomLockdown)]
        public void OnLockdownLifted(Scp079CancelRoomLockdownEvent ev)
        {
            if (!Config.CurrentConfig.Scp079Config.Scp079CanLockElevators)
            {
                return;
            }
            ElevatorDoor[] doors = ev.Room.GetElevatorDoors();
            if (doors.IsEmpty())
            {
                return;
            }
            foreach (var door in doors)
            {
                DoorLockMode mode = DoorLockUtils.GetMode((DoorLockReason)door.NetworkActiveLocks);
                DoorLockReason reason = (DoorLockReason)door.NetworkActiveLocks;
                if (reason.HasFlag(DoorLockReason.Warhead) || reason.HasFlag(DoorLockReason.DecontLockdown) || reason.HasFlag(DoorLockReason.AdminCommand))
                {
                    continue;
                }
                door.ServerChangeLock(DoorLockReason.Lockdown079, false);
            }
        }
    }
}
