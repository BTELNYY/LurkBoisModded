using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Gamemodes.Deathmatch
{
    [Gamemode]
    public class DeathmatchGamemode : Gamemode
    {
        public override string Name => "Deathmatch";

        public override bool RequireRoundRestart => true;

        public override List<Type> EventHandlers => new List<Type> 
        {

        };
    }
}
