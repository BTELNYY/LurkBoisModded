using MEC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace LurkBoisModded.EnvriomentalHazards
{
    public abstract class BasicTimedHazard : BasicHazard
    {
        private Stopwatch stopwatch = new Stopwatch();

        public abstract float Duration { get; }
        
        public float TimeRemaining
        {
            get
            {
                return (Duration * 1000) - (stopwatch.ElapsedMilliseconds);
            }
        }

        public void StartStopwatch()
        {
            stopwatch.Start();
        }

        public void Pause()
        {
            if(stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
            else
            {
                stopwatch.Start();
            }
        }
        
        public void Reset()
        {
            stopwatch.Reset();
        }

        void Start()
        {
            stopwatch.Start();
        }

        void Update()
        {
            if((stopwatch.ElapsedMilliseconds * 1000) <= Duration)
            {
                Destroy();
            }
        }
    }
}
