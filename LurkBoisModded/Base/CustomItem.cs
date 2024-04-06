using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using LurkBoisModded.EventHandlers.Item;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginAPI.Core.Attributes;
using UnityEngine;
using PluginAPI.Enums;

namespace LurkBoisModded.Base
{
    public abstract class CustomItem : MonoBehaviour
    {
        public abstract CustomItemType CustomItemType { get; }

        public abstract ItemType BaseItemType { get; }

        public bool IsHeldItem 
        { 
            get
            {
                if(CurrentOwner == null)
                {
                    return false;
                }
                if(CurrentOwner.inventory.CurItem.SerialNumber == TrackedSerial)
                {
                    return true;
                }
                return false;
            }
            set
            {
                if(CurrentOwner == null)
                {
                    return;
                }
                if (value)
                {
                    CurrentOwner.inventory.ServerSelectItem(TrackedSerial);
                }
                else
                {
                    CurrentOwner.inventory.ServerSelectItem(0);
                }
            }
        }

        public ItemBase ItemBase 
        {
            get
            {
                if(ItemState == ItemState.Inventory)
                {
                    return _itemBaseReference;
                }
                return null;
            }
            private set 
            {
                _itemBaseReference = value;
            } 
        }

        private ItemBase _itemBaseReference;

        private ItemPickupBase _itemPickupBase;

        public ItemPickupBase ItemPickupBase
        {
            get
            {
                return _itemPickupBase;
            }
        }

        public ushort TrackedSerial
        {
            get
            {
                return _trackedSerial;
            }
            private set
            {
                _trackedSerial = value;
            }
        }

        private ushort _trackedSerial;

        public ReferenceHub CurrentOwner 
        { 
            get
            {
                return _currentOwner;
            }
        }

        private ReferenceHub _currentOwner;

        public ItemState ItemState 
        { 
            get
            {
                return _state;
            } 
        }

        private ItemState _state;

        public virtual void OnItemPickedUp(ReferenceHub newOwner)
        {
            _currentOwner = newOwner;
            _state = ItemState.Inventory;

            if (newOwner.inventory.UserInventory.Items.ContainsKey(TrackedSerial))
            {
                _itemBaseReference = newOwner.inventory.UserInventory.Items[TrackedSerial];
            }
            else
            {
                Log.Warning("For some ungodly reason the fucking tracked serial ID isn't in the inventory (what the fuck?)");
            }
        }

        public virtual bool OnItemEquip()
        {
            return true;
        }

        public virtual void OnItemDropped(ReferenceHub lastOwner, ItemPickupBase pickupBase)
        {
            _state = ItemState.Dropped;
            _currentOwner = null;
            _itemBaseReference = null;
            _itemPickupBase = pickupBase;
        }

        public virtual void OnItemCreated(ReferenceHub owner, ushort serial)
        {
            _currentOwner = owner;
            TrackedSerial = serial;
            _state = ItemState.Inventory;
            gameObject.name = serial.ToString();
            _itemBaseReference = owner.inventory.UserInventory.Items[TrackedSerial];
            if (!CustomItemHandler.SerialToItem.ContainsKey(TrackedSerial))
            {
                CustomItemHandler.SerialToItem.Add(TrackedSerial, this);
            }
        }

        public virtual void OnItemDestroyed()
        {
            CustomItemHandler.SerialToItem.Remove(TrackedSerial);
            if (ItemState == ItemState.Inventory)
            {
                CurrentOwner.inventory.ServerRemoveItem(TrackedSerial, ItemPickupBase);
            }
            GameObject.Destroy(this);
        }

        public virtual void ForceDropItem()
        {
            ItemPickupBase item = CurrentOwner.inventory.ServerDropItem(TrackedSerial);
        }

        public virtual void OnRoundRestart()
        {
            _currentOwner = null;
            _itemBaseReference = null;
            _trackedSerial = 0;
            _state = ItemState.Dropped;
        }
    }

    public enum ItemState
    {
        Inventory,
        Dropped
    }

    public enum CustomItemType
    {
        None,
        SniperE11,
        Landmine,
        MolotovCocktail,
    }
}
