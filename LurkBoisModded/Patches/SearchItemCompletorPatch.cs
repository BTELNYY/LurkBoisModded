using HarmonyLib;
using InventorySystem.Items.Pickups;
using InventorySystem.Searching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(ItemSearchCompletor), nameof(ItemSearchCompletor.Complete))]
    public class SearchItemCompletorPatch
    {
        public static event Action<ItemPickupBase, ReferenceHub> OnItemPickedUp;

        public static void Postfix(ItemSearchCompletor __instance)
        {
            ReferenceHub owner = (ReferenceHub)AccessTools.Field(typeof(SearchCompletor), "Hub").GetValue(__instance);
            ItemPickupBase pickup = (ItemPickupBase)AccessTools.Field(typeof(ItemSearchCompletor), "TargetPickup").GetValue(__instance);
            OnItemPickedUp?.Invoke(pickup, owner);
        }
    }
}
