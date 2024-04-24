using HarmonyLib;
using InventorySystem.Items.Radio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Random;

namespace LurkBoisModded.Extensions
{
    public static class RadioitemExtensions
    {
        public static void SendStatusMessage(this RadioItem item)
        {
            AccessTools.Method(typeof(RadioItem), "SendStatusMessage").Invoke(item, null);
        }

        public static void SetEnabled(this RadioItem item, bool state)
        {
            AccessTools.Field(typeof(RadioItem), "_enabled").SetValue(item, state);
            item.SendStatusMessage();
        }

        public static void SetBattery(this RadioItem item, float newBattery) 
        {
            AccessTools.Field(typeof(RadioItem), "_battery").SetValue(item, newBattery);
            item.SendStatusMessage();
        }

        public static void SetRange(this RadioItem item, RadioMessages.RadioRangeLevel level)
        {
            AccessTools.Field(typeof(RadioItem), "_rangeId").SetValue(item, level);
            item.SendStatusMessage();
        }
    }
}
