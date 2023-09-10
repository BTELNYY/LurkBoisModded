using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public interface ICustomFirearmItem
    {
        void OnShot();
        void OnReloadStart();
        void OnReloadFinish(IAmmoManagerModule module, Firearm firearm);
        void OnDamageByItem(DamageHandlerBase damageHandlerBase, ReferenceHub target);
    }
}
