using HarmonyLib;
using LurkBoisModded.Extensions;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Profiling;
using UnityEngine;

namespace LurkBoisModded.Patches.DamageHandler
{
    [HarmonyPatch(typeof(FirearmDamageHandler), nameof(FirearmDamageHandler.ProcessRagdoll))]
    public class FirearmDamageHandlerProccessRagdollPatch
    {
        public static Dictionary<FirearmDamageHandler, float> HandlerToForceDict = new Dictionary<FirearmDamageHandler, float>();

        public static Dictionary<FirearmDamageHandler, float> HandlerToForceMultiplierDict = new Dictionary<FirearmDamageHandler, float>();

        public static bool Prefix(FirearmDamageHandler __instance, BasicRagdoll ragdoll)
        {
            float num;
            sbyte hitDirX = (sbyte)AccessTools.Field(typeof(FirearmDamageHandler), "_hitDirectionX").GetValue(__instance);
            sbyte hitDirZ = (sbyte)AccessTools.Field(typeof(FirearmDamageHandler), "_hitDirectionZ").GetValue(__instance);
            ItemType ammoType = (ItemType)AccessTools.Field(typeof(FirearmDamageHandler), "_ammoType").GetValue(__instance);
            Dictionary<HitboxType, float> dict = __instance.GetHitboxForce();
            if (!dict.TryGetValue(__instance.Hitbox, out num))
            {
                return false;
            }
            DynamicRagdoll dynamicRagdoll = ragdoll as DynamicRagdoll;
            if (dynamicRagdoll == null)
            {
                return false;
            }
            float num3;
            float addedKnockback = 0f;
            if (HandlerToForceDict.ContainsKey(__instance))
            {
                addedKnockback = HandlerToForceDict[__instance];
                HandlerToForceDict.Remove(__instance );
            }
            float multiplier = 1f;
            if (HandlerToForceMultiplierDict.ContainsKey(__instance))
            {
                multiplier = HandlerToForceMultiplierDict[__instance];
                HandlerToForceMultiplierDict.Remove(__instance );
            }
            float num2 = num * (__instance.GetForceByAmmoType().TryGetValue(ammoType, out num3) ? num3 : 1f);
            num2 = (num2 * multiplier) + addedKnockback;
            Rigidbody[] linkedRigidbodies = dynamicRagdoll.LinkedRigidbodies;
            for (int i = 0; i < linkedRigidbodies.Length; i++)
            {
                linkedRigidbodies[i].AddForce(num2 * 127f * 0.1f * Vector3.up, ForceMode.VelocityChange);
            }
            Vector3 vector = new Vector3((float)hitDirX, 0f, (float)hitDirZ * num2);
            foreach (HitboxData hitboxData in dynamicRagdoll.Hitboxes)
            {
                if (hitboxData.RelatedHitbox == __instance.Hitbox)
                {
                    hitboxData.Target.AddForce(vector, ForceMode.VelocityChange);
                }
            }
            return false;
        }
    }
}
