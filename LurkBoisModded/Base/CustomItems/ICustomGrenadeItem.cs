using InventorySystem.Items.ThrowableProjectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base.CustomItems
{
    public interface ICustomGrenadeItem
    {
        bool OnFuseEnd(EffectGrenade grenade);
    }
}
