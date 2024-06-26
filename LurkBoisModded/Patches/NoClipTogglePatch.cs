﻿using HarmonyLib;
using NorthwoodLib.Pools;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using System.Collections.Generic;
using System.Reflection.Emit;
using LurkBoisModded.Managers;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
//Taken from https://github.com/Jesus-QC/ScpChatExtension/tree/master, modified to suit my needs.
namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(FpcNoclipToggleMessage), nameof(FpcNoclipToggleMessage.ProcessMessage))]
    public class NoClipTogglePatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            Label ret = generator.DefineLabel();
            newInstructions[newInstructions.Count - 1].labels.Add(ret);
            int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Ret) + 1;
            newInstructions.InsertRange(index, new CodeInstruction[]
            {
                new CodeInstruction(OpCodes.Ldloc_0).MoveLabelsFrom(newInstructions[index]),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(AbilityManager), nameof(AbilityManager.OnPlayerTogglingNoClip))),
                new CodeInstruction(OpCodes.Brfalse, ret),
            });
            foreach (CodeInstruction instruction in newInstructions)
            {
                yield return instruction;
            }
            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
