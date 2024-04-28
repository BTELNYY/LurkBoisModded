using MapGeneration;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using LurkBoisModded.Managers;
using LurkBoisModded.Base.CustomItems;

namespace LurkBoisModded.Base
{
    [Serializable]
    public class Subclass
    {
        public virtual string FileName { get; set; } = "default";

        public virtual string SubclassNiceName { get; set; } = "Default Text";

        public virtual string SubclassDescription { get; set; } = "Default Text";

        public virtual List<AbilityType> Abilities { get; set; } = new List<AbilityType>();

        public virtual RoleTypeId Role { get; set; } = RoleTypeId.ClassD;

        public virtual Dictionary<ItemDefinition, short> SpawnItems { get; set; } = new Dictionary<ItemDefinition, short>();

        public virtual bool ClearInventoryOnSpawn { get; set; } = false;

        public virtual List<RoomName> SpawnRooms { get; set; } = new List<RoomName>();

        public virtual List<NonNamedRoomDefinition> NonNamedRoomSpawns { get; set; } = new List<NonNamedRoomDefinition>();

        public virtual bool AllowKeycardDoors { get; set; } = false;

        public virtual float[] HeightVariety { get; set; } = new float[2] { 0.95f, 1.1f };

        public virtual string ClassColor { get; set; } = "white";

        public virtual bool ApplyClassColorToCustomInfo { get; set; } = false;

        public virtual float MaxHealth { get; set; } = 0f;

        public virtual Dictionary<ItemType, short> RandomItems { get; set; } = new Dictionary<ItemType, short>();

        public virtual int NumberOfRandomItems { get; set; } = 0;

        public virtual Dictionary<CustomItemType, short> RandomCustomItems { get; set; } = new Dictionary<CustomItemType, short>();

        public virtual int NumberOfCustomRandomItems { get; set; } = 0;

        public virtual List<EffectDefinition> SpawnEffects { get; set; } = new List<EffectDefinition> { };

        public virtual bool AllowEscape { get; set; } = true;

        public virtual string EscapeFailMessage { get; set; } = "This subclass cannot escape!";

        public Subclass()
        {

        }
    }
    public class NonNamedRoomDefinition
    {
        public RoomShape RoomShape { get; set; } = RoomShape.Undefined;

        public FacilityZone FacilityZone { get; set; } = FacilityZone.None;
    }
}
