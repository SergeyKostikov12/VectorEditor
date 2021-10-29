using GraphicEditor.Functionality.Shadows;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public abstract class Shadow
    {
        public delegate void EndDrawFigureEventHandler(Figure figure);
        public abstract event EndDrawFigureEventHandler EndDrawShadodw;

        public abstract Shape GetShape();
        public abstract Figure GetCreatedFigure();

        public abstract void LeftMouseButtonDown(Point position);
        public abstract void LeftMouseButtonUp(Point position);
        public abstract void RightMouseButtonDown(Point position);
        public abstract void MouseMove(Point position);

        public static Shadow Create(DrawingMode drawingMode)
        {
            if (drawingMode == DrawingMode.LineMode)
            {
                return new LineShadow();
            }
            else 
            {
                return new RectangleShadow();
            }
        }
    }
}
