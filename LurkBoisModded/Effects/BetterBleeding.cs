﻿using CustomPlayerEffects;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Effects
{
    public class BetterBleeding : TickingEffectBase, IHealablePlayerEffect
    {
        public bool IsHealable(ItemType item)
        {
            if(item == ItemType.Medkit || item == ItemType.SCP500)
            {
                return true;
            }
            return false;
        }

        public override void OnTick()
        {
            float damage = 2 * Intensity;
            UniversalDamageHandler handler = new UniversalDamageHandler(damage, DeathTranslations.Bleeding);
            Hub.playerStats.DealDamage(handler);
        }
    }
}
