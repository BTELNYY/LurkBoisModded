using PluginAPI.Core;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Gamemodes
{
    public class GamemodeManager
    {

        private static Gamemode _currentGamemode;

        public static Gamemode ActiveGamemode
        {
            get
            {
                if(_currentGamemode == null)
                {
                    Log.Warning("Tried to get active gamemode when none was active.");
                }
                return _currentGamemode;
            }
        }

        public static bool RunningGamemode
        {
            get
            {
                return _currentGamemode != null;
            }
        }

        private static Gamemode _nextRestartGameMode;

        public static bool NextRestartGamemodeExists
        {
            get
            {
                return _nextRestartGameMode != null;
            }
        }

        public static void TriggerNextRoundGamemode()
        {
            if(!NextRestartGamemodeExists || RunningGamemode)
            {
                return;
            }
            else
            {
                TriggerGamemode(_nextRestartGameMode);
            }
        }

        public static void StopGamemode()
        {
            if (registeredHandlers.Count > 0)
            {
                foreach (object obj in registeredHandlers)
                {
                    PluginAPI.Events.EventManager.UnregisterEvents(Plugin.instance, obj);
                }
                registeredHandlers.Clear();
            }
            if (_currentGamemode != null)
            {
                if (_currentGamemode.RegisterSelfAsEventHandler)
                {
                    PluginAPI.Events.EventManager.UnregisterEvents(Plugin.instance, _currentGamemode);
                }
                _currentGamemode.OnTerminate();
                _currentGamemode = null;
            }
        }

        public static void TriggerGamemode(string name)
        {
            List<Type> allGamemodes = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(Gamemode)) && x.GetCustomAttribute(typeof(GamemodeAttribute)) != null).ToList();
            foreach(Type type in allGamemodes)
            {
                Gamemode gamemode = Activator.CreateInstance(type) as Gamemode;
                if(gamemode != null)
                {
                    if (gamemode.Name == name)
                    {
                        TriggerGamemode(gamemode);
                        return;
                    }
                }
            }
            Log.Error("Cant find gamemode by name! Gamemode: " +  name);
        }

        private static List<object> registeredHandlers = new List<object>();

        public static void TriggerGamemode(Gamemode gamemode)
        {
            if(gamemode.RequireRoundRestart && !NextRestartGamemodeExists)
            {
                _nextRestartGameMode = gamemode;
                RoundSummary.singleton.ForceEnd();
                return;
            }
            StopGamemode();
            _currentGamemode = gamemode;
            foreach(Type t in gamemode.EventHandlers)
            {
                object obj = Activator.CreateInstance(t);
                if(obj == null)
                {
                    Log.Warning($"Can't create event handler type for gamemode! Name: {gamemode.Name}, type: {t.Name}");
                    continue;
                }
                registeredHandlers.Add(obj);
                PluginAPI.Events.EventManager.RegisterEvents(Plugin.instance, obj);
            }
            if (gamemode.RegisterSelfAsEventHandler)
            {
                PluginAPI.Events.EventManager.RegisterEvents(Plugin.instance, _currentGamemode);
            }
            if(_nextRestartGameMode != null)
            {
                _nextRestartGameMode = null;
            }
        }
    }
}
