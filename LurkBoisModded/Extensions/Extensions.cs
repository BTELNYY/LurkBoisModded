using CentralAuth;
using HarmonyLib;
using Hints;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Disarming;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Pickups;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.Base.Ability;
using LurkBoisModded.Base;
using LurkBoisModded.EventHandlers.Item;
using LurkBoisModded.Managers;
using LurkBoisModded.StatModules;
using MapGeneration;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Spectating;
using PlayerRoles.Voice;
using PlayerStatsSystem;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;
using InventorySystem.Items.Firearms.Modules;
using static UnityStandardAssets.Utility.TimedObjectActivator;
using Iced.Intel;
using static PlayerList;
using System.Collections;

namespace LurkBoisModded.Extensions
{
    public static class Extensions
    {
        public static bool RoundEnded(this RoundSummary roundSummary)
        {
            FieldInfo field = AccessTools.Field(typeof(RoundSummary), "_roundEnded");
            if(RoundSummary.singleton == null || roundSummary == null)
            {
                return false;
            }
            return (bool)field.GetValue(roundSummary);
        }

        public static T GetRandomItem<T>(this IEnumerable<T> items) 
        {
            if(items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            if(items.IsEmpty())
            {
                throw new InvalidOperationException("Can't get a random item from an empty list.");
            }
            System.Random random = new System.Random();
            int index = random.Next(items.Count());
            return items.ElementAt(index);
        }

        public static T SetFlag<T>(this T flags, T flag, bool value) where T : Enum
        {
            if(typeof(T).GetCustomAttribute(typeof(FlagsAttribute), false) == null)
            {
                return default(T);
            }
            int flagsInt = Convert.ToInt32(flags);
            int flagInt = Convert.ToInt32(flag);
            if (value)
            {
                flagsInt |= flagInt;
            }
            else
            {
                flagsInt &= ~flagInt;
            }
            return (T)Enum.ToObject(flags.GetType(), flagsInt);
        }

        public static T SetAll<T>(this T value, bool state) where T : Enum
        {
            if (typeof(T).GetCustomAttribute(typeof(FlagsAttribute), false) == null)
            {
                return default(T);
            }
            Type type = value.GetType();
            object result = value;
            string[] names = Enum.GetNames(type);
            foreach (var name in names)
            {
                T flag = (T)Enum.Parse(value.GetType(), name);
                value.SetFlag(flag, state);
            }
            return (T)result;
        }

        public static void SendProximityMessage(this VoiceMessage msg)
        {
            foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
            {
                if(referenceHub.authManager.InstanceMode != ClientInstanceMode.ReadyClient)
                {
                    continue;
                }

                if (referenceHub.roleManager.CurrentRole is SpectatorRole && !msg.Speaker.IsSpectatedBy(referenceHub))
                {
                    continue;
                }

                if (!(referenceHub.roleManager.CurrentRole is IVoiceRole voiceRole2))
                {
                    continue;
                }

                if (Vector3.Distance(msg.Speaker.transform.position, referenceHub.transform.position) >= Plugin.GetConfig().ProximityChatConfig.ProximityChatDistance)
                {
                    continue;
                }

                if (voiceRole2.VoiceModule.ValidateReceive(msg.Speaker, VoiceChatChannel.Proximity) == VoiceChatChannel.None)
                {
                    continue;
                }
                msg.Channel = VoiceChatChannel.Proximity;
                msg.SendToSpectatorsOf(referenceHub);
                referenceHub.connectionToClient.Send(msg);
            }
            msg.SendToSpectatorsOf(msg.Speaker);
        }
        
        public static List<ReferenceHub> GetPlayersInRoom(this RoomIdentifier room)
        {
            List<ReferenceHub> hubs = ReferenceHub.AllHubs.Where(x => RoomIdUtils.RoomAtPosition(x.transform.position) == room).ToList();
            return hubs;
        }

        public static Color ConvertToColor(this float[] array)
        {
            if(array.Count() == 3)
            {
                return new Color(array[0], array[1], array[2]);
            }
            if(array.Count() == 4)
            {
                return new Color(array[0], array[1], array[2], array[3]);
            }
            return Color.white;
        }
    }

    public enum DoorState
    {
        Open,
        Close,
    }
}
