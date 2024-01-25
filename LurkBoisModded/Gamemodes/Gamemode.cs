using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Gamemodes
{
    public class Gamemode
    {
        public virtual string Name { get; } = "Gamemode";

        public virtual bool RequireRoundRestart { get; } = false;

        public virtual List<Type> EventHandlers { get; } = new List<Type>();

        public virtual bool RegisterSelfAsEventHandler { get; } = false;

        public virtual void OnStarted()
        {

        }

        public virtual void OnTerminate()
        {

        }
    }
}
