using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public interface IRequiredItemAbility
    {
        ItemType RequiredItemType { get; }
    }
}
