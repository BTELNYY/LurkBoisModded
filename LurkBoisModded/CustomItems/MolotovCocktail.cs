using InventorySystem.Items.ThrowableProjectiles;
using LurkBoisModded.Base;
using LurkBoisModded.EnvriomentalHazards;
using LurkBoisModded.EnvriomentalHazards.Hazards.Fire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.CustomItems
{
    public class MolotovCocktail : CustomItem, ICustomGrenadeItem
    {
        public override CustomItemType CustomItemType => CustomItemType.MolotovCocktail;

        public override ItemType BaseItemType => ItemType.GrenadeFlash;

        public override bool OnItemEquip()
        {
            CurrentOwner.SendHint(Plugin.GetConfig().MolotovConfiguration.HeldTip);
            return base.OnItemEquip();
        }

        public bool OnFuseEnd(EffectGrenade grenade)
        {
            FireHazard haz = (FireHazard)HazardManager.CreateHazard(HazardType.Fire);
            haz.gameObject.transform.position = grenade.transform.position;
            grenade.DestroySelf();
            return false;
        }
    }
}
