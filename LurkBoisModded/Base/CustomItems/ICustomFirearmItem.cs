using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base.CustomItems
{
    public interface ICustomFirearmItem
    {
        bool OnShot();
        bool OnReloadStart();
        void OnReloadFinish(IAmmoManagerModule module, Firearm firearm);
        bool OnPlayerShotByWeapon(FirearmDamageHandler damageHandlerBase, ReferenceHub target);
    }
}
