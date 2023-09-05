using CustomPlayerEffects;
using PlayerRoles;
using PlayerStatsSystem;

namespace LurkBoisModded.Effects
{
    public class MaxHealthReduction : StatusEffectBase
    {
        private byte SavedIntensity = 0;

        protected override void Enabled()
        {
            base.Enabled();
            float newMaxHealth = Hub.playerStats.StatModules[0].MaxValue - Intensity;
            SavedIntensity = Intensity;
            Hub.SetMaxHealth(newMaxHealth);
            (Hub.playerStats.StatModules[0] as HealthStat).ServerHeal(newMaxHealth);
        }

        protected override void Disabled()
        {
            base.Disabled();
            float newMaxHealth = Hub.playerStats.StatModules[0].MaxValue + SavedIntensity;
            if (Hub.roleManager.CurrentRole is IHealthbarRole)
            {
                IHealthbarRole role = Hub.roleManager.CurrentRole as IHealthbarRole;
                if (role.MaxHealth < newMaxHealth)
                {
                    newMaxHealth = role.MaxHealth;
                }
            }
            Hub.SetMaxHealth(newMaxHealth);
            (Hub.playerStats.StatModules[0] as HealthStat).ServerHeal(SavedIntensity);
            Hub.SendHint("Your Max Health has been restored!");
        }
    }
}
