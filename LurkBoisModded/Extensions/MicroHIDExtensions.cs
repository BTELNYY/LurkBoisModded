using HarmonyLib;
using InventorySystem.Items.MicroHID;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LurkBoisModded.Extensions
{
    public static class MicroHIDExtensions
    {
        public static void SetCharge(this MicroHIDItem item, byte percent)
        {
            percent = Math.Min((byte)100, percent);
            item.RemainingEnergy = 1f;
            item.SendMessage(HidStatusMessageType.EnergySync, item.EnergyToByte());
        }

        public static byte EnergyToByte(this MicroHIDItem item)
        {
            return (byte)Mathf.RoundToInt(Mathf.Clamp01(item.RemainingEnergy) * 255f);
        }

        public static void SendMessage(this MicroHIDItem item, HidStatusMessageType msgType, byte code) 
        {
            AccessTools.Method(typeof(MicroHIDItem), "ServerSendStatus").Invoke(item, new object[] { msgType, code });
        }
    }
}
