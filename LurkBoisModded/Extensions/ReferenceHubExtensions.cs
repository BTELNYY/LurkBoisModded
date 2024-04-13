using InventorySystem.Items.Firearms;
using InventorySystem.Items;
using InventorySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CentralAuth;
using Hints;
using Interactables.Interobjects.DoorUtils;
using Interactables.Interobjects;
using LurkBoisModded.Base.Ability;
using LurkBoisModded.Base;
using LurkBoisModded.Managers;
using LurkBoisModded.StatModules;
using MapGeneration;
using PlayerRoles.FirstPersonControl;
using PlayerStatsSystem;
using PluginAPI.Core;
using UnityEngine;
using InventorySystem.Disarming;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.EventHandlers.Item;

namespace LurkBoisModded.Extensions
{
    public static class ReferenceHubExtensions
    {
        /// <summary>
        /// Returns the first found itmebase of the itemtype in a users inventory.
        /// </summary>
        public static ItemBase GetItemById(this ReferenceHub hub, ItemType type)
        {
            return hub.inventory.UserInventory.Items.First(x => x.Value.ItemTypeId == type).Value;
        }

        public static ItemBase GetItemBySerial(this ReferenceHub hub, ushort serial)
        {
            return hub.inventory.UserInventory.Items.First(x => x.Value.ItemSerial == serial).Value;
        }

        /// <summary>
        /// Returns the first found instance of a item from a players inventory according to type
        /// </summary>
        public static T GetItemByType<T>(this ReferenceHub hub, T type) where T : ItemBase
        {
            return hub.inventory.UserInventory.Items.FirstOrDefault(x => (x as T) != null).Value as T;
        }

        public static bool RemoveItemFromHub(this ReferenceHub target, ItemType type)
        {
            foreach (var item in target.inventory.UserInventory.Items.Keys)
            {
                if (target.inventory.UserInventory.Items[item].ItemTypeId == type)
                {
                    target.inventory.ServerRemoveItem(item, target.inventory.UserInventory.Items[item].PickupDropModel);
                    return true;
                }
            }
            return false;
        }

        public static Firearm GetFirearm(this ReferenceHub hub, ItemType type = ItemType.None)
        {
            foreach (ItemBase item in hub.inventory.UserInventory.Items.Values)
            {
                if (item.ItemTypeId != type && type != ItemType.None)
                {
                    continue;
                }
                if ((item as Firearm) == null)
                {
                    continue;
                }
                return item as Firearm;
            }
            return null;
        }

        public static void SendHint(this ReferenceHub target, string text, float duration = 3f)
        {
            if (target == null || target.hints == null || target.authManager.InstanceMode != ClientInstanceMode.ReadyClient)
            {
                return;
            }
            target.hints.Show(new TextHint(text, new HintParameter[] { new StringHintParameter(string.Empty) }, null, duration));
        }

