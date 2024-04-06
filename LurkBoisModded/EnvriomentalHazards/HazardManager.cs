using LurkBoisModded.EnvriomentalHazards.Hazards.Fire;
using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LurkBoisModded.EnvriomentalHazards
{
    public class HazardManager
    {
        private static Dictionary<HazardType, Type> hazards = new Dictionary<HazardType, Type>()
        {
            [HazardType.Fire] = typeof(FireHazard)
        };

        private static ushort currentSerial = 0;

        public static ushort GetNextSerial()
        {
            return currentSerial++;
        }

        public static BasicHazard CreateHazard(HazardType type)
        {
            if (!hazards.ContainsKey(type))
            {
                Log.Warning("Hazard type isnt registered: " + type);
                return null;
            }
            GameObject hazardObject = new GameObject($"Hazard");
            BasicHazard hazard = (BasicHazard)hazardObject.AddComponent(hazards[type]);
            if(hazard == null)
            {
                GameObject.Destroy(hazardObject);
                Log.Warning("Failed to create instance of Hazard: " + type);
                return null;
            }
            return hazard;
        }
    }

    public enum HazardType
    {
        Fire
    }
}
