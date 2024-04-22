using InventorySystem.Items.ThrowableProjectiles;
using LurkBoisModded.Base.CustomItems;
using LurkBoisModded.EnvriomentalHazards;
using LurkBoisModded.Extensions;
using LurkBoisModded.EnvriomentalHazards.Hazards.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Items.Pickups;
using PlayerRoles;
using PluginAPI.Core;

namespace LurkBoisModded.CustomItems
{
    public class MolotovCocktail : CustomItem, ICustomGrenadeItem
    {
        public override CustomItemType CustomItemType => CustomItemType.MolotovCocktail;

        public override ItemType BaseItemType => ItemType.GrenadeFlash;

        FireHazard _createdHazard;

        public override bool OnItemEquip()
        {
            CurrentOwner.SendHint(Plugin.GetConfig().MolotovConfiguration.HeldTip);
            return base.OnItemEquip();
        }

        public override void OnItemPickedUp(ReferenceHub newOwner)
        {
            base.OnItemPickedUp(newOwner);
            CurrentOwner.SendHint(Plugin.GetConfig().MolotovConfiguration.PickupTip);
        }

        public bool OnFuseEnd(EffectGrenade grenade)
        {
            FireHazard haz = (FireHazard)HazardManager.CreateHazard(HazardType.Fire);
            haz.gameObject.transform.position = grenade.transform.position;
            haz.Owner = new Footprinting.Footprint(grenade.PreviousOwner.Hub);
            haz.OwnerTeam = grenade.PreviousOwner.Hub.GetTeam();
            haz.Create();
            _createdHazard = haz;
            grenade.DestroySelf();
            return false;
        }

        public override void OnItemDestroyed()
        {
            if(_createdHazard != null)
            {
                _createdHazard?.Destroy();
            }
            base.OnItemDestroyed();
        }
    }
}
