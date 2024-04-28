using HarmonyLib;
using LurkBoisModded.EventHandlers;
using LurkBoisModded.EventHandlers.Item;
using LurkBoisModded.EventHandlers.Map;
using LurkBoisModded.Managers;
using PlayerRoles.Ragdolls;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LurkBoisModded
{
    public class Plugin
    {
        public const string PluginName = "LurkBoisModded";
        public const string PluginVersion = "1.0.0";
        public const string PluginDesc = "LurkBois Modded!";
        public Harmony harmony;
        public static Plugin instance;

        public static readonly List<Vector3> Directions = new List<Vector3>
        {
            Vector3.back,
            Vector3.down,
            Vector3.forward,
            Vector3.right,
            Vector3.left,
            Vector3.up
        };

        [PluginConfig(PluginName + ".yml")]
        public Config config = new Config();
        public static Config GetConfig()
        {
            return instance.config;
        }
        public EventHandler eventHandler;
        public string SubclassPath = "";
        public string ConfigPath = "";

        [PluginEntryPoint(PluginName, PluginVersion, PluginDesc, "btelnyy#8395")]
        public void LoadPlugin()
        {
            if (!config.PluginEnabled)
            {
                Log.Info("Plugin is disabled!");
                return;
            }
            instance = this;
            ConfigPath = PluginHandler.Get(this).PluginDirectoryPath;
            SubclassPath = Path.Combine($"{PluginHandler.Get(this).PluginDirectoryPath}", "subclasses");
            Log.Info("Patching...");
            harmony = new Harmony("com.thelurkbois.modded");
            harmony.PatchAll();
            Log.Info("Registering events...");
            List<Type> eventHandlers = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.GetCustomAttribute(typeof(EventHandlerAttribute)) != null).ToList();
            foreach (Type type in eventHandlers)
            {
                Log.Info("Registering Handler: " + type.Name);
                object obj = Activator.CreateInstance(type);
                if(obj == null)
                {
                    Log.Error("Unable to create instance of Handler " + type.FullName);
                    continue;
                }
                PluginAPI.Events.EventManager.RegisterEvents(this, obj);
                Log.Info("Done register of handler " + type.Name);
            }
            RagdollManager.OnRagdollSpawned += RagdollHandler.OnRagdollSpawn;
            RagdollManager.OnRagdollSpawned += RagdollHandler.PocketRagdollHandle;
            Log.Info("Running Init...");
            CustomItemHandler.Init();
            CustomItemManager.Init();
            SubclassManager.Init();
            Log.Info("LurkBoisModded v" + PluginVersion + " loaded.");
        }

        [PluginUnload()]
        public void Unload()
        {
            config = null;
            eventHandler = null;
            harmony.UnpatchAll("com.thelurkbois.modded"); // this needs to be the same as above or else we unpatch everyone's patches (other plugins) for some godforsaken reason
        }
    }
}
