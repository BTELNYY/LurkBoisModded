using HarmonyLib;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using LightContainmentZoneDecontamination;
using MEC;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(DecontaminationController), "UpdateTime")]
    public class LczDeconDoorPatch
    {
        public static void Prefix(DecontaminationController __instance)
        {
            int nextPhase = __instance._nextPhase;
            if (NetworkServer.active && __instance.DecontaminationPhases[nextPhase].Function == DecontaminationController.DecontaminationPhase.PhaseFunction.OpenCheckpoints)
            {
                Timing.CallDelayed(10f, () =>
                {
                    if(__instance._curFunction != DecontaminationController.DecontaminationPhase.PhaseFunction.OpenCheckpoints)
                    {
                        return;
                    }
                    List<DoorVariant> lczDoors = Utility.GetDoorsByZone(MapGeneration.FacilityZone.LightContainment);
                    foreach (DoorVariant door in lczDoors)
                    {
                        if (door is CheckpointDoor)
                        {
                            continue;
                        }
                        if (door is ElevatorDoor)
                        {
                            continue;
                        }
                        door.ServerChangeLock(DoorLockReason.DecontEvacuate, true);
                        door.NetworkTargetState = true;
                    }
                });
            }
        }
    }
}
