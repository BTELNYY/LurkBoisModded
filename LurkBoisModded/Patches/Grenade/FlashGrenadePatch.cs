using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using InventorySystem.Items.ThrowableProjectiles;
using LurkBoisModded.EventHandlers.Item;

namespace LurkBoisModded.Patches.Grenade
{
    [HarmonyPatch(typeof(FlashbangGrenade), "ServerFuseEnd")]
    public class FlashGrenadePatch
    {
        public static bool Prefix(FlashbangGrenade __instance)
        {
            return CustomItemHandler.OnGrenadeFuseEnd(__instance);
        }
    }
}
