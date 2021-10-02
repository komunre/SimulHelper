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
}