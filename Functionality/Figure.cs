using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;



namespace GraphicEditor.Functionality
{
    public abstract class Figure
    {
        public FigureType FigureType { get; protected set; } 
        public Point AnchorPoint { get; set; }
        public int StrokeWidth { get => GetStrokeWidth(); set => SetStrokeWidth(value); }
        public SolidColorBrush LineColor { get => GetLineColor(); set => SetLineColor(value); }
        public SolidColorBrush Fill { get => GetFill(); set => SetFill(value); }



        protected abstract int GetStrokeWidth();
        protected abstract void SetStrokeWidth(int value);
        protected abstract SolidColorBrush GetFill();
        protected abstract void SetFill(SolidColorBrush brush);
        protected abstract SolidColorBrush GetLineColor();
        protected abstract void SetLineColor(SolidColorBrush colorBrush);
        public abstract void ShowOutline();
        public abstract void HideOutline(); 
        public abstract void MoveMarker(Point position); 
        public abstract void ExecuteRelize(Point position); 
        public abstract bool SelectMarker(Point poin); 
        public abstract bool SelectLine(Point point); 
        public abstract void DeselectFigure(); 
        public abstract List<Rectangle> GetMarkers(); 
        public abstract Polyline GetShape();
        public abstract Rectangle ExecuteDoubleClick(Point position);



    }
}
