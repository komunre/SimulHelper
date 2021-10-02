using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulHelper;
using SimulHelper.Math;
using SkiaSharp;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace SimulationExample
{
    public class TemperatureSystem : SimulationSystem
    {
        public float Temperature = 23;

        public override void End()
        {
            
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteNumber("temperature", Temperature);
        }

        public override void Update(uint tick)
        {
            Temperature += 0.005f * tick;
        }
    }

    public class WaterSystem : SimulationSystem
    {
        public float IceMass = 100;
        private float _waterTotal = 100;
        public TemperatureSystem TemperatureSystem;
        public LinearGraphHelper LinearGraphHelper = new LinearGraphHelper(1000, 100);
        private int _water;
        private int _ice;
        public WaterSystem()
        {
            _water = LinearGraphHelper.AddLine(true, new SKColor(0, 0, 255));
            _ice = LinearGraphHelper.AddLine(true, new SKColor(255, 0, 0));
        }
        public override void Update(uint tick)
        {
            var changed = TemperatureSystem.Temperature / 23 / 10;
            IceMass -= changed;
            _waterTotal += changed * 100;
            Logger.Log(IceMass + ":" + _waterTotal);
            LinearGraphHelper.TranslateLineH(_water,  _waterTotal / 100);
            LinearGraphHelper.TranslateLineH(_ice, IceMass);
        }

        public override void End()
        {
            LinearGraphHelper.Save("water_ice.png");
        }

        public override void Serialize(Utf8JsonWriter writer)
        {
            writer.WriteNumber("IceMass", IceMass);
            writer.WriteNumber("Water", _waterTotal);
        }
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IceMelt()
        {
            Ticker ticker = new Ticker("ice.json");
            var tempSystem = new TemperatureSystem();
            ticker.RegisterSystem(tempSystem);
            var waterSystem = new WaterSystem() { TemperatureSystem = tempSystem };
            ticker.RegisterSystem(waterSystem);

            while (waterSystem.IceMass >= 0)
            {
                ticker.Update();
            }

            ticker.End();
        }

        public class RandomSystem : SimulationSystem
        {
            private List<int> _lines = new List<int>();
            private LinearGraphHelper _lineGraph = new LinearGraphHelper(500, 500);
            public RandomSystem()
            {
                var random = new Random();
                for (int i = 0; i < 100; i++) {
                    _lines.Add(_lineGraph.AddLine(true, new SKColor((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255))));
                }
            }
            public override void End()
            {
                _lineGraph.Save("lines.png");
            }

            public override void Serialize(Utf8JsonWriter writer)
            {
                return;
            }

            public override void Update(uint tick)
            {
                var random = new Random();
                foreach (var line in _lines)
                {
                    _lineGraph.TranslateLineH(line, random.Next((int)_lineGraph.GraphBox[1].Y));
                }
            }
        }

        [TestMethod]
        public void RandomBig()
        {
            Ticker ticker = new Ticker("random.json");

            ticker.RegisterSystem(new RandomSystem());
            
            while (ticker.Tick < 500)
            {
                ticker.Update();
            }

            ticker.End();
        }
    }

    [TestClass]
    public class Breathing
    {
        public class Lungs : SimulationSystem
        {
            public float Pressure;
            public float MaxVolume;
            public float Volume = 0.1554f;
            public float BreathRate = 8f;
            public float InhaleExhale;
            public bool Inhaling = true;
            public float Force = 0.0f;
            public LinearGraphHelper LinearGraph;
            private int _volume;
            private int _inhale;
            private int _force;

            public Lungs(float heightMeters)
            {
                MaxVolume = 2.5f * heightMeters;
                InhaleExhale = MaxVolume / BreathRate / 60;
                LinearGraph = new LinearGraphHelper(1200, (int)MaxVolume);
                _volume = LinearGraph.AddLine(true, new SKColor(0, 100, 100));
                _inhale = LinearGraph.AddLine(true, new SKColor(0, 0, 255));
                _force = LinearGraph.AddLine(true, new SKColor(255, 0, 0));
            }

            public override void Update(uint tick)
            {
                if (Inhaling)
                {
                    Volume += InhaleExhale * Force;
                }
                else
                {
                    Volume -= InhaleExhale * Force;
                }

                if (Volume >= MaxVolume && Inhaling)
                {
                    Inhaling = false;
                    Force = 0;
                }
                if (Volume <= 0.7 && !Inhaling)
                {
                    Inhaling = true;
                    Force = 0;
                }

                Force += 0.1f;

                LinearGraph.TranslateLineH(_volume, Volume);
                LinearGraph.TranslateLineH(_inhale, Inhaling ? MaxVolume - 10 : 0);
                LinearGraph.TranslateLineH(_force, Force);
            }

            public override void Serialize(Utf8JsonWriter writer)
            {
                writer.WriteNumber("Volume", Volume);
                writer.WriteBoolean("Inhaling", Inhaling);
            }

            public override void End()
            {
                LinearGraph.Save("breathe.png");
            }
        }
        [TestMethod]
        public void Breathe()
        {
            Ticker ticker = new Ticker("breathe.json");

            ticker.RegisterSystem(new Lungs(170));
            
            while (ticker.Tick <= 1200)
            {
                ticker.Update();
            }

            ticker.End();
        }
    }

    [TestClass]
    public class Physics
    {
        public class CarSystem : SimulationSystem
        {
            public float Force = 0f;
            public float Mass = 1500f;
            public float Acceleration = 0f;
            public float Velocity = 0f;
            public float AirResistance = 0f;
            public float Friction = 0f;
            public bool Vacuum = false;
            public LinearGraphHelper LinearGraph = new LinearGraphHelper(2000, 800);
            private int _velocity;
            private int _acc;
            private int _airRes;

            public CarSystem(bool vacuum)
            {
                if (!vacuum)
                {
                    _velocity = LinearGraph.AddLine(true, SKColors.Black);
                    _acc = LinearGraph.AddLine(true, SKColors.Green);
                    _airRes = LinearGraph.AddLine(true, SKColors.Purple);
                }
                else
                {
                    _velocity = LinearGraph.AddLine(true, SKColors.Red);
                    _acc = LinearGraph.AddLine(true, SKColors.Blue);
                    _airRes = LinearGraph.AddLine(true, SKColors.Gray);
                }
                Vacuum = vacuum;
            }
            public override void End()
            {
                var vac = Vacuum ? "vacuum" : "standard";
                LinearGraph.Save("car_" + vac + ".png");
            }

            public override void Serialize(Utf8JsonWriter writer)
            {
                writer.WriteNumber("Acceleration", Acceleration);
                writer.WriteNumber("AirResistance", AirResistance);
                writer.WriteNumber("Velocity", Velocity);
                writer.WriteBoolean("Vacuum", Vacuum);
            }

            public override void Update(uint tick)
            {
                if (!Vacuum)
                {
                    Logger.Log(Velocity.ToString());
                    AirResistance = 1f / 2f * 1.2f * (Velocity * Velocity) * 0.04f;
                    Logger.Log(AirResistance.ToString());
                }
                Friction = 0.68f * Mass * 0.81f;
                Force = 1560f - AirResistance - Friction;
                Acceleration = Force / Mass;
                Velocity += Acceleration;

                LinearGraph.TranslateLineH(_velocity, Velocity);
                LinearGraph.TranslateLineH(_acc, Acceleration);
                LinearGraph.TranslateLineH(_airRes, AirResistance);
            }
        }
        [TestMethod]
        public void VacuumCar()
        {
            Ticker ticker = new Ticker("cars.json");

            ticker.RegisterSystem(new CarSystem(false));
            //ticker.RegisterSystem(new CarSystem(true));

            while (ticker.Tick < 2000)
            {
                ticker.Update();
            }

            ticker.End();
        }
    }
}
