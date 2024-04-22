using AdminToys;
using CustomPlayerEffects;
using System;
using UnityEngine;
using Mirror;
using PlayerStatsSystem;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.Effects
{
    public class OnFire : TickingEffectBase
    {
        public LightSourceToy CurrentBase;

        protected override void Enabled()
        {
            base.Enabled();
            if(!Utility.TryGetAdminToyByName("LightSource", out AdminToyBase abase))
            {
                Debug.LogError("Failed to find LightSource");
                return;
            }
            else
            {
                CurrentBase = Instantiate(abase as LightSourceToy);
                CurrentBase.NetworkLightColor = new Color(Plugin.GetConfig().FireConfig.FireColor[0], Plugin.GetConfig().FireConfig.FireColor[1], Plugin.GetConfig().FireConfig.FireColor[2]);
                CurrentBase.NetworkLightRange = Plugin.GetConfig().FireConfig.ColorRange;
                CurrentBase.NetworkLightIntensity = Plugin.GetConfig().FireConfig.ColorIntensity;
                CurrentBase.OnSpawned(Hub, new ArraySegment<string>());
            }
            Hub.playerEffectsController.ChangeState<Burned>(Intensity, Duration);
            Hub.SendHint(Plugin.GetConfig().FireConfig.FireTip);
        }

        protected override void Update()
        {
            base.Update();
            if(CurrentBase == null)
            {
                return;
            }
            CurrentBase.NetworkPosition = Hub.transform.position;
            CurrentBase.transform.position = Hub.transform.position;
        }

        protected override void Disabled()
        {
            base.Disabled();
            if (CurrentBase == null)
            {
                return;
            }
            NetworkServer.Destroy(CurrentBase.gameObject);
        }

        protected override void OnTick()
        {
            float multiplier = 1f;
            if (Config.CurrentConfig.FireConfig.DamageMultipliers.ContainsKey(Hub.roleManager.CurrentRole.RoleTypeId))
            {
                multiplier = Config.CurrentConfig.FireConfig.DamageMultipliers[Hub.roleManager.CurrentRole.RoleTypeId];
            }
            float currentDamage = (Plugin.GetConfig().FireConfig.Damage * Intensity) * multiplier;
            CustomReasonDamageHandler handler = new CustomReasonDamageHandler(Plugin.GetConfig().FireConfig.DeathReason, currentDamage, customCassieAnnouncement: "SUCCESSFULLY TERMINATED . TERMINATION CAUSE UNSPECIFIED");
            Hub.playerStats.DealDamage(handler);
        }
    }
}
