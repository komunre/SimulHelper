using System;
using System.Collections.Generic;

namespace SimulHelper
{
    public class Ticker
    {
        public uint Tick { get; protected set; }
        protected float Time = 0;
        protected List<SimulationSystem> SimSystems = new List<SimulationSystem>();
        public Ticker()
        {
            Tick = 0;
        }

        public void Update()
        {
            Tick++;
            foreach (var system in SimSystems)
            {
                system.Update(Tick);
            }
        }

        public void End()
        {
            foreach (var system in SimSystems)
            {
                system.End();
            }

        }

        public void RegisterSystem(SimulationSystem system)
        {
            SimSystems.Add(system);
        }
    }
}
