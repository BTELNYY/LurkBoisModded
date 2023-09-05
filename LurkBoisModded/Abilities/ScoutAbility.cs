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

namespace LurkBoisModded.Abilities
{
    public class ScoutAbility : CustomCooldownAbilityBase
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
                if(!CurrentHub.playerEffectsController.TryGetEffect(effectName, out var effect))
                {
                    Log.Warning("Failed to find status effect by name. Name: " + effectName);
                    continue;
                }
                CurrentHub.playerEffectsController.ChangeState(effectName, statusEffect.Intensity, statusEffect.Duration);
                if(effectName == nameof(MovementBoost))
                {
                    speedBoost = statusEffect.Intensity;
                    duration = effect.Duration;
                }
                if(effectName == nameof(MaxHealthReduction))
                {
                    healthReduction = statusEffect.Intensity;
                }
            }
            CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.ScoutAbilityConfig.AbilityUsed.Replace("{intensity}", healthReduction.ToString()).Replace("{duration}", duration.ToString()).Replace("{speed}", speedBoost.ToString()));
        }
    }
}
