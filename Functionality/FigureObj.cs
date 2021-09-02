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
        public SolidColorBrush Fill { get => GetFill(); set => SetFill(value); }
        public SolidColorBrush LineColor { get; set; }


        public abstract void MoveFigure(Point position);
        public abstract void ShowOutline();
        public abstract void HideOutline();
        public abstract void PlacingInWorkPlace(Canvas canvas);
        
        public virtual void AddPoint(Point point)
        {
            throw new NotImplementedException();
        }

        protected abstract int GetStrokeWidth();
        protected abstract void SetStrokeWidth(int value);
        protected abstract SolidColorBrush GetFill();
        protected abstract void SetFill(SolidColorBrush brush);

        public virtual void DeletePolyline()
        {
            throw new NotImplementedException();
        }

    }
}
