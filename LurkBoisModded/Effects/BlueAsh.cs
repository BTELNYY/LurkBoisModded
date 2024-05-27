using CentralAuth;
using CustomPlayerEffects;
using PluginAPI.Core;
using LurkBoisModded.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class BlueAsh : TickingEffectBase
    {
        public override void Enabled()
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

        public override void IntensityChanged(byte prevState, byte newState)
        {
            base.IntensityChanged(prevState, newState);
            Player p = Player.Get(Hub);
            p.EffectsManager.ChangeState<MovementBoost>(newState, TimeLeft, false);
        }

        public override void Disabled()
        {
            base.Disabled();
            if(TimeLeft <= 1f && Hub.authManager.InstanceMode == ClientInstanceMode.ReadyClient)
            {
                Player p = Player.Get(Hub);
                p.Kill(Plugin.GetConfig().Scp914Config.Scp914BlueAshDeathReason);
            }
        }

        public override void OnTick()
        {
            
        }
    }
}
