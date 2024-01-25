using PluginAPI.Events;
using PluginAPI.Enums;
using PluginAPI.Core.Attributes;
using PluginAPI.Core;
using UnityEngine;

namespace LurkBoisModded.EventHandlers
{
    [EventHandler]
    public class GunDebug
    {
        public void OnPlayerShoot(PlayerShotWeaponEvent ev)
        {
            Player target = ev.Player;
            Ray r = new Ray(target.Camera.position, Vector3.forward);
            bool result = Physics.Raycast(r, out RaycastHit hitInfo);
            if (result)
            {
                target.SendConsoleMessage($"{hitInfo.transform.gameObject.name}, {hitInfo.transform.gameObject.tag}, {hitInfo.transform.parent.gameObject.name}, {hitInfo.transform.root}");
            }
            else
            {
                target.SendConsoleMessage("did not hit anything.");
            }
        }
    }
}
