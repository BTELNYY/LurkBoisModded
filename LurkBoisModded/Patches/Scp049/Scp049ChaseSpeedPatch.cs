using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches.Scp049
{
    [HarmonyPatch(typeof(Scp049MovementModule), "UpdateMovement")]
    public class Scp049ChaseSpeedPatch
    {
        public static void Prefix(Scp049MovementModule __instance)
        {
            var field = AccessTools.Field(typeof(Scp049MovementModule), "_enragedSpeed");
            field.SetValue(__instance, Plugin.GetConfig().Scp049Config.SprintSpeed);
        }
    }
}
