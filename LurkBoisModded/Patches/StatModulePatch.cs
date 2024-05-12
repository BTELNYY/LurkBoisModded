using HarmonyLib;
using LurkBoisModded.StatModules;
using PlayerStatsSystem;
using System;
using PluginAPI.Core;
using System.Collections.Generic;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(PlayerStats), "Start")]
    public class StatModulePatch
    {
        public static void Prefix(PlayerStats __instance)
        {
            try
            {
               Dictionary<Type, StatBase> dictTypes = AccessTools.FieldRefAccess<PlayerStats, Dictionary<Type, StatBase>>(__instance, "_dictionarizedTypes");
                ReferenceHub hub = ReferenceHub.GetHub(__instance.gameObject);
                __instance.StatModules[0] = new NewHealthStat();
                __instance.StatModules[1] = new NewAhpStat();
                dictTypes[PlayerStats.DefinedModules[0]] = __instance.StatModules[0];
                dictTypes[PlayerStats.DefinedModules[1]] = __instance.StatModules[1];
                for (int i = 0; i < __instance.StatModules.Length; i++)
                {
                    object[] obj = { hub };
                    AccessTools.PropertySetter(typeof(StatBase), nameof(StatBase.Hub)).Invoke(__instance.StatModules[i], obj);
                    if (__instance.StatModules[i] is IUpdatingStat)
                    {
                        IUpdatingStat updateStat = __instance.StatModules[i] as IUpdatingStat;
                        updateStat.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }

    [HarmonyPatch(typeof(PlayerStats), "Update")]
    public class StatUpdatePatch
    {
        public static void Prefix(PlayerStats __instance)
        {
            foreach(StatBase stat in __instance.StatModules)
            {
                if(stat is IUpdatingStat)
                {
                    IUpdatingStat update = stat as IUpdatingStat;
                    update.Update();
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerStats), "OnClassChanged")]
    public class StatClassChangePatch
    {
        public static void Prefix(PlayerStats __instance)
        {
            foreach (StatBase stat in __instance.StatModules)
            {
                if (stat is IUpdatingStat)
                {
                    IUpdatingStat update = stat as IUpdatingStat;
                    update.ClassChanged();
                }
            }
        }
    }
}