        public static void AddAbility(this ReferenceHub target, AbilityType type)
        {
            if (target.authManager.InstanceMode != ClientInstanceMode.ReadyClient || target.nicknameSync.MyNick == "Dedicated Server")
            {
                Log.Error("Tried adding ability to Dedicated Server or Unready Client!");
                return;
            }
            if (!AbilityManager.AbilityToType.ContainsKey(type))
            {
                Log.Error("Failed to find ability by AbilityType!");
                return;
            }
            CustomAbility ability = (CustomAbility)target.gameObject.AddComponent(AbilityManager.AbilityToType[type]);
            ability.CurrentHub = target;
            ability.OnFinishSetup();
        }
        public static void SetSubclass(this ReferenceHub target, Subclass subclass)
        {
            if (target.authManager.InstanceMode != ClientInstanceMode.ReadyClient || target.nicknameSync.MyNick == "Dedicated Server")
            {
                Log.Error("Tried adding ability to Dedicated Server or Unready Client!");
                return;
            }
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
            foreach (AbilityType ability in subclass.Abilities)
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
            if (subclass.MaxHealth != 0)
            {
                target.SetMaxHealth(subclass.MaxHealth);
                player.Heal(subclass.MaxHealth);
            }
            if (subclass.ClearInventoryOnSpawn)
            {
                player.ClearInventory();
            }
            foreach (ItemType item in subclass.SpawnItems.Keys)
            {
                if (item.ToString().Contains("Ammo"))
                {
                    player.AddAmmo(item, subclass.SpawnItems[item]);
                    continue;
                }
                if (item.ToString().Contains("Gun"))
                {
                    //ItemBase itemBase = player.AddItem(item);
                    //InventoryItemLoader.TryGetItem(item, out itemBase);
                    player.AddFirearm(item, (byte)subclass.SpawnItems[item]);
                    //if (itemBase is Firearm firearm && subclass.SpawnItems[item] > 0)
                    //{
                    //    Timing.CallDelayed(0.25f, () => 
                    //    {
                    //        byte ammo = (byte)subclass.SpawnItems[item];
                    //        firearm.Status = new FirearmStatus(ammo, firearm.Status.Flags, firearm.Status.Attachments);
                    //    });
                    //}
                    continue;
                }
                for (int i = 0; i < subclass.SpawnItems[item]; i++)
                {
                    player.AddItem(item);
                }
            }
            for (int i = 0; i < subclass.NumberOfRandomItems; i++)
            {
                ItemType item = subclass.RandomItems.Keys.ToList().RandomItem();
                if (item.ToString().Contains("Ammo"))
                {
                    player.AddAmmo(item, subclass.RandomItems[item]);
                    continue;
                }
                if (item.ToString().Contains("Gun"))
                {
                    player.AddFirearm(item, (byte)subclass.RandomItems[item]);
                    //ItemBase itemBase = player.AddItem(item);
                    //if (itemBase is Firearm firearm && subclass.RandomItems[item] > 0)
                    //{
                    //    Timing.CallDelayed(0.25f, () => 
                    //    {
                    //        byte ammo = (byte)subclass.RandomItems[item];
                    //        firearm.Status = new FirearmStatus(ammo, firearm.Status.Flags, firearm.Status.Attachments);
                    //    });
                    //}
                    continue;
                }
                for (int f = 0; f < subclass.RandomItems[item]; f++)
                {
                    player.AddItem(item);
                }
            }
            player.ApplyAttachments();
            //if the spawnrooms is more than one, otherwise just use the default spawn
            if (subclass.SpawnRooms.Count > 0 || subclass.NonNamedRoomSpawns.Count > 0)
            {
                List<RoomIdentifier> foundRooms = RoomIdentifier.AllRoomIdentifiers.Where(x => subclass.SpawnRooms.Contains(x.Name) && !SubclassManager.TempDisallowedRooms.Contains(x.Name)).ToList();
                foreach (NonNamedRoomDefinition nonNamedRoomDefinition in subclass.NonNamedRoomSpawns)
                {
                    List<RoomIdentifier> rooms = RoomIdUtils.FindRooms(RoomName.Unnamed, nonNamedRoomDefinition.FacilityZone, nonNamedRoomDefinition.RoomShape).ToList();
                    foundRooms.AddRange(rooms);
                }
                foundRooms.ShuffleList();
                if (foundRooms.Count != 0)
                {
                    RoomIdentifier chosenRoom = foundRooms.RandomItem();
                    DoorVariant door = null;
                    if (subclass.AllowKeycardDoors)
                    {
                        door = DoorVariant.DoorsByRoom[chosenRoom].Where(x => !(x is ElevatorDoor)).ToList().RandomItem();
                    }
                    else
                    {
                        door = DoorVariant.DoorsByRoom[chosenRoom].Where(x => x.RequiredPermissions.RequiredPermissions == KeycardPermissions.None && !(x is ElevatorDoor) && !(x is INonInteractableDoor)).ToList().RandomItem();
                    }
                    door.SetDoorState(DoorState.Open);
                    Vector3 pos = door.transform.position;
                    pos.y += 1f;
                    target.TryOverridePosition(pos, Vector3.forward);
                }
            }
            player.PlayerInfo.IsRoleHidden = true;
            if (subclass.ApplyClassColorToCustomInfo)
            {
                player.CustomInfo = $"<color={subclass.ClassColor}>" + subclass.SubclassNiceName + "(Custom Subclass)</color>";
            }
            else
            {
                player.CustomInfo = subclass.SubclassNiceName + " (Custom Subclass)";
            }
            string hintFormatted = $"You are <color={subclass.ClassColor}><b>{subclass.SubclassNiceName}</b></color>! \n {subclass.SubclassDescription}";
            target.SendHint(hintFormatted, 30f);
            target.gameConsoleTransmission.SendToClient(hintFormatted, "green");
        }
        public static void RemoveAllAbilities(this ReferenceHub target)
        {
            List<CustomAbility> abilities = target.gameObject.GetComponents<CustomAbility>().ToList();
            foreach (CustomAbility ability in abilities)
            {
                ability.OnRemoved();
                GameObject.Destroy(ability);
            }
        }

        public static void SetMaxHealth(this ReferenceHub hub, float amount)
        {
            if (!hub.playerStats.TryGetModule<HealthStat>(out HealthStat health))
            {
                return;
            }
            if (!(health is NewHealthStat))
            {
                return;
            }
            NewHealthStat healthStat = health as NewHealthStat;
            healthStat.SetMaxValue = amount;
        }

        public static List<ReferenceHub> GetAllDisarmedPlayersByDisarmer(this ReferenceHub disarmer)
        {
            List<DisarmedPlayers.DisarmedEntry> disarmedEntries = DisarmedPlayers.Entries.Where(x => x.Disarmer == disarmer.networkIdentity.netId).ToList();
            List<uint> netIds = disarmedEntries.Select(x => x.DisarmedPlayer).ToList();
            List<ReferenceHub> hubs = ReferenceHub.AllHubs.Where(x => netIds.Contains(x.networkIdentity.netId)).ToList();
            return hubs;
        }

        public static CustomItem AddCustomItem(this ReferenceHub target, CustomItemType type)
        {
            return CustomItemHandler.AddItem(target, type);
        }

        public static void AddItem(this ReferenceHub target, ItemType type)
        {
            target.inventory.ServerAddItem(type);
        }

        public static void AddItem(this ReferenceHub target, ItemType type, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                target.inventory.ServerAddItem(type);
            }
        }

        public static void RemoveItems(this ReferenceHub target, ItemType type, int amount)
        {
            Player p = Player.Get(target);
            p.RemoveItems(type, amount);
        }
    }
}
