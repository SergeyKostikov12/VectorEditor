using GraphicEditor.Functionality.Shadows;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public abstract class ShadowFigure
    {
        public delegate void EndDrawFigureEventHandler(object sender);

        public abstract event EndDrawFigureEventHandler EndDrawFigure;
        public abstract void StartDraw(Point point);
        public abstract void Draw(Point currentPosition);
        public abstract void EndDraw(Point endPoint);
        public abstract void AddPoint(Point position);
        public abstract void Show();
        public abstract void Hide();
        public abstract Shape GetShape();
        public abstract void LeftMouseButtonDown(Point position);
        public abstract void LeftMouseButtonUp(Point position);
        public abstract void RightMouseButtonDown(Point position);
        public abstract void MouseMove(Point position);
    }
}
