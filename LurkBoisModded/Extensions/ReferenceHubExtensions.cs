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
using PlayerRoles;
using LurkBoisModded.EventHandlers.General;
using PluginAPI.Core.Items;
using MEC;
using InventorySystem.Items.MicroHID;
using Mirror.LiteNetLib4Mirror;
using InventorySystem.Items.Radio;
using PlayerRoles.FirstPersonControl.NetworkMessages;
using System.Runtime.CompilerServices;

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

        public static Dictionary<ReferenceHub, Subclass> RefHubToSubclassDict = new Dictionary<ReferenceHub, Subclass>();

        public static void UnsetSubclass(this ReferenceHub target, bool keepCurrentRole = false, bool keepCurrentPosition = false, bool keepInventory = false, RoleTypeId newRole = RoleTypeId.Spectator)
        {
            if (!RefHubToSubclassDict.ContainsKey(target))
            {
                return;
            }
            RoleTypeId setRole = newRole;
            if (keepCurrentRole)
            {
                setRole = target.roleManager.CurrentRole.RoleTypeId;
            }
            RoleSpawnFlags spawnFlags = new RoleSpawnFlags();
            spawnFlags.SetFlag(RoleSpawnFlags.UseSpawnpoint, !keepCurrentPosition);
            spawnFlags.SetFlag(RoleSpawnFlags.AssignInventory, !keepInventory);
            target.roleManager.ServerSetRole(setRole, RoleChangeReason.RemoteAdmin, spawnFlags);
            RefHubToSubclassDict.Remove(target);
        }

        public static Subclass GetSubclass(this ReferenceHub target)
        {
            if (RefHubToSubclassDict.ContainsKey(target))
            {
                return RefHubToSubclassDict[target];
            }
            return null;
        }

        public static void SetSubclass(this ReferenceHub target, Subclass subclass)
        {
            try
            {
                Log.Info($"Player {target.nicknameSync.MyNick} was set to Subclass: {subclass.FileName}");
                if (target.authManager.InstanceMode != ClientInstanceMode.ReadyClient || target.nicknameSync.MyNick == "Dedicated Server")
                {
                    Log.Error("Tried setting subclass to Dedicated Server or Unready Client!", nameof(SubclassManager));
                    return;
                }
                Player player = Player.Get(target);
                player.SetRole(subclass.Role);
                if (subclass.ClearInventoryOnSpawn)
                {
                    player.ClearInventory();
                }
                //if the spawnrooms is more than one, otherwise just use the default spawn
                DoorVariant door = null;
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
                        RoomIdentifier chosenRoom = foundRooms.GetRandomItem();
                        if (subclass.AllowKeycardDoors)
                        {
                            if (DoorVariant.DoorsByRoom[chosenRoom].Where(x => !(x is ElevatorDoor)).Count() == 0)
                            {
                                Log.Warning($"Tried to get door out of room with no doors! Room: {chosenRoom.Name} Subclass: {subclass.FileName}");
                            }
                            else
                            {
                                door = DoorVariant.DoorsByRoom[chosenRoom].Where(x => !(x is ElevatorDoor)).GetRandomItem();
                            }
                        }
                        else
                        {
                            if (DoorVariant.DoorsByRoom[chosenRoom].Where(x => x.RequiredPermissions.CheckPermissions(null, target) && !(x is ElevatorDoor)).Count() == 0)
                            {
                                Log.Warning($"Tried to get door out of room with no doors! (No keycard needed) Room: {chosenRoom.Name} Subclass: {subclass.FileName}");
                            }
                            else
                            {
                                door = DoorVariant.DoorsByRoom[chosenRoom].Where(x => x.RequiredPermissions.CheckPermissions(null, target) && !(x is ElevatorDoor)).GetRandomItem();
                            }
                        }
                    }
                }
                foreach (KeyValuePair<ItemDefinition, short> pair in subclass.SpawnItems)
                {
                    int spawnAmount = pair.Value;
                    ItemDefinition item = pair.Key;
                    if (spawnAmount < 0)
                    {
                        player.RemoveItems(item.VanillaItemType, Math.Abs(spawnAmount));
                        continue;
                    }
                    if (item.IsAmmo)
                    {
                        spawnAmount = 1;
                        item.AmmoAmount = (ushort)spawnAmount;
                    }
                    for (int i = 0; i < spawnAmount; i++)
                    {
                        target.AddItem(item);
                    }
                }
                for (int i = 0; i < subclass.NumberOfRandomItems; i++)
                {
                    if (subclass.RandomItems.Keys.IsEmpty())
                    {
                        Log.Warning("Tried to spawn random items from an empty random item list. Subclass: " + subclass.FileName);
                        continue;
                    }
                    ItemType item = subclass.RandomItems.Keys.GetRandomItem();
                    short amount = subclass.RandomItems[item];
                    if (amount < 0)
                    {
                        player.RemoveItems(item, Math.Abs(amount));
                        continue;
                    }
                    if (item.ToString().Contains("Ammo"))
                    {
                        player.AddAmmo(item, (ushort)subclass.RandomItems[item]);
                        continue;
                    }
                    if (item.ToString().Contains("Gun"))
                    {
                        player.AddFirearm(item, (byte)subclass.RandomItems[item]);
                        continue;
                    }
                    for (int f = 0; f < subclass.RandomItems[item]; f++)
                    {
                        player.AddItem(item);
                    }
                }
                foreach (AbilityType ability in subclass.Abilities)
                {
                    if (AbilityManager.AbilityToType.ContainsKey(ability))
                    {
                        target.AddAbility(ability);
                    }
                    else
                    {
                        Log.Warning("Ability is missing! Ability: " + ability.ToString(), nameof(SubclassManager));
                    }
                }
                for (int i = 0; i < subclass.NumberOfCustomRandomItems; i++)
                {
                    if (subclass.RandomCustomItems.Keys.IsEmpty())
                    {
                        Log.Warning("Tried to spawn random custom items from an empty random item list. Subclass: " + subclass.FileName);
                        continue;
                    }
                    CustomItemType item = subclass.RandomCustomItems.Keys.GetRandomItem();
                    short amount = subclass.RandomCustomItems[item];
                    for (int f = 0; f < subclass.RandomCustomItems[item]; f++)
                    {
                        player.ReferenceHub.AddCustomItem(item);
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
                Timing.CallDelayed(0.15f, () => 
                {
                    if (door != null)
                    {
                        Vector3 pos = door.transform.position;
                        pos.y += 1f;
                        target.TryOverridePosition(pos, Vector3.forward);
                    }
                });
                Timing.CallDelayed(1f, () => 
                {
                    return;
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
                });
                foreach (EffectDefinition effect in subclass.SpawnEffects)
                {
                    target.playerEffectsController.ChangeState(effect.Name, effect.Intensity, effect.Duration);
                }
                if (RefHubToSubclassDict.ContainsKey(target))
                {
                    RefHubToSubclassDict[target] = subclass;
                }
                else
                {
                    RefHubToSubclassDict.Add(target, subclass);
                }
                if (subclass.MaxHealth != 0)
                {
                    target.SetMaxHealth(subclass.MaxHealth);
                    player.Heal(subclass.MaxHealth);
                }
                target.SendHint(hintFormatted, 30f);
                target.gameConsoleTransmission.SendToClient(hintFormatted, "green");
                target.SetHubRotation(target.gameObject.transform.forward);
            }
            catch(Exception ex) 
            {
                if (RefHubToSubclassDict.ContainsKey(target))
                {
                    RefHubToSubclassDict[target] = subclass;
                }
                else
                {
                    RefHubToSubclassDict.Add(target, subclass);
                }
                Log.Error(ex.ToString(), nameof(SubclassManager));
            }
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

        public static void AddAbility(this ReferenceHub target, AbilityType type)
        {
            AbilityManager.AddAbility(target, type);
        }

        public static void RemoveAbility<T>(this ReferenceHub target) where T : CustomAbility
        {
            AbilityManager.RemoveAbility<T>(target);
        }

        public static T GetAbility<T>(this ReferenceHub target) where T : CustomAbility
        {
            return AbilityManager.GetAbility<T>(target);
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
            healthStat.NewMaxValue = amount;
        }

        public static List<ReferenceHub> GetAllDisarmedPlayersByDisarmer(this ReferenceHub disarmer)
        {
            List<DisarmedPlayers.DisarmedEntry> disarmedEntries = DisarmedPlayers.Entries.Where(x => x.Disarmer == disarmer.networkIdentity.netId).ToList();
            List<uint> netIds = disarmedEntries.Select(x => x.DisarmedPlayer).ToList();
            List<ReferenceHub> hubs = ReferenceHub.AllHubs.Where(x => netIds.Contains(x.networkIdentity.netId)).ToList();
            return hubs;
        }

        public static void AddItem(this ReferenceHub target, ItemDefinition itemDefinition)
        {
            Firearm firearm = null;
            ItemBase itemBase = null;
            CustomItem customItem = null;
            if (itemDefinition.IsAmmo)
            {
                ushort amount = itemDefinition.AmmoAmount;
                if (itemDefinition.IsCustomItem)
                {
                    target.AddCustomItem(itemDefinition.CustomItemType, (short)amount);
                }
                else
                {
                    target.AddItem(itemDefinition.VanillaItemType, amount);
                }
                return;
            }
            if (itemDefinition.IsCustomItem)
            {
                customItem = target.AddCustomItem(itemDefinition.CustomItemType);
                itemBase = customItem.ItemBase;
            }
            else
            {
                itemBase = target.AddItem(itemDefinition.VanillaItemType);
            }
            if(itemDefinition.VanillaItemType == ItemType.MicroHID)
            {
                MicroHIDItem micro = itemBase as MicroHIDItem;
                if (micro == null) { return; }
                micro.SetCharge(itemDefinition.Charge);
            }
            if(itemDefinition.VanillaItemType == ItemType.Radio)
            {
                RadioItem radio = itemBase as RadioItem;
                if(radio == null) { return; }
                radio.BatteryPercent = itemDefinition.Charge;
            }
            if (itemDefinition.IsFirearm)
            {
                firearm = itemBase as Firearm;
                if (firearm == null)
                {
                    Log.Error("ItemBase given is not a firearm!");
                    return;
                }
                if (itemDefinition.FirearmDefinition != null)
                {
                    if (itemDefinition.FirearmDefinition.Attachments == 0 && !itemDefinition.FirearmDefinition.NoAttachments)
                    {
                        firearm.ApplyAttachments();
                    }
                }
                byte ammo = itemDefinition.FirearmDefinition.Ammo;
                if (ammo == 0 && !itemDefinition.FirearmDefinition.NoAmmo)
                {
                    ammo = firearm.AmmoManagerModule.MaxAmmo;
                }
                if (itemDefinition.FirearmDefinition.ClampToMaxAmmo)
                {
                    ammo = Math.Min(ammo, firearm.AmmoManagerModule.MaxAmmo);
                }
                firearm.Status = new FirearmStatus(ammo, itemDefinition.FirearmDefinition.Flags, firearm.Status.Attachments);
            }
        }

        public static CustomItem AddCustomItem(this ReferenceHub target, CustomItemType type)
        {
            return CustomItemManager.AddItem(target, type);
        }

        public static CustomItem[] AddCustomItem(this ReferenceHub target, CustomItemType type, short amount)
        {
            List<CustomItem> items = new List<CustomItem>();
            for(int i = 0; i > amount; i++)
            {
                CustomItem item = AddCustomItem(target, type);
                if(item == null)
                {
                    Log.Warning($"Failed to give custom item to target! Target: {target.nicknameSync.MyNick}, Item: {type}");
                    continue;
                }
                items.Add(item);
            }
            return items.ToArray();
        }

        public static ItemBase AddItem(this ReferenceHub target, ItemType type)
        {
            return target.inventory.ServerAddItem(type);
        }

        public static ItemBase[] AddItem(this ReferenceHub target, ItemType type, int amount)
        {
            ItemBase[] items = new ItemBase[amount];
            for (int i = 0; i < amount; i++)
            {
                items[i] = target.inventory.ServerAddItem(type);
            }
            return items;
        }

        public static void RemoveItems(this ReferenceHub target, ItemType type, int amount)
        {
            Player p = Player.Get(target);
            p.RemoveItems(type, amount);
        }

        public static PlayerRoleBase GetLastLife(this ReferenceHub target)
        {
            if (DeathHandler.Instance.deadToOldRoles.TryGetValue(target.characterClassManager.netId, out PlayerRoleBase value))
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        public static void SetHubRotation(this ReferenceHub hub, Vector3 forward)
        {
            ValueTuple<ushort, ushort> valueTuple = Quaternion.LookRotation(forward, Vector3.up).ToClientUShorts();
            ushort item = valueTuple.Item1;
            ushort item2 = valueTuple.Item2;
            hub.SetHubRotation(item, item2);
        }

        public static void SetHubRotation(this ReferenceHub hub, Quaternion rotation)
        {
            ValueTuple<ushort, ushort> valueTuple = rotation.ToClientUShorts();
            ushort item = valueTuple.Item1;
            ushort item2 = valueTuple.Item2;
            hub.SetHubRotation(item, item2);
        }

        public static void SetHubRotation(this ReferenceHub hub, ushort horizontal, ushort vertical)
        {
            if (!(hub.roleManager.CurrentRole is IFpcRole))
            {
                return;
            }
            FpcPositionMessageWriter.appliedMouseLook = new ValueTuple<ushort, ushort>(horizontal, vertical);
            FpcPositionMessageWriter.valuesToApply |= FpcPositionMessageWriter.AppliedValues.ApplyMouseLook;
            hub.connectionToClient.Send<FpcPositionMessage>(new FpcPositionMessage(hub), 0);
        }
    }
}
