using CustomPlayerEffects;
using LurkBoisModded.Extensions;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class Radiation : TickingEffectBase, IHealablePlayerEffect, IStaminaModifier
    {
        public int CurrentExposure = 0;
        private int _currentTicks = 0;
        public float MinDamage = Config.CurrentConfig.RadiationConfig.MinDamage;
        public float CurrentDamage = Config.CurrentConfig.RadiationConfig.MinDamage;
        public int MultiplyDamageEveryTicks = Config.CurrentConfig.RadiationConfig.MultiplyDamageEveryTicks;
        public float DamageMultiplier = Config.CurrentConfig.RadiationConfig.DamageMultiplier;

        public bool StaminaModifierActive => CurrentExposure > Config.CurrentConfig.RadiationConfig.MaxExposure;

        public float StaminaUsageMultiplier => 1f;

        public float StaminaRegenMultiplier
        {
            get
            {
                if (Hub.playerStats.StatModules[2].CurValue > 0.5f && _currentTicks > 20)
                {
                    return 0f;
                }
                if(_currentTicks > 30)
                {
                    return 0.75f;
                }
                if(_currentTicks > 40)
                {
                    return 0.5f;
                }
                if(_currentTicks > 50)
                {
                    return 0.5f;
                }
                return 1f;
            }
        }

        public bool SprintingDisabled => _currentTicks > 80;

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
            if(CurrentExposure < Config.CurrentConfig.RadiationConfig.MaxExposure)
            {
                return;
            }
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

        public bool IsHealable(ItemType item)
        {
            if(item == ItemType.SCP500)
            {
                CurrentExposure = 0;
                return true;
            }
            return false;
        }
    }
}
