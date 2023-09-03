using HarmonyLib;
using InventorySystem.Items.Usables.Scp330;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches.Scp330
{
    [HarmonyPatch(typeof(CandyPink), nameof(CandyPink.SpawnChanceWeight), MethodType.Getter)]
    public class PinkCandyPatch
    {
        public static void Postfix(ref float __result)
        {
            if (!Plugin.GetConfig().Scp330Config.EnablePinkCandy)
            {
                __result = 0;
                return;
            }
            __result = Plugin.GetConfig().Scp330Config.PinkCandyChance;
        }
    }
}
