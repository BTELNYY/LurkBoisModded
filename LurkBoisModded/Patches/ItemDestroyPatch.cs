using HarmonyLib;
using InventorySystem.Items.Pickups;
using LurkBoisModded.Base;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.EventHandlers.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches
{
    //[HarmonyPatch(typeof(ItemPickupBase), "DestroySelf")]
    public class ItemDestroyPatch
    {
        public static void Prefix(ItemPickupBase __instance)
        {
            if (CustomItemHandler.SerialToItem.ContainsKey(__instance.Info.Serial))
            {
                CustomItem item = CustomItemHandler.SerialToItem[__instance.Info.Serial];
                item.OnItemDestroyed();
            }
        }
    }
}
