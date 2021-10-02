using System;
using System.Collections.Generic;
using System.Text;

namespace SimulHelper
{
    public static class DataRandom
    {
        public static int Seed = 0;
        public static int Next(int min, int max)
        {
            var random = new Random(Seed);
            return random.Next(min, max);
        }
    }
}
