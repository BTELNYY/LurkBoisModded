using HarmonyLib;
using InventorySystem.Items.Pickups;
using LurkBoisModded.Base;
using LurkBoisModded.Managers;
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
            if (CustomItemManager.SerialToItem.ContainsKey(__instance.Info.Serial))
            {
                CustomItem item = CustomItemManager.SerialToItem[__instance.Info.Serial];
                item.OnItemDestroyed();
            }
        }
    }
}
