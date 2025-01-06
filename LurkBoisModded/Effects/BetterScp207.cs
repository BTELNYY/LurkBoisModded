using CustomPlayerEffects;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using RemoteAdmin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;

namespace LurkBoisModded.Effects
{
    public class BetterScp207 : TickingEffectBase, IStaminaModifier, IMovementSpeedModifier, IConflictableEffect, IHealableEffect
    {
        public bool StaminaModifierActive => Intensity > 0;

        public float StaminaUsageMultiplier
        {
            get
            {
                StaminaStat stat = Hub.playerStats.GetModule<StaminaStat>();
                if(stat.CurValue <= 0.05f)
                {
                    stat.CurValue = 0.05f;
                    return 0f;
                }
                return Mathf.Clamp(1.25f * Intensity, 1f, 2f);
            }
        }

        public float StaminaRegenMultiplier
        {
            get
            {
                return 0.75f;
            }
        }

        public bool SprintingDisabled => false;

        public string DisplayName => "Better SCP 207";

        public bool CanBeDisplayed => true;

        public bool MovementModifierActive => Intensity > 0;

        public float MovementSpeedLimit => 10f;

        public float MovementSpeedMultiplier => Math.Max(1f, 1.2f * (float)Intensity);

        private byte _oldIntensity = 0;

        private float _oldDuration = 0f;

        private bool goingToExplode = false;

        private float explosiveTimer = 0f;

        protected override void Enabled()
        {
            base.Enabled();
            goingToExplode = false;
            MovementBoost effect = Hub.playerEffectsController.GetEffect<MovementBoost>();
            if (effect == null)
            {
                return;
            }
            _oldIntensity = effect.Intensity;
            _oldDuration = effect.Duration;
            effect.ServerSetState((byte)(15 * Intensity), 0, false);
        }

        protected override void Disabled()
        {
            base.Disabled();
            MovementBoost effect = Hub.playerEffectsController.GetEffect<MovementBoost>();
            if (effect == null)
            {
                return;
            }
            effect.ServerSetState(_oldIntensity, _oldDuration, false);
            goingToExplode = false;
        }

        protected override void IntensityChanged(byte prevState, byte newState)
        {
            base.IntensityChanged(prevState, newState);
            byte newPercent = (byte)(15 * Intensity);
            MovementBoost effect = Hub.playerEffectsController.GetEffect<MovementBoost>();
            if(effect == null)
            {
                return;
            }
            effect.ServerSetState(newPercent, 0, false);
        }

        protected override void OnEffectUpdate()
        {
            base.OnEffectUpdate();
            if (!NetworkServer.active)
            {
                return;
            }
            if (!goingToExplode)
            {
                return;
            }
            explosiveTimer += Time.deltaTime;
            if (explosiveTimer < 1.05f)
            {
                return;
            }
            ExplosionUtils.ServerExplode(base.Hub, ExplosionType.Custom);
            goingToExplode = false;
        }

        protected override void OnTick()
        {
            StaminaStat stamina = Hub.playerStats.GetModule<StaminaStat>();
            if(stamina.CurValue < 0.075f)
            {
                if (!NetworkServer.active)
                {
                    return;
                }
                if (!(Hub.roleManager.CurrentRole is IFpcRole fpcRole))
                {
                    return;
                }
                if (Vitality.CheckPlayer(Hub))
                {
                    return;
                }
                Hub.playerStats.DealDamage(new UniversalDamageHandler(2f * Intensity, DeathTranslations.Scp207, null));
            }
            else
            {
                Hub.playerStats.DealDamage(new UniversalDamageHandler(0.25f * Intensity, DeathTranslations.Scp207, null));
            }
        }

        public bool CheckConflicts(StatusEffectBase other)
        {
            if (other is Scp1853)
            {
                Poisoned poisoned;
                if (!base.Hub.playerEffectsController.TryGetEffect<Poisoned>(out poisoned))
                {
                    return false;
                }
                if (!poisoned.IsEnabled)
                {
                    poisoned.ForceIntensity(1);
                }
                return true;
            }
            else
            {
                CokeBase cokeBase = other as CokeBase;
                if (cokeBase == null)
                {
                    return false;
                }
                goingToExplode = true;
                cokeBase.ServerDisable();
                return true;
            }
        }

        public bool IsHealable(ItemType item)
        {
            if(item == ItemType.SCP500)
            {
                return true;
            }
            return false;
        }
    }
}
