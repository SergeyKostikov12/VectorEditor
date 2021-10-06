using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality.Shadows
{
    public class ShadowRectangle : IDraw
    {
        private Point firstPoint;
        private Rectangle rectangle;
        public ShadowRectangle()
        {
            rectangle = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
            };
        }
        public void StartDraw(Point point)
        {
            firstPoint = point;
        }

        public void Draw(Point currentMousePos)
        {
            double xTop = Math.Max(firstPoint.X, currentMousePos.X);
            double yTop = Math.Max(firstPoint.Y, currentMousePos.Y);

            double xMin = Math.Min(firstPoint.X, currentMousePos.X);
            double yMin = Math.Min(firstPoint.Y, currentMousePos.Y);

            rectangle.Height = yTop - yMin;
            rectangle.Width = xTop - xMin;

            Canvas.SetLeft(rectangle, xMin);
            Canvas.SetTop(rectangle, yMin);
        }

        public void EndDraw(Point endPoint)
        {
            return;
        }

        public void AddPoint(Point position)
        {
            return;
        }

        public Shape GetShape()
        {
            return rectangle;
        }

        public void Show()
        {
            rectangle.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            rectangle.Visibility = Visibility.Hidden;
        }
    }
}
