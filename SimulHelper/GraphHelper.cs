using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace SimulHelper
{
    public class GraphHelper
    {
        protected SKImageInfo Info;
        protected SKSurface Surface;
        protected SKCanvas Canvas;
        public GraphHelper(int x, int y)
        {
            Info = new SKImageInfo(x, y);
            Surface = SKSurface.Create(Info);
            Canvas = Surface.Canvas;
            Canvas.Clear(SKColors.White);
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
