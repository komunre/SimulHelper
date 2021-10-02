using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SimulHelper.Math;

namespace SimulHelper
{
    public class GraphHelper
    {
        protected SKImageInfo Info;
        protected SKSurface Surface;
        protected SKCanvas Canvas;
        public Vector2[] GraphBox { get; protected set; }
        public GraphHelper(int x, int y)
        {
            Info = new SKImageInfo(x, y);
            Surface = SKSurface.Create(Info);
            Canvas = Surface.Canvas;
            Canvas.Clear(SKColors.White);
            var paint = new SKPaint()
            {
                Color = SKColors.Black,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1f,
            };
            Canvas.DrawLine(new SKPoint(5, 5), new SKPoint(5, Info.Height - 5), paint);
            Canvas.DrawLine(new SKPoint(5, Info.Height - 5), new SKPoint(Info.Width - 5, Info.Height - 5), paint);
            GraphBox = new Vector2[2] { new Vector2(5, 5), new Vector2(Info.Width, Info.Height - 5) };
        }

        public GraphHelper() : this(10000, 10000)
        {
            
        }

        public void Save(string name)
        {
            var image = Surface.Snapshot();
            var data = image.Encode(SKEncodedImageFormat.Png, 99);
            data.SaveTo(File.OpenWrite(name));
        }
    }
}
