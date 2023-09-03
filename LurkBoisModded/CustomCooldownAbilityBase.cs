using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded
{
    public abstract class CustomCooldownAbilityBase : CustomAbilityBase
    {
        public override abstract AbilityType AbilityType { get; }

        public abstract float Cooldown { get; }

        public Stopwatch Stopwatch { get; set; }

        public float RemainingTime 
        {
            get
            {
                if(Stopwatch == null || !Stopwatch.IsRunning)
                {
                    return 0f;
                }
                return (float)(Cooldown - Stopwatch.Elapsed.TotalSeconds);
            }
        }

        public override void OnFinishSetup()
        {
            base.OnFinishSetup();
            Stopwatch = new Stopwatch();
        }

        public override void OnTrigger()
        {
            base.OnTrigger();
            if (!CheckCooldown())
            {
                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.CooldownMessage.Replace("{time}", RemainingTime.ToString()));
            }
            else
            {
                Stopwatch.Restart();
            }
        }

        public bool CheckCooldown()
        {
            if(Stopwatch == null)
            {
                return true;
            }
            if (!Stopwatch.IsRunning)
            {
                Stopwatch.Start();
                return true;
            }
            if(Stopwatch.Elapsed.TotalMilliseconds > (Cooldown * 1000))
            {
                return true;
            }
            return false;
        }
    }
}
