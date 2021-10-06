using System.Windows;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    interface IDraw
    {
        void StartDraw(Point point);
        void Draw(Point currentPosition);
        void EndDraw(Point endPoint);
        void AddPoint(Point position);
        void Show();
        void Hide();
        Shape GetShape();
    }
}
