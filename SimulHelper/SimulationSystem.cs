using System;
using System.Collections.Generic;
using System.Text;

namespace SimulHelper
{
    public abstract class SimulationSystem
    {
        public abstract void Update(uint tick);
        public abstract void End();
    }
}
