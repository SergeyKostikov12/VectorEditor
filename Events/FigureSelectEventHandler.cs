using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor.Events
{
    public delegate void FigureSelectEventHandler(Figure sender, FigureSelectEventArgs e);
    public class FigureSelectEventArgs
    {
        public int Width { get; }
        public FigureSelectEventArgs(int width)
        {
            Width = width;
        }
    }
}
