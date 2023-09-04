using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public class EffectDefinition
    {
        [Description("Name of the effect")]
        public string Name { get; set; }
        [Description("Intensity (0-255)")]
        public byte Intensity { get; set; }
        [Description("Duration in Seconds")]
        public float Duration { get; set; }

        public EffectDefinition()
        {

        }

        public EffectDefinition(string name, byte intensity, float duration)
        {
            Name = name;
            Intensity = intensity;
            Duration = duration;
        }
    }
}
