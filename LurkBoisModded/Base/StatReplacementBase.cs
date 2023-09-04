using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public class StatReplacementBase
    {
        public SyncedStatBase StatBase { get; set; }

        public ReferenceHub AssignedHub { get; set; }

        public float MaxValue { get; set; }

        public float MinValue { get; set; }

        public virtual StatBaseType StatBaseType { get; }
    }

    public enum StatBaseType
    {
        Health,
        Ahp,
        Hume,
    }
}
