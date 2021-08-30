using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public abstract class FigureObj
    {
        public FigureType FigureType { get; } 
        public Point AnchorPoint { get; set; }
        public int StrokeWidth { get; }
        public SolidColorBrush LineColor { get; }


        public abstract void MoveFigure(Point position);
    }
}
