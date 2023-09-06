using HarmonyLib;
using Hints;
using InventorySystem.Disarming;
using PluginAPI.Core;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(DisarmedPlayers), nameof(DisarmedPlayers.CanDisarm))]
    public class DisarmPatch
    {
        public static bool Prefix(ReferenceHub disarmerHub, ReferenceHub targetHub, ref bool __result)
        {
            int currentDisarmedPlayers = disarmerHub.GetAllDisarmedPlayersByDisarmer().Count;
            ItemType currentItemType = disarmerHub.inventory.CurItem.TypeId;
            if (Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType] == -1)
            {
                return true;
            }
            if (Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType] <= currentDisarmedPlayers && !targetHub.inventory.IsDisarmed())
            {
                string message = Plugin.GetConfig().MaxDisarmsReached.Replace("{count}", Plugin.GetConfig().MaxDisarmsPerWeapon[currentItemType].ToString());
                disarmerHub.SendHint(message);
                __result = false;
                return false;
            }
            return true;
        }
    }
}
