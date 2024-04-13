using CustomPlayerEffects;
using HarmonyLib;
using System;
using PluginAPI.Core;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Effects;
using System.Reflection;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(PlayerEffectsController), "Awake")]
    public class EffectPatch
    {
        public static void Prefix(PlayerEffectsController __instance)
        {
            try
            {
                var effectsObjectField = AccessTools.Field(typeof(PlayerEffectsController), "effectsGameObject");
                GameObject effectsObject = (GameObject)effectsObjectField.GetValue(__instance);
                foreach (var effect in CustomEffectManager.CustomEffects)
                {
                    foreach (Transform t in effectsObject.transform)
                    {
                        if (t.name == effect.Name)
                        {
                            continue;
                        }
                    }
                    GameObject newObj = GameObject.Instantiate(new GameObject(), Vector3.zero, Quaternion.Euler(0, 0, 0), effectsObject.transform);
                    newObj.name = effect.Name;
                    if (newObj.GetComponent(effect) == null)
                    {
                        newObj.AddComponent(effect);
                    }
                    Log.Info($"Created Custom Effect '{effect.Name}'");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
