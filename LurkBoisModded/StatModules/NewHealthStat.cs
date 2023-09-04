using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using UnityEngine;
using PluginAPI.Core;
using Mirror;

namespace LurkBoisModded.StatModules
{
    public class NewHealthStat : HealthStat, IUpdatingStat
    {
        public override float CurValue { get => base.CurValue; set => base.CurValue = value; }

        public override float MaxValue => SetMaxValue;

        public float SetMaxValue 
        { 
            get 
            {
                if (Hub.roleManager.CurrentRole is IHealthbarRole)
                {
                    IHealthbarRole role = Hub.roleManager.CurrentRole as IHealthbarRole;
                    if (role.MaxHealth != MaxHealthField && MaxHealthField != -1f)
                    {
                        return MaxHealthField;
                    }
                    return role.MaxHealth;
                }
                else
                {
                    return 0f;
                }
            } 
            set { MaxHealthField = value; } 
        }

        private float MaxHealthField = -1f;

        public override float MinValue => SetMinValue;

        public float SetMinValue = 0f;

        public new void ServerHeal(float healAmount)
        {
            CurValue = Mathf.Min(CurValue + Mathf.Abs(healAmount), MaxValue);
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }

        public void ClassChanged()
        {
            
        }
    }
}
