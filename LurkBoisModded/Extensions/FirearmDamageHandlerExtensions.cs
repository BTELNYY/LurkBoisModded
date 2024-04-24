using LurkBoisModded.Patches.DamageHandler;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Profiling;

namespace LurkBoisModded.Extensions
{
    public static class FirearmDamageHandlerExtensions
    {
        public static void SetKnockbackMultiplier(this FirearmDamageHandler handler, float multiplier)
        {
            if(FirearmDamageHandlerProccessRagdollPatch.HandlerToForceMultiplierDict.ContainsKey(handler))
            {
                FirearmDamageHandlerProccessRagdollPatch.HandlerToForceMultiplierDict[handler] = multiplier;
            }
            else
            {
                FirearmDamageHandlerProccessRagdollPatch.HandlerToForceMultiplierDict.Add(handler, multiplier);
            }
        }

        public static void SetKnockbackAddative(this FirearmDamageHandler handler, float addAmount)
        {
            if (FirearmDamageHandlerProccessRagdollPatch.HandlerToForceDict.ContainsKey(handler))
            {
                FirearmDamageHandlerProccessRagdollPatch.HandlerToForceDict[handler] = addAmount;
            }
            else
            {
                FirearmDamageHandlerProccessRagdollPatch.HandlerToForceDict.Add(handler, addAmount);
            }
        }

        public static void SetKnockback(this FirearmDamageHandler handler, float amount)
        {
            handler.SetKnockbackMultiplier(0);
            handler.SetKnockbackAddative(amount);
        }
    }
}
