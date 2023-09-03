using CustomPlayerEffects;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class BetterBleeding : TickingEffectBase
    {
        protected override void OnTick()
        {
            float damage = 2 * Intensity;
            UniversalDamageHandler handler = new UniversalDamageHandler(damage, DeathTranslations.Bleeding);
            Hub.playerStats.DealDamage(handler);
        }
    }
}
