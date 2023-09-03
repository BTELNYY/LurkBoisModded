using HarmonyLib;
using LurkBoisModded.EventHandlers;
using PlayerRoles.Ragdolls;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
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
            Log.Info("Registering events...");
            PluginAPI.Events.EventManager.RegisterAllEvents(this);
            RagdollManager.OnRagdollSpawned += RagdollHandler.OnRagdollSpawn;
            RagdollManager.OnRagdollSpawned += RagdollHandler.PocketRagdollHandle;
            Log.Info("Patching...");
            harmony = new Harmony("com.thelurkbois.modded");
            harmony.PatchAll();
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
