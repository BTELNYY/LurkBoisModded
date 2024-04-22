using LurkBoisModded.Base;
using LurkBoisModded.Base.CustomItems;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Subclasses
{
    public class MtfPyro : Subclass
    {
        public override RoleTypeId Role => RoleTypeId.NtfPrivate;

        public override string ClassColor => "blue";

        public override string SubclassNiceName => "MTF Pyro";

        public override string FileName => "mtf_pyro";

        public override string SubclassDescription => "A little bit of insanity mixed with fire is what every team needs.";

        public override Dictionary<CustomItemType, short> CustomItems => new Dictionary<CustomItemType, short>() 
        {
            [CustomItemType.MolotovCocktail] = 2
        };

        public MtfPyro() { }
    }
}
