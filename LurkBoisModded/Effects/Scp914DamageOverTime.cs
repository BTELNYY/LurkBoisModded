using CustomPlayerEffects;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class Scp914DamageOverTime : TickingEffectBase
    {
        protected override void OnTick()
        {
            CustomReasonDamageHandler damageHandler = new CustomReasonDamageHandler(Plugin.GetConfig().Scp914Config.Scp914DamageOverTimeDeathReason, Plugin.GetConfig().Scp914Config.Scp914DamageOverTimeIntensity * Intensity);
            Hub.playerStats.DealDamage(damageHandler);
        }
    }
}
