using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace SimulHelper
{
    public class Ticker
    {
        public uint Tick { get; protected set; }
        protected float Time = 0;
        protected List<SimulationSystem> SimSystems = new List<SimulationSystem>();
        protected Dictionary<Type, SimulationSystem> SimSystemsDict = new Dictionary<Type, SimulationSystem>();
        protected Utf8JsonWriter JsonWriter;
        public Ticker(string log)
        {
            Tick = 0;
            JsonWriter = new Utf8JsonWriter(File.OpenWrite(log));
            JsonWriter.WriteStartArray();
        }

        [Obsolete("Use UpdateWithDeps instead")]
        public void Update()
        {
            Tick++;
            foreach (var system in SimSystems)
            {
                try
                {
                    DataRandom.Seed += system.DataRandomAdd();
                }
                catch (OverflowException)
                {
                    DataRandom.Seed = DataRandom.Next(-999999, 999999);
                }
                system.Update(this);
                JsonWriter.WriteStartObject();
                system.Serialize(JsonWriter);
                JsonWriter.WriteEndObject();
            }
        }

        public void UpdateWithDeps()
        {
            Tick++;
            foreach (var system in SimSystemsDict.Values)
            {
                try
                {
                    DataRandom.Seed += system.DataRandomAdd();
                }
                catch (OverflowException)
                {
                    DataRandom.Seed = DataRandom.Next(-999999, 999999);
                }
                system.Update(this);
                JsonWriter.WriteStartObject();
                system.Serialize(JsonWriter);
                JsonWriter.WriteEndObject();
            }
        }

        public void End()
        {
            foreach (var system in SimSystems)
            {
                system.End();
            }
            JsonWriter.WriteEndArray();
            JsonWriter.Flush();
        }

        [Obsolete("Use RegisterSystemDependency instead")]
        public void RegisterSystem(SimulationSystem system)
        {
            SimSystems.Add(system);
        }

        public void RegisterSystemDependency<System>(System sys) where System : SimulationSystem
        {
            SimSystemsDict.Add(sys.GetType(), sys);
        }

        public System Resolve<System>() where System : SimulationSystem
        {
            if (SimSystemsDict.TryGetValue(typeof(System), out var sys))
            {
                return sys as System;
            }
            return null;
        }
    }
}
