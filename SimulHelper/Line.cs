using System;
using System.Collections.Generic;
using System.Text;
using SimulHelper.Math;
using SkiaSharp;

namespace SimulHelper
{
    public class Line
    {
        public Vector2 Pos = Vector2.Zero;
        public bool Inverted = false;
        public SKPaint Paint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 1f,
            Style = SKPaintStyle.Stroke,
        };
    }
}
