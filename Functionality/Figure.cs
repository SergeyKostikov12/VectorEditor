using System.Collections.Generic;
using System.Windows;
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

        public abstract void ShowOutline();
        public abstract void HideOutline(); 
        public abstract void DeselectFigure(); 
        public abstract bool SelectLine(Point point); 
        public abstract bool SelectMarker(Point poin); 
        public abstract void MoveMarker(Point position); 
        public abstract void ExecuteRelizeMarker(Point position); 
        public abstract List<Rectangle> GetMarkers(); 
        public abstract Polyline GetShape();
        public abstract Rectangle InsertPoint(Point position);

        protected abstract int GetStrokeWidth();
        protected abstract void SetStrokeWidth(int value);
        protected abstract void SetFill(SolidColorBrush brush);
        protected abstract void SetLineColor(SolidColorBrush colorBrush);
        protected abstract SolidColorBrush GetFill();
        protected abstract SolidColorBrush GetLineColor();
    }
}
