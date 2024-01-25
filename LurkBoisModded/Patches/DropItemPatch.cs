using HarmonyLib;
using InventorySystem;
using InventorySystem.Items.Pickups;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(InventoryExtensions), nameof(InventoryExtensions.ServerDropItem))]
    public class DropItemPatch
    {
        public static event Action<ItemPickupBase, ReferenceHub> OnItemDropped;

        public static void Postfix(Inventory inv, ushort itemSerial, ref ItemPickupBase __result)
        {
            OnItemDropped?.Invoke(__result, (ReferenceHub)AccessTools.Field(typeof(Inventory), "_hub").GetValue(inv));
        }
    }
}
