using CentralAuth;
using CustomPlayerEffects;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class BlueAsh : TickingEffectBase
    {
        protected override void Enabled()
        {
            base.Enabled();
            if (Hub.authManager.InstanceMode != ClientInstanceMode.ReadyClient)
            {
                return;
            }
            Player p = Player.Get(Hub);
            p.EffectsManager.ChangeState<MovementBoost>(Intensity, TimeLeft, false);
            p.SendHint(Plugin.GetConfig().Scp914Config.Scp914BlueAshReminder.Replace("{time}", ((int)TimeLeft).ToString()));
        }

        protected override void IntensityChanged(byte prevState, byte newState)
        {
            base.IntensityChanged(prevState, newState);
            Player p = Player.Get(Hub);
            p.EffectsManager.ChangeState<MovementBoost>(newState, TimeLeft, false);
        }

        protected override void Disabled()
        {
            base.Disabled();
            if(TimeLeft <= 1f && Hub.authManager.InstanceMode == ClientInstanceMode.ReadyClient)
            {
                Player p = Player.Get(Hub);
                p.Kill(Plugin.GetConfig().Scp914Config.Scp914BlueAshDeathReason);
            }
        }

        protected override void OnTick()
        {
            
        }
    }
}
