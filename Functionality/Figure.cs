using GraphicEditor.Events;
using GraphicEditor.Functionality.Shadows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;



namespace GraphicEditor.Functionality
{
    public abstract class Figure
    {
        public delegate void AddAdditionalElementEventHandler(Shape element);

        public abstract event FigureSelectEventHandler SelectFigure;
        public abstract event FigureDeselectEventHandler DeselectFigure;
        public abstract event AddAdditionalElementEventHandler AddAdditionalElement;

        public FigureType FigureType { get; protected set; }
        public int StrokeWidth { get => GetStrokeWidth(); set => SetStrokeWidth(value); }
        public SolidColorBrush LineColor { get => GetLineColor(); set => SetLineColor(value); }
        public SolidColorBrush Fill { get => GetFill(); set => SetFill(value); }

        protected Point AnchorPoint { get; set; }
        protected bool isSelected;

        public abstract void LeftMouseButtonDown(Point position);
        public abstract void LeftMouseButtonUp(Point position);
        public abstract void RightMouseButtonDown(Point position);
        public abstract void MouseMove(Point position);
        public abstract void LeftMouseButtonClick(Point position);


        public abstract List<Shape> GetShapes(); 
        public abstract void HideOutline(); 
        public abstract void ShowOutline();
        public abstract void Deselect(); 
        public abstract void Collapse();

        public static Figure Create(SerializableFigure serializable)
        {
            var type = (FigureType)serializable.FigureTypeNumber;

            if (type == FigureType.Rectangle)
            {
                return new RectangleFigure(serializable);
            }
            else 
            {
                return new LineFigure(serializable);
            }
        }
        public static Figure Create(Shadow shadow)
        {
            if (shadow is RectangleShadow)
            {
                return new RectangleFigure(shadow.GetShape());
            }
            else
            {
                return new LineFigure(shadow.GetShape());
            }
        }


        protected abstract void ExecuteRelizeMarker(Point position); 
        protected abstract void SelectLine(Point point); 
        protected abstract void SelectMarker(Point poin);
        protected abstract void MoveMarker(Point position); 
        protected abstract int GetStrokeWidth();
        protected abstract void SetStrokeWidth(int value);
        protected abstract void SetFill(SolidColorBrush brush);
        protected abstract void SetLineColor(SolidColorBrush colorBrush);
        protected abstract SolidColorBrush GetFill();
        protected abstract SolidColorBrush GetLineColor();
    }
}
