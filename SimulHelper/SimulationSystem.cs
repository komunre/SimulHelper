using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SimulHelper
{
    public abstract class SimulationSystem
    {
        public abstract void Update(uint tick);
        public abstract void End();
        public abstract void Serialize(Utf8JsonWriter writer);
        public virtual int DataRandomAdd()
        {
            return 0;
        }
    }
}
