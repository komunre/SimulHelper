using System;
using System.Collections.Generic;
using System.Text;

namespace SimulHelper.Math
{
    public class Vector2
    {
        public float X;
        public float Y;
        public static Vector2 Zero => new Vector2(0, 0);

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
