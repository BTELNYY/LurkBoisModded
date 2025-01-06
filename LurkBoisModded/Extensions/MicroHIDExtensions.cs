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
            float energy = (float)Convert.ToDouble(percent) / 100;
            item.EnergyManager.ServerSetEnergy(item.ItemSerial, energy);
        }

        public static byte EnergyToByte(this MicroHIDItem item)
        {
            return (byte)Mathf.RoundToInt(Mathf.Clamp01(item.EnergyManager.Energy) * 255f);
        }
    }
}
