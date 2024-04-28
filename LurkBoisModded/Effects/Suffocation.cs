using CustomPlayerEffects;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class Suffocation : TickingEffectBase, IStaminaModifier
    {
        public bool StaminaModifierActive => true;

        public float StaminaUsageMultiplier
        {
            get
            {
                if(!IsEnabled) { return 1f; }
                StaminaStat staminaStat = Hub.playerStats.StatModules[2] as StaminaStat;
                if (staminaStat == null)
                {
                    return 1f;
                }
                if (staminaStat.CurValue > 0.25)
                {
                    return 2f;
                }
                return 1f;
            }
        }

        public float StaminaRegenMultiplier
        {
            get
            {
                if (!IsEnabled) { return 1f; }
                StaminaStat staminaStat = Hub.playerStats.StatModules[2] as StaminaStat;
                if(staminaStat == null)
                {
                    return 1f;
                }
                if(staminaStat.CurValue > 0.25)
                {
                    return 0f;
                }
                return 1f;
            }
        }

        public bool SprintingDisabled => false;

        protected override void Enabled()
        {
            base.Enabled();
            StaminaStat staminaStat = Hub.playerStats.StatModules[2] as StaminaStat;
            if (staminaStat == null)
            {
                return;
            }
            staminaStat.CurValue = 0.25f;
        }

        protected override void OnTick()
        {
            float damage = 1 * Intensity;
            CustomReasonDamageHandler customReason = new CustomReasonDamageHandler(Plugin.GetConfig().Scp079Config.SuffocationDeathMessage, damage);
            Hub.playerStats.DealDamage(customReason);
            StaminaStat staminaStat = Hub.playerStats.StatModules[2] as StaminaStat;
            if (staminaStat == null)
            {
                return;
            }
        }
    }
}
