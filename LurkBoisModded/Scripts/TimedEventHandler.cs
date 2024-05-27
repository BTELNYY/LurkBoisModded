using PluginAPI.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LurkBoisModded.Base;
using System.Collections.ObjectModel;

namespace LurkBoisModded.Scripts
{
    public class TimedEventHandler : MonoBehaviour
    {
        Stopwatch stopwatch = new Stopwatch();

        public virtual ReadOnlyCollection<TimedEvent> Events { get; } = new ReadOnlyCollection<TimedEvent>(new TimedEvent[]
        {
        
        });

        public TimedEvent CurrentEvent
        {
            get
            {
                return Events[currentIndex];
            }
        }

        int currentIndex = 0;

        float currentDelay = 0f;

        float currentDuration = 0f;

        bool hasExecuted = false;

        bool repeatEveryFrame = false;

        void Start()
        {
            stopwatch.Start();
            MoveToEvent(0);
        }

        void Update()
        {
            if (!hasExecuted)
            {
                if(stopwatch.Elapsed.TotalSeconds >= currentDelay)
                {
                    hasExecuted = true;
                    stopwatch.Reset();
                    stopwatch.Start();
                    CurrentEvent.Action.Invoke();
                }
            }
            else
            {
                if(stopwatch.Elapsed.TotalSeconds >= currentDuration)
                {
                    MoveToNextEvent();
                }
                else
                {
                    if (repeatEveryFrame)
                    {
                        CurrentEvent.Action.Invoke();
                    }
                }
            }
        }

        public void MoveToNextEvent()
        {
            int index = currentIndex + 1;
            MoveToEvent(index);
        }

        public void MoveToEvent(int index)
        {
            if (index > Events.Count - 1)
            {
                return;
            }
            else
            {
                currentIndex = index;
                currentDuration = Events[currentIndex].Duration;
                currentDelay = Events[currentIndex].Delay;
                hasExecuted = false;
                repeatEveryFrame = Events[currentIndex].RepeatEveryFrame;
                stopwatch.Reset();
                stopwatch.Start();
            }
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

        public void Pause(bool paused)
        {
            if(paused)
            {
                stopwatch.Start();
            }
            else
            {
                stopwatch.Stop();
            }
        }

        public void Restart()
        {
            MoveToEvent(0);
        }
    }
}
