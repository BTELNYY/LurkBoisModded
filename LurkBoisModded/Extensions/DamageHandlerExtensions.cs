using HarmonyLib;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Extensions
{
    public static class DamageHandlerExtensions
    {
        public static void SetDamage(this StandardDamageHandler handler, float damage)
        {
            AccessTools.PropertySetter(typeof(StandardDamageHandler), nameof(StandardDamageHandler.Damage)).Invoke(handler, new object[] { damage });
        }
    }
}
