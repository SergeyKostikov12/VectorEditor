using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor
{
    public delegate void WidthPickEventHandler(object sender, WidthPickEventArgs e);
    public class WidthPickEventArgs
    {
        public int Width { get; }

        public WidthPickEventArgs(int width)
        {
            Width = width;
        }
    }
}
