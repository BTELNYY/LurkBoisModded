using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using System.Threading.Tasks;
using InventorySystem.Items.ThrowableProjectiles;
using LurkBoisModded.EventHandlers.Item;

namespace LurkBoisModded.Patches.Scp2176
{
    [HarmonyPatch(typeof(Scp2176Projectile), "ServerFuseEnd")]
    public class Scp2176Patch
    {
        public static bool Prefix(Scp2176Projectile __instance)
        {
            return CustomItemHandler.OnGrenadeFuseEnd(__instance);
        }
    }
}
