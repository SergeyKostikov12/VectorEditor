using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;



namespace GraphicEditor.Functionality
{
    public abstract class Figure
    {
        public delegate void SelectFigureEventHandler(Figure sender);
        public delegate void DeselectFigureEventHandler(Figure sender);
        public delegate void AddAdditionalElementEventHandler(Shape element);

        public abstract event SelectFigureEventHandler SelectFigure;
        public abstract event DeselectFigureEventHandler DeselectFigure;
        public abstract event AddAdditionalElementEventHandler AddAdditionalElement;

        public bool IsSelected;
        public FigureType FigureType { get; protected set; }
        public Point AnchorPoint { get; set; }
        public int StrokeWidth { get => GetStrokeWidth(); set => SetStrokeWidth(value); }
        public SolidColorBrush LineColor { get => GetLineColor(); set => SetLineColor(value); }
        public SolidColorBrush Fill { get => GetFill(); set => SetFill(value); }

        public abstract void LeftMouseButtonDown(Point position);
        public abstract void LeftMouseButtonUp(Point position);
        public abstract void RightMouseButtonDown(Point position);
        public abstract void MouseMove(Point position);
        public abstract void LeftMouseButtonClick(Point position);


        public abstract List<Shape> GetShapes(); 
        public abstract void HideOutline(); 
        public abstract void Deselect(); 
        public abstract void Collapse();


        protected abstract void ExecuteRelizeMarker(Point position); 
        protected abstract void SelectLine(Point point); 
        public abstract void ShowOutline();
        protected abstract void SelectMarker(Point poin);
        protected abstract void MoveMarker(Point position); 
        protected abstract int GetStrokeWidth();
        protected abstract void SetStrokeWidth(int value);
        protected abstract void SetFill(SolidColorBrush brush);
        protected abstract void SetLineColor(SolidColorBrush colorBrush);
        protected abstract SolidColorBrush GetFill();
        protected abstract SolidColorBrush GetLineColor();


        //public abstract bool IsMarkerSelect();
        //public abstract List<Rectangle> GetMarkers(); 
        //public abstract Rectangle InsertPoint(Point position);
    }
}
