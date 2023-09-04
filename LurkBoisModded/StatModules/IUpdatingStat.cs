using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LurkBoisModded.StatModules
{
    public interface IUpdatingStat
    {
        void Start();

        void Update();

        void ClassChanged();
    }
}
