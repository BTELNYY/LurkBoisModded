using HarmonyLib;
using Mirror;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using VoiceChat.Networking;
using NorthwoodLib.Pools;
using LurkBoisModded.Abilities;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(VoiceTransceiver), "ServerReceiveMessage")]
    public class VoiceChatPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            Label ret = generator.DefineLabel();
            newInstructions[newInstructions.Count - 1].labels.Add(ret);
            int index = newInstructions.FindLastIndex(x => x.opcode == OpCodes.Ldloc_0);
            newInstructions.InsertRange(index, new[]
            {
                new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(newInstructions[index]),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(ProximityChatAbility), nameof(ProximityChatAbility.OnPlayerUsingVoiceChat))),
                new CodeInstruction(OpCodes.Brfalse_S, ret),
            });
            foreach (CodeInstruction instruction in newInstructions)
            {
                yield return instruction;
            }
            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
