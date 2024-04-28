using HarmonyLib;
using InventorySystem.Disarming;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using LurkBoisModded.Base;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.Patches
{
    [HarmonyPatch(typeof(Escape), "ServerGetScenario")]
    public class EscapePatch
    {
        public static bool Prefix(ReferenceHub hub, ref object __result)
        {
            HumanRole humanRole = hub.roleManager.CurrentRole as HumanRole;
            if (humanRole == null)
            {
                __result = 0;
                return false;
            }
            Vector3 escapePos = (Vector3)AccessTools.Field(typeof(Escape), "WorldPos").GetValue(null);
            if ((humanRole.FpcModule.Position - escapePos).sqrMagnitude > 156.5f)
            {
                __result = 0;
                return false;
            }
            if (humanRole.ActiveTime < 10f)
            {
                __result = 0;
                return false;
            }
            Subclass currentSubclass = hub.GetSubclass();
            if (currentSubclass != null && !currentSubclass.AllowEscape)
            {
                hub.SendHint(currentSubclass.EscapeFailMessage);
                __result = 0;
                return false;
            }
            bool detained = hub.inventory.IsDisarmed();
            bool cuffedChangeTeam = (bool)AccessTools.Field(typeof(CharacterClassManager), "CuffedChangeTeam").GetValue(null);
            if (detained && !cuffedChangeTeam)
            {
                __result = 0;
                return false;
            }
            RoleTypeId roleTypeId = humanRole.RoleTypeId;
            if (roleTypeId == RoleTypeId.Scientist)
            {
                if (!detained)
                {
                    __result = 3;
                    return false;
                }
                __result = 4;
                return false;
            }
            if(roleTypeId == RoleTypeId.ClassD)
            {
                if (!detained)
                {
                    __result = 1;
                    return false;
                }
                __result = 2;
                return false;
            }
            if(roleTypeId == RoleTypeId.FacilityGuard)
            {
                if (!Config.CurrentConfig.GuardsCanEscape)
                {
                    __result = 0;
                    return false;
                }
                if (detained)
                {
                    __result = 4;
                    return false;
                }
                __result = 2;
                return false;
            }
            return false;
        }
    }
}
