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
    [HarmonyPatch(typeof(ExplosionGrenade), "ServerFuseEnd")]
    public class HEGrenadePatch
    {
        public static bool Prefix(ExplosionGrenade __instance)
        {
            return CustomItemHandler.OnGrenadeFuseEnd(__instance);
        }
    }
}
