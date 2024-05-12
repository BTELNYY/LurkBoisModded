using Mirror;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerStatsSystem;
using UnityEngine;

namespace LurkBoisModded.StatModules
{
    public class NewHumeShieldStat : HumeShieldStat, IUpdatingStat
    {
        public override float CurValue { get => base.CurValue; set => base.CurValue = value; }

        public override float MinValue => SetMinValue;

        public float SetMinValue = 0f;

        public override float MaxValue => NewMaxValue;

        public float NewMaxValue
        {
            get 
            {
                HumeShieldModuleBase humeShieldModuleBase;
                if (!TryGetHsModule(out humeShieldModuleBase))
                {
                    if (SetMaxValueField != -1f)
                    {
                        return SetMaxValueField;
                    }
                    return 0f;
                }
                if(SetMaxValueField != -1f)
                {
                    return SetMaxValueField;
                }
                return humeShieldModuleBase.HsMax;
            } 
            set 
            {
                SetMaxValueField = value;
            }
        }

        private float SetMaxValueField = -1f;

        public void Start()
        {
            if (!TryGetHsModule(out var controller))
            {
                NewMaxValue = 0f;
                return;
            }
            NewMaxValue = controller.HsMax;
        }

        public void Update()
        {
            if (!NetworkServer.active || !TryGetHsModule(out var controller) || controller.HsRegeneration == 0f)
            {
                return;
            }
            float hsCurrent = controller.HsCurrent;
            float num = controller.HsRegeneration * Time.deltaTime;
            if (num > 0f)
            {
                if (!(hsCurrent >= controller.HsMax))
                {
                    CurValue = Mathf.MoveTowards(hsCurrent, controller.HsMax, num);
                }
            }
        }


        private bool TryGetHsModule(out HumeShieldModuleBase controller)
        {
            if(Hub == null)
            {
                Debug.LogWarning("Hub is null!");
                controller = null;
                return false;
            }
            IHumeShieldedRole humeShieldedRole = Hub.roleManager.CurrentRole as IHumeShieldedRole;
            if (humeShieldedRole != null)
            {
                controller = humeShieldedRole.HumeShieldModule;
                return true;
            }
            controller = null;
            return false;
        }

        public void ClassChanged()
        { 
            
        }
    }
}