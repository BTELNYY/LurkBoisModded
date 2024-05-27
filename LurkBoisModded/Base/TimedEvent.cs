using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public class TimedEvent
    {
        public float Duration { get; set; } = 0f;

        public float Delay { get; set; } = 0f;

        public Action Action { get; set; } = null;

        public bool RepeatEveryFrame { get; set; } = false;

        public TimedEvent(float delay, float duration, Action action)
        {
            Duration = duration;
            Delay = delay;
            Action = action;
        }

        public TimedEvent(float delay, float duration, bool repeatEveryFrame, Action action)
        {
            Duration = duration;
            Delay = delay;
            Action = action;
            RepeatEveryFrame = repeatEveryFrame;
        }

        public TimedEvent(float duration, Action action)
        {
            Duration = duration;
            Action = action;
        }

        public TimedEvent(float duration, bool repeatEveryFrame, Action action)
        {
            Duration = duration;
            Action = action;
            RepeatEveryFrame = repeatEveryFrame;
        }
    }
}
