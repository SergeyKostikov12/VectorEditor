using System.Windows;

namespace GraphicEditor.Functionality
{
    public interface IDrawing
    {
        void StartDraw(Point point);
        void Draw(Point currentPosition);
        void EndDraw(Point endPoint);
        void Show();
        void Hide();

    }
}