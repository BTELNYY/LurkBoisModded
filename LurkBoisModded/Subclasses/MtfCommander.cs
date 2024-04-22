using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Subclasses
{
    public class MtfCommander : Subclass
    {
        public override string FileName => "mtf_commander";

        public override string SubclassNiceName => "MTF Commander";

        public override string SubclassDescription => "Use your noclip key (left alt by default) to provide a buff to nearby teammates!";

        public override string ClassColor => "blue";

        public override RoleTypeId Role => RoleTypeId.NtfCaptain;

        public override Dictionary<ItemType, short> SpawnItems => new Dictionary<ItemType, short>() 
        {
            [ItemType.GunFRMG0] = -1,
            [ItemType.GunE11SR] = 40,
        };

        public override List<AbilityType> Abilities => new List<AbilityType>() { AbilityType.Inspire };

        public MtfCommander()
        {

        }
    }
}
