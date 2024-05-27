using CustomPlayerEffects;
using PluginAPI.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.InteropServices;
using PlayerRoles.FirstPersonControl;
using LurkBoisModded.Extensions;

namespace LurkBoisModded.Effects
{
    public class Spin : TickingEffectBase
    {
        public override void OnTick()
        {
            Player p = Player.Get(Hub);
            Vector3 vec = Vector3.zero;
            vec.x = Random.Range(-100f * Intensity, 100f * Intensity);
            vec.y = Random.Range(-100f * Intensity, 100f * Intensity);
            vec.z = Random.Range(-100f * Intensity, 100f * Intensity);
            Hub.SetHubRotation(vec);
        }
    }
}