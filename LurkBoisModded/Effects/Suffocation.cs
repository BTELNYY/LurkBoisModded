using CustomPlayerEffects;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class Suffocation : TickingEffectBase
    {
        protected override void OnTick()
        {
            float damage = 1 * Intensity;
            CustomReasonDamageHandler customReason = new CustomReasonDamageHandler(Plugin.GetConfig().Scp079Config.SuffocationDeathMessage, damage);
            Hub.playerStats.DealDamage(customReason);
        }
    }
}
