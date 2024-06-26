﻿using LurkBoisModded.Base;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Managers;

namespace LurkBoisModded.Subclasses
{
    public class MtfCommander : Subclass
    {
        public override string FileName => "mtf_commander";

        public override string SubclassNiceName => "MTF Commander";

        public override string SubclassDescription => "Use your noclip key (left alt by default) to provide a buff to nearby teammates!";

        public override RoleTypeId TargetRole => RoleTypeId.NtfSergeant;

        public override bool TargetRoleUsed => true;

        public override string ClassColor => "blue";

        public override RoleTypeId Role => RoleTypeId.NtfCaptain;

        public override Dictionary<ItemDefinition, short> SpawnItems => new Dictionary<ItemDefinition, short>() 
        {
            [new ItemDefinition(ItemType.GunFRMG0)] = -1,
            [new ItemDefinition(ItemType.GunE11SR)] = 1,
        };

        public override List<AbilityType> Abilities => new List<AbilityType>() { AbilityType.Inspire };

        public MtfCommander()
        {

        }
    }
}
