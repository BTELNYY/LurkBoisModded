using Hints;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using MapGeneration;
using Mirror;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Spectating;
using PlayerRoles.Voice;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;

namespace LurkBoisModded
{
    public static class Extensions
    {
        private static MethodInfo sendSpawnMessage;

        public static MethodInfo SendSpawnMessage => sendSpawnMessage = typeof(NetworkServer).GetMethod("SendSpawnMessage", BindingFlags.NonPublic | BindingFlags.Static);

        public static void SetScale(this Player target, float scale)
        {
            try
            {
                target.ReferenceHub.transform.localScale = new Vector3(scale, scale, scale);
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                    SendSpawnMessage?.Invoke(null, new object[] { target.ReferenceHub.networkIdentity, hub.connectionToClient });
            }
            catch (Exception exception)
            {
                Log.Error($"Error: {exception}");
            }
        }
        public static void SetScale(this NetworkIdentity netId, float scale)
        {
            try
            {
                netId.gameObject.transform.localScale = new Vector3(scale, scale, scale);
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    SendSpawnMessage?.Invoke(null, new object[] { netId, hub.connectionToClient });
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        public static void SetScale(this NetworkIdentity netId, Vector3 scale)
        {
            try
            {
                netId.gameObject.transform.localScale = scale;
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    SendSpawnMessage?.Invoke(null, new object[] { netId, hub.connectionToClient });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        public static void Respawn(this NetworkIdentity netId)
        {
            try
            {
                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    SendSpawnMessage?.Invoke(null, new object[] { netId, hub.connectionToClient });
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
        public static void SetScale(this Player target, Vector3 scale)
        {
            try
            {
                target.ReferenceHub.transform.localScale = scale;

                foreach (ReferenceHub hub in ReferenceHub.AllHubs)
                {
                    SendSpawnMessage?.Invoke(null, new object[] { target.ReferenceHub.networkIdentity, hub.connectionToClient });
                }
            }
            catch (Exception exception)
            {
                Log.Error($"Error: {exception}");
            }
        }
        public static bool InElevator(this Player target)
        {
            List<ElevatorDoor> AllElevs = new List<ElevatorDoor>();
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczB01, out var list))
            {
                AllElevs.AddRange(list);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczA01, out var list1))
            {
                AllElevs.AddRange(list1);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczA02, out var list2))
            {
                AllElevs.AddRange(list2);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.LczB02, out var list3))
            {
                AllElevs.AddRange(list3);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.Scp049, out var list4))
            {
                AllElevs.AddRange(list4);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.Nuke, out var list5))
            {
                AllElevs.AddRange(list5);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.GateA, out var list6))
            {
                AllElevs.AddRange(list6);
            }
            if (ElevatorDoor.AllElevatorDoors.TryGetValue(ElevatorManager.ElevatorGroup.GateB, out var list7))
            {
                AllElevs.AddRange(list7);
            }
            foreach (var elevdoors in AllElevs)
            {
                ElevatorChamber chamber = elevdoors.TargetPanel.AssignedChamber;
                if (chamber.WorldspaceBounds.Contains(target.Position))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Returns the first found itmebase of the itemtype in a users inventory.
        /// </summary>
        public static ItemBase GetItemById(this Inventory inv, ItemType type)
        {
            return inv.UserInventory.Items.First(x => x.Value.ItemTypeId == type).Value;
        }
        /// <summary>
        /// Returns the first found instance of a item from a players inventory according to type
        /// </summary>
        public static T GetItemByType<T>(this Inventory inv, T type) where T : ItemBase
        {
            return inv.UserInventory.Items.FirstOrDefault(x => (x as T) != null).Value as T;
        }
        public static Firearm GetFirearm(this Inventory inv, ItemType type = ItemType.None)
        {
            foreach(ItemBase item in inv.UserInventory.Items.Values) 
            {
                if(item.ItemTypeId != type && type != ItemType.None)
                {
                    continue;
                }
                if((item as Firearm) == null) 
                {
                    continue;
                }
                return item as Firearm;
            }
            return null;
        }
        public static T SetFlag<T>(this T flags, T flag, bool value) where T : Enum
        {
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
        public static void SetDoorState(this DoorVariant door, DoorState state)
        {
            if(state == DoorState.Open)
            {
                door.NetworkTargetState = true;
            }
            else
            {
                door.NetworkTargetState = false;
            }
        }
        public static void SetDoorState(this DoorVariant door, DoorState state, bool locked, DoorLockReason reason = DoorLockReason.AdminCommand)
        {
            if (state == DoorState.Open)
            {
                door.TargetState = true;
            }
            else
            {
                door.TargetState = false;
            }
            door.ServerChangeLock(reason, locked);
        }
        public static void SendHint(this Player target, string text, float duration = 3f)
        {
            target.ReferenceHub.hints.Show(new TextHint(text, new HintParameter[] { new StringHintParameter(string.Empty) }, null, duration));
        }
        public static void SendHint(this ReferenceHub target, string text, float duration = 3f)
        {
            target.hints.Show(new TextHint(text, new HintParameter[] { new StringHintParameter(string.Empty) }, null, duration));
        }
        public static void SendProximityMessage(this VoiceMessage msg)
        {
            foreach (ReferenceHub referenceHub in ReferenceHub.AllHubs)
            {
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
                msg.SendToSpectatorsOf(msg.Speaker);
                msg.SendToSpectatorsOf(referenceHub);
                referenceHub.connectionToClient.Send(msg);
            }
        }
        public static void AddAbility(this ReferenceHub target, AbilityType type)
        {
            if (!AbilityManager.AbilityToType.ContainsKey(type))
            {
                Debug.LogError("Failed to find ability by AbilityType!");
                return;
            }
            CustomAbilityBase ability = (CustomAbilityBase)target.gameObject.AddComponent(AbilityManager.AbilityToType[type]);
            ability.CurrentHub = target;
            ability.OnFinishSetup();
        }
        public static void SetSubclass(this ReferenceHub target, SubclassBase subclass)
        {
            Player player = Player.Get(target);
            player.SetRole(subclass.Role);
            if (subclass.HeightVariety[0] == subclass.HeightVariety[1])
            {
                Vector3 nonRandomHeight = new Vector3(1, subclass.HeightVariety[0], 1);
                player.SetScale(nonRandomHeight);
            }
            else
            {
                float height = UnityEngine.Random.Range(subclass.HeightVariety[0], subclass.HeightVariety[1]);
                Vector3 heightVec = new Vector3(1, height, 1);
                player.SetScale(heightVec);
            }
            foreach(AbilityType ability in subclass.Abilities)
            {
                if (AbilityManager.AbilityToType.ContainsKey(ability))
                {
                    target.AddAbility(ability);
                }
                else
                {
                    Log.Warning("Ability is missing! Ability: " + ability.ToString(), "Subclass");
                }
            }
            if (subclass.ClearInventoryOnSpawn)
            {
                player.ClearInventory();
            }
            foreach(ItemType item in subclass.SpawnItems.Values)
            {
                if (item.ToString().Contains("Ammo"))
                {
                    player.AddAmmo(item, subclass.SpawnItems[item]);
                }
                else
                {
                    for (int i = 0; i < subclass.SpawnItems[item]; i++)
                    {
                        player.AddItem(item);
                    }
                }
            }
            string hintFormatted = $"You are <b>{subclass.SubclassNiceName}</b>! \n ${subclass.SubclassDescription}";
            target.SendHint(hintFormatted, 10f);
            //if the spawnrooms is more than one, otherwise just use the default spawn
            if(subclass.SpawnRooms.Count > 0)
            {
                List<RoomIdentifier> foundRooms = RoomIdentifier.AllRoomIdentifiers.Where(x => subclass.SpawnRooms.Contains(x.Name)).ToList();
                RoomIdentifier chosenRoom = foundRooms.RandomItem();
                //Make sure that the door can't be a keycard door
                DoorVariant door = DoorVariant.DoorsByRoom[chosenRoom].Where(x => x.RequiredPermissions.RequiredPermissions == KeycardPermissions.None).ToList().RandomItem();
                Vector3 pos = door.transform.position;
                pos.y += 1f;
                target.TryOverridePosition(pos, Vector3.forward);
            }
        }
    }

    public enum DoorState
    {
        Open,
        Close,
    }
}
