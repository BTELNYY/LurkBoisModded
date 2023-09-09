using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using PlayerRoles.PlayableScps.Scp939;
using LurkBoisModded.Effects;

namespace LurkBoisModded.EventHandlers.Scp939
{
    public class Scp939ClawHandler 
    {
        [PluginEvent(ServerEventType.PlayerDamage)]
        public void OnScp939Claw(PlayerDamageEvent ev)
        {
            if(ev.DamageHandler is Scp939DamageHandler handler)
            {
                if(handler.Scp939DamageType == Scp939DamageType.Claw)
                {
                    ev.Target.EffectsManager.ChangeState<BetterBleeding>(Plugin.GetConfig().Scp939Config.Intensity, Plugin.GetConfig().Scp939Config.Duration, Plugin.GetConfig().Scp939Config.Stacks);
                }
            }
        }
    }
}
