using CustomPlayerEffects;
using LurkBoisModded.Effects;
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using Scp914;

namespace LurkBoisModded.EventHandlers.Scp914
{
    [EventHandler]
    public class Scp914SettingEventHandler
    {
        public static void HandlePlayer(Player target, Scp914Event ev)
        {
            switch(ev)
            {
                case Scp914Event.Nothing:
                    break;
                case Scp914Event.Death:
                    target.Kill(Plugin.GetConfig().Scp914Config.Scp914DeathReason);
                    break;
                case Scp914Event.Scp914DamageOverTimeEffect:
                    target.EffectsManager.ChangeState<Scp914DamageOverTime>(1, Plugin.GetConfig().Scp914Config.Scp914DamageOverTimeDuration, true);
                    break;
                case Scp914Event.Zombie:
                    target.DropEverything();
                    target.SetRole(PlayerRoles.RoleTypeId.Scp0492);
                    break;
                case Scp914Event.BlueAsh:
                    target.EffectsManager.ChangeState<BlueAsh>(Plugin.GetConfig().Scp914Config.Scp914BlueAshIntensity, Plugin.GetConfig().Scp914Config.Scp914BlueAshDuration, false);
                    break;
                case Scp914Event.Spectator:
                    target.DropEverything();
                    target.SetRole(PlayerRoles.RoleTypeId.Spectator);
                    break;
                default:
                    break;
            }
        }


        [PluginEvent(ServerEventType.Scp914ProcessPlayer)]
        public void OnProccessPlayer(Scp914ProcessPlayerEvent ev)
        {
            if (ev.Player.IsSCP)
            {
                return;
            }
            Scp914Event randomEvent = Plugin.GetConfig().Scp914Config.Scp914Settings[ev.KnobSetting].RandomItem();
            Timing.CallDelayed(0.25f, () =>
            {
                HandlePlayer(ev.Player, randomEvent);
            });
        }

        public enum Scp914Event
        {
            Nothing,
            Death,
            Scp914DamageOverTimeEffect,
            Zombie,
            TeleportAndDamage,
            BlueAsh,
            Spectator,
        }
    }
}
