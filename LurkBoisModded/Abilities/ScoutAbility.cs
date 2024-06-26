﻿using LurkBoisModded.Base.Ability;
using LurkBoisModded.Base;
using System;
using PluginAPI.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Managers;
using LurkBoisModded.Effects;
using CustomPlayerEffects;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.Abilities
{
    public class ScoutAbility : CustomCooldownAbility
    {
        public override AbilityType AbilityType => AbilityType.Scout;

        public override float Cooldown => Plugin.GetConfig().AbilityConfig.ScoutAbilityConfig.Cooldown;

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (!CooldownReady)
            {
                return;
            }
            float duration = 0;
            byte healthReduction = 0;
            byte speedBoost = 0;
            foreach(EffectDefinition statusEffect in Plugin.GetConfig().AbilityConfig.ScoutAbilityConfig.Effects)
            {
                string effectName = statusEffect.Name;
                if(!CurrentOwner.playerEffectsController.TryGetEffect(effectName, out var effect))
                {
                    Log.Warning("Failed to find status effect by name. Name: " + effectName);
                    continue;
                }
                CurrentOwner.playerEffectsController.ChangeState(effectName, statusEffect.Intensity, statusEffect.Duration);
                if(effectName == nameof(MovementBoost))
                {
                    speedBoost = statusEffect.Intensity;
                    duration = effect.Duration;
                }
            }
            CurrentOwner.playerEffectsController.ChangeState<MaxHealthReduction>(Plugin.GetConfig().AbilityConfig.ScoutAbilityConfig.HealthReductionAmount, Plugin.GetConfig().AbilityConfig.ScoutAbilityConfig.HealthReductionDuration);
            healthReduction = Plugin.GetConfig().AbilityConfig.ScoutAbilityConfig.HealthReductionAmount;
            CurrentOwner.SendHint(Plugin.GetConfig().AbilityConfig.ScoutAbilityConfig.AbilityUsed.Replace("{intensity}", healthReduction.ToString()).Replace("{duration}", duration.ToString()).Replace("{speed}", speedBoost.ToString()));
        }
    }
}
