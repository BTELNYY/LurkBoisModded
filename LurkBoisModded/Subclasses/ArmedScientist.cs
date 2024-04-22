using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class ArmedScientist : Subclass
    {
        public override RoleTypeId Role => RoleTypeId.Scientist;

        public override string FileName => "scientist_armed";

        public override string SubclassNiceName => "Armed Scientist";

        public override string SubclassDescription => "You have a gun with very little ammo, use it well!";

        public override string ClassColor => "#FFFF7C";

        public override Dictionary<ItemType, short> SpawnItems => new Dictionary<ItemType, short>() 
        {
            [ItemType.GunCOM15] = 3,
        };
    }
}
