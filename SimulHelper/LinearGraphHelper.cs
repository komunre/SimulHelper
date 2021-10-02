using System;
using System.Collections.Generic;
using System.Text;
using SimulHelper.Math;
using SkiaSharp;

namespace SimulHelper
{
    public class LinearGraphHelper : GraphHelper
    {
        public Dictionary<int, Line> Lines = new Dictionary<int, Line>();
        protected int Counter = 0;
        
        public LinearGraphHelper(int x, int y) : base(x ,y)
        {

        }
        public LinearGraphHelper() : base()
        {

        }
        public int AddLine(bool inverted, SKPaint paint = null)
        {
            var line = new Line();
            if (inverted)
            {
                line.Pos = new Vector2(0, Info.Height);
                line.Inverted = true;
            }
            if (paint != null)
            {
                line.Paint = paint;
            }
            var id = Counter;
            Lines.Add(id, line);
            Counter++;
            return id;
        }

        public int AddLine(bool inverted, SKColor color)
        {
            return AddLine(inverted, new SKPaint()
            {
                Color = color,
                StrokeWidth = 1f,
                Style = SKPaintStyle.Stroke,
            });
        }

        public void TranslateLineH(int id, float y)
        {
            var line = Lines[id];
            if (line == null)
                return;
            var newPos = new Vector2(line.Pos.X + 1, y);
            if (line.Inverted)
            {
                Canvas.DrawLine(new SKPoint(line.Pos.X, Info.Height - line.Pos.Y), new SKPoint(newPos.X, Info.Height - newPos.Y), line.Paint);
            }
            else
            {
                Canvas.DrawLine(new SKPoint(line.Pos.X, line.Pos.Y), new SKPoint(newPos.X, newPos.Y), line.Paint);
            }
            line.Pos = newPos;
        }
    }
}
