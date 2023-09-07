using System.Diagnostics;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Base
{
    public abstract class CustomCooldownAbility : CustomAbility
    {
        public override abstract AbilityType AbilityType { get; }

        public abstract float Cooldown { get; }

        public Stopwatch Stopwatch { get; set; }

        public bool CooldownReady { get; set; } = true;

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
            CooldownReady = CheckCooldown();
            if (!CooldownReady)
            {

                CurrentHub.SendHint(Plugin.GetConfig().AbilityConfig.CooldownMessage.Replace("{time}", ((int)RemainingTime).ToString()));
                return;
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
