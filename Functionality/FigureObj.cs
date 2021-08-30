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
        public int StrokeWidth { get => GetStrokeWidth(); set => SetStrokeWidth(value); }
        public SolidColorBrush LineColor { get; }


        public abstract void MoveFigure(Point position);
        public abstract void ShowOutline();
        public abstract void HideOutline();
        public abstract void PlacingInWorkPlace(Canvas canvas);
        protected abstract int GetStrokeWidth();
        protected abstract void SetStrokeWidth(int value);
    }
}
