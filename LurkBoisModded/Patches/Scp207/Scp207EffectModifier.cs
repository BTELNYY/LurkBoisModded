using HarmonyLib;
using InventorySystem.Items.Usables;
using LurkBoisModded.Effects;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches.Scp207
{
    [HarmonyPatch(typeof(InventorySystem.Items.Usables.Scp207), "OnEffectsActivated")]
    public class Scp207EffectModifier
    {
        public static bool Prefix(InventorySystem.Items.Usables.Scp207 __instance)
        {
            __instance.Owner.playerStats.GetModule<StaminaStat>().CurValue = 1f;
            __instance.Owner.playerStats.GetModule<HealthStat>().ServerHeal(30f);
            BetterScp207 scp;
            if (!__instance.Owner.playerEffectsController.TryGetEffect<BetterScp207>(out scp))
            {
                return false;
            }
            byte intensity = scp.Intensity;
            if (intensity >= 2)
            {
                return false;
            }
            __instance.Owner.playerEffectsController.ChangeState<BetterScp207>((byte)(intensity + 1), 0f, false);
            return false;
        }
    }
}
