using CustomPlayerEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.Managers
{
    public class CustomEffectManager
    {
        public static List<Type> CustomEffects
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(StatusEffectBase)) && !x.IsAbstract).ToList();
            }
        }
    }
}
