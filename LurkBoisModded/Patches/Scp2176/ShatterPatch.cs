using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.ThrowableProjectiles;
using MapGeneration;
using Mirror;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches.Scp2176
{
    [HarmonyPatch(typeof(Scp2176Projectile), "ServerShatter")]
    public class ShatterPatch
    {
        public static void Postfix(Scp2176Projectile __instance)
        {
            if(!Config.CurrentConfig.Scp2176DisablesElevators)
            {
                return;
            }
            RoomIdentifier rid = RoomIdUtils.RoomAtPositionRaycasts(__instance.transform.position, true);
            if (rid == null)
            {
                return;
            }
            IEnumerable<RoomLightController> enumerable = from x in RoomLightController.Instances
                                                          where x.Room == rid
                                                          select x;

            HashSet<DoorVariant> doors;
            if (!DoorVariant.DoorsByRoom.TryGetValue(rid, out doors))
            {
                return;
            }
            foreach (DoorVariant door in doors)
            {
                if (door is ElevatorDoor)
                {
                    DoorLockMode mode = DoorLockUtils.GetMode((DoorLockReason)door.NetworkActiveLocks);
                    DoorLockReason reason = (DoorLockReason)door.NetworkActiveLocks;
                    if (reason == DoorLockReason.Lockdown2176)
                    {
                        door.ServerChangeLock(DoorLockReason.Lockdown2176, false);
                    }
                    else
                    {
                        door.ServerChangeLock(DoorLockReason.Lockdown2176, true);
                        door.UnlockLater(13f, DoorLockReason.Lockdown2176);
                    }
                }
            }
        }

        private static void LockDoors(IEnumerable<ElevatorDoor> doors)
        {
            bool inProgress = AlphaWarheadController.InProgress;
            foreach (ElevatorDoor doorVariant in doors)
            {
                INonInteractableDoor nonInteractableDoor = doorVariant as INonInteractableDoor;
                if (nonInteractableDoor == null || !nonInteractableDoor.IgnoreLockdowns)
                {
                    DoorLockReason activeLocks = (DoorLockReason)doorVariant.ActiveLocks;
                    if (!doorVariant.TargetState && (activeLocks.HasFlagFast(DoorLockReason.Lockdown079) || activeLocks.HasFlagFast(DoorLockReason.Lockdown2176) || activeLocks.HasFlagFast(DoorLockReason.Regular079)))
                    {
                        doorVariant.UnlockLater(0f, DoorLockReason.Lockdown2176);
                        if (!doorVariant.RequiredPermissions.Bypass2176)
                        {
                            doorVariant.NetworkTargetState = true;
                        }
                    }
                    else
                    {
                        DoorLockMode mode = DoorLockUtils.GetMode((DoorLockReason)doorVariant.ActiveLocks);
                        if (mode.HasFlagFast(DoorLockMode.CanClose) || mode.HasFlagFast(DoorLockMode.ScpOverride))
                        {
                            doorVariant.ServerChangeLock(DoorLockReason.Lockdown2176, true);
                            doorVariant.UnlockLater(13f, DoorLockReason.Lockdown2176);
                            if (!inProgress)
                            {
                                doorVariant.NetworkTargetState = false;
                            }
                        }
                    }
                }
            }
        }

    }
}
