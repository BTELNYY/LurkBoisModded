using System;
using System.Diagnostics;
using LurkBoisModded.Managers;
using PluginAPI.Core;
using LurkBoisModded.Extensions;
using UnityEngine;

namespace LurkBoisModded.Base.Ability
{
    public abstract class CustomCooldownAbility : CustomAbility
    {
        public override abstract AbilityType AbilityType { get; }

        public abstract float Cooldown { get; }

        public float EffectiveCooldown 
        { 
            get 
            {
                try
                {
                    return Math.Max(0f, Cooldown * CooldownMultiplier);
                }
                catch(Exception ex)
                {
                    Log.Error(ex.ToString());
                    return 0f;
                }
            } 
        }

        public Stopwatch CooldownStopwatch 
        {
            get
            {
                if(_stopwatch == null)
                {
                    _stopwatch = new Stopwatch();
                }
                return _stopwatch;
            }
        }

        private Stopwatch _stopwatch;

        public bool CooldownReady { get; set; } = true;

        public float RemainingCooldownTime 
        {
            get
            {
                if(!CooldownStopwatch.IsRunning)
                {
                    return 0f;
                }
                return Math.Max(0f, (float)(EffectiveCooldown - CooldownStopwatch.Elapsed.Seconds));
            }
            set
            {
                float newTime = EffectiveCooldown - value;
                newTime = Math.Max(newTime, 0);
                TimeSpan span = TimeSpan.FromSeconds(newTime);
                CooldownStopwatch.Reset();
                CooldownStopwatch.Elapsed.Add(span);
                CooldownStopwatch.Start();
            }
        }

        public float CooldownMultiplier 
        { 
            get 
            {
                return _cooldownMultiplier;
            }
            set 
            {
                float usedValue = Math.Max(0f, value);
                _cooldownMultiplier = usedValue;
            }
        }

        private float _cooldownMultiplier = 1f;

        public const float CooldownMultiplierDefault = 1f;

        public void ResetCooldownMultiplier()
        {
            CooldownMultiplier = CooldownMultiplierDefault;
        }

        public void SetCooldownMultiplierInverse(byte amount)
        {
            CooldownMultiplier /= amount;
        }

        public override void OnFinishSetup()
        {
            base.OnFinishSetup();
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            CooldownReady = CheckCooldown();
            if (!CooldownReady)
            { 
                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.CooldownMessage.Replace("{time}", ((int)RemainingCooldownTime).ToString()));
                return;
            }
            else
            {
                CooldownStopwatch.Restart();
            }
        }

        public void ResetCooldown()
        {
            CooldownStopwatch.Reset();
        }

        public void SetRemainingCooldown(float time)
        {
            float newTime = EffectiveCooldown - time;
            newTime = Math.Max(newTime, 0);
            TimeSpan span = TimeSpan.FromSeconds(newTime);
            CooldownStopwatch.Reset();
            CooldownStopwatch.Elapsed.Add(span);
            CooldownStopwatch.Start();
        }


        public bool CheckCooldown()
        {
            if (!CooldownStopwatch.IsRunning)
            {
                CooldownStopwatch.Start();
                return true;
            }
            if(CooldownStopwatch.Elapsed.TotalSeconds > (EffectiveCooldown))
            {
                return true;
            }
            return false;
        }
    }
}
