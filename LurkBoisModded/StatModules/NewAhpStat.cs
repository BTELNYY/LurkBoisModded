using HarmonyLib;
using PlayerStatsSystem;
using System.Collections.Generic;

namespace LurkBoisModded.StatModules
{
    public class NewAhpStat : AhpStat
    {
        public override float CurValue { get => base.CurValue; set => base.CurValue = value; }

        public override float MaxValue => SetMaxValue;

        public float SetMaxValue = 75f;

        public override float MinValue => SetMinValue;

        public float SetMinValue = 0f;

        public List<AhpProcess> AhpProcesses
        {
            get
            {
                List<AhpProcess> procList = AccessTools.FieldRefAccess<AhpStat, List<AhpProcess>>(this, "_activeProcesses");
                AhpProcesses = procList;
                return procList;
            }
            set
            {
                List<AhpProcess> procList = AccessTools.FieldRefAccess<AhpStat, List<AhpProcess>>(this, "_activeProcesses");
                procList = value;
                AhpProcesses = value;
            }
        }
    }
}
