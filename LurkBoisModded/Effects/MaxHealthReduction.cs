using CustomPlayerEffects;
using PlayerRoles;
using PlayerStatsSystem;

namespace LurkBoisModded.Effects
{
    public class MaxHealthReduction : StatusEffectBase
    {
        protected override void Enabled()
        {
            base.Enabled();
            float newMaxHealth = Hub.playerStats.StatModules[0].MaxValue - Intensity;
            Hub.SetMaxHealth(newMaxHealth);
            (Hub.playerStats.StatModules[0] as HealthStat).ServerHeal(newMaxHealth);
        }

        protected override void Disabled()
        {
            base.Disabled();
            float newMaxHealth = Hub.playerStats.StatModules[0].MaxValue + Intensity;
            if (Hub.roleManager.CurrentRole is IHealthbarRole)
            {
                IHealthbarRole role = Hub.roleManager.CurrentRole as IHealthbarRole;
                if (role.MaxHealth < newMaxHealth)
                {
                    newMaxHealth = role.MaxHealth;
                }
            }
            Hub.SetMaxHealth(newMaxHealth);
            Hub.SendHint("Your Max Health has been restored!");
        }
    }
}
