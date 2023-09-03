using MapGeneration;
using PlayerRoles;
using System;
using System.Collections.Generic;
namespace LurkBoisModded.Base
{
    [Serializable]
    public class SubclassBase
    {
        public virtual string FileName { get; set; } = "default";

        public virtual string SubclassNiceName { get; set; } = "Default Text";

        public virtual string SubclassDescription { get; set; } = "Default Text";

        public virtual List<AbilityType> Abilities { get; set; } = new List<AbilityType>();

        public virtual RoleTypeId Role { get; set; } = RoleTypeId.ClassD;

        public virtual Dictionary<ItemType, ushort> SpawnItems { get; set; } = new Dictionary<ItemType, ushort>();

        public virtual bool ClearInventoryOnSpawn { get; set; } = false;

        public virtual List<RoomName> SpawnRooms { get; set; } = new List<RoomName>();

        public virtual float[] HeightVariety { get; set; } = new float[2] { 0.95f, 1.1f };

        public SubclassBase()
        {

        }
    }
}
