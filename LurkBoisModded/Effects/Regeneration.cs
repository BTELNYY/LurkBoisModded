using CustomPlayerEffects;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class Regeneration : TickingEffectBase
    {
        public float DefaultRegenTick = 1f;

        public override EffectClassification Classification => EffectClassification.Positive;

        public override void OnTick()
        {
            float amount = DefaultRegenTick * Intensity;
            Player.Get(Hub).Heal(amount);
        }
    }
}
