using CustomPlayerEffects;
using LurkBoisModded.Extensions;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class Radiation : TickingEffectBase
    {
        private int _currentTicks = 0;
        public float MinDamage = Config.CurrentConfig.RadiationConfig.MinDamage;
        public float CurrentDamage = Config.CurrentConfig.RadiationConfig.MinDamage;
        public int MultiplyDamageEveryTicks = Config.CurrentConfig.RadiationConfig.MultiplyDamageEveryTicks;
        public float DamageMultiplier = Config.CurrentConfig.RadiationConfig.DamageMultiplier;

        protected override void Disabled()
        {
            base.Disabled();
            _currentTicks = 0;
        }

        protected override void Enabled()
        {
            base.Enabled();
            _currentTicks = 0;
            Hub.SendHint(Config.CurrentConfig.RadiationConfig.EffectHint);
            CurrentDamage = MinDamage * Intensity;
        }

        protected override void OnTick()
        {
            int multiplyAmount = (int)Math.Floor((float)(_currentTicks / MultiplyDamageEveryTicks));
            CurrentDamage = MinDamage * Intensity;
            for (int i = 0; i < multiplyAmount; i++)
            {
                CurrentDamage *= DamageMultiplier;
            }
            CustomReasonDamageHandler handler = new CustomReasonDamageHandler(Config.CurrentConfig.RadiationConfig.DeathMessage, CurrentDamage);
            Hub.playerEffectsController.ServerSendPulse<Poisoned>();
            Hub.playerStats.DealDamage(handler);
            _currentTicks++;
        }
    }
}
