﻿using InventorySystem.Items.Firearms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Base
{
    public class FirearmDefinition
    {
        public FirearmDefinition(byte ammo, uint attachments, bool clampToMaxAmmo)
        {
            Ammo = ammo;
            Attachments = attachments;
            ClampToMaxAmmo = clampToMaxAmmo;
        }

        public FirearmDefinition(bool clampToMaxAmmo)
        {
            ClampToMaxAmmo = clampToMaxAmmo;
        }

        public FirearmDefinition() { }

        public byte Ammo { get; set; } = 0;

        public uint Attachments { get; set; } = 0;

        public bool ClampToMaxAmmo { get; set; } = true;

        public bool NoAmmo { get; set; } = false;

        public bool NoAttachments { get; set; } = false;
    }
}
