using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality.Shadows
{
    public class ShadowLine : IDraw
    {
        private Polyline polyline;
        public ShadowLine()
        {
            polyline = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };
        }
        public void StartDraw(Point point)
        {
            polyline.Points.Add(point);
            polyline.Visibility = Visibility.Visible;
        }

        public void Draw(Point currentMousePos)
        {
            if (polyline.Points.Count == 1)
            {
                polyline.Points.Add(currentMousePos);
            }
            int n = polyline.Points.Count - 1;
            polyline.Points[n] = new Point(currentMousePos.X, currentMousePos.Y);
        }

        public void EndDraw(Point endPoint)
        {
            if (polyline.Points.Count <= 2)
            {
                polyline.Points[polyline.Points.Count - 1] = new Point(endPoint.X, endPoint.Y);
                return;
            }
            polyline.Points.RemoveAt(polyline.Points.Count - 1);
        }

        public void AddPoint(Point clickPosition)
        {
            polyline.Points[polyline.Points.Count - 1] = clickPosition;
            polyline.Points.Add(clickPosition);
        }

        public Shape GetShape()
        {
            return polyline;
        }

        public void Show()
        {
            polyline.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            polyline.Visibility = Visibility.Hidden;
        }
    }
}
