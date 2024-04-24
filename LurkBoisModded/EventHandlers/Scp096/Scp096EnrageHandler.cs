using PluginAPI.Enums;
using PluginAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.EventHandlers.Scp096
{
    [EventHandler]
    public class Scp096EnrageHandler
    {
        [PluginAPI.Core.Attributes.PluginEvent(ServerEventType.Scp096Enraging)]
        public void OnEnrage(Scp096EnragingEvent ev)
        {
            if (Config.CurrentConfig.Scp096Config.FlickerLightsOnEnrage)
            {
                foreach(RoomLightController controller in RoomLightController.Instances.Where(x => x.Room.Zone == ev.Player.Zone)) 
                {
                    controller.ServerFlickerLights(Config.CurrentConfig.Scp096Config.FlickerLightTime);
                }
            }
        }
    }
}
