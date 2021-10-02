using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulHelper;
using SimulHelper.Math;
using SkiaSharp;
using System;
using System.IO;

namespace SimulationExample
{
    public class TemperatureSystem : SimulationSystem
    {
        public float Temperature = 23;

        public override void End()
        {
            
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
    }
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IceMelt()
        {
            Ticker ticker = new Ticker();
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
    }
}
