using InventorySystem.Items.Radio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public interface ICustomRadioItem
    {
        bool OnToggled(ReferenceHub hub, RadioItem item, bool state);

        bool OnUse(ReferenceHub hub, RadioItem item, float drain);

        bool OnRangeChanged(ReferenceHub hub, RadioItem item, RadioMessages.RadioRangeLevel rangeLevel);
    }
}
