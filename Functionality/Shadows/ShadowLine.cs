using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality.Shadows
{
    public class ShadowLine : ShadowFigure
    {
        public Polyline Polyline { get ; set ; }

        public override event EndDrawFigureEventHandler EndDrawFigure;

        public ShadowLine()
        {
            Polyline = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };
        }

        public override void LeftMouseButtonDown(Point position)
        {
            throw new System.NotImplementedException();
        }
        public override void LeftMouseButtonUp(Point position)
        {
            throw new System.NotImplementedException();
        }
        public override void RightMouseButtonDown(Point position)
        {
            throw new System.NotImplementedException();
        }
        public override void MouseMove(Point position)
        {
            throw new System.NotImplementedException();
        }
        public override void StartDraw(Point point)
        {
            Polyline.Points.Add(point);
            Polyline.Visibility = Visibility.Visible;
        }
        public override void Draw(Point currentMousePos)
        {
            if (Polyline.Points.Count == 1)
            {
                Polyline.Points.Add(currentMousePos);
            }
            int n = Polyline.Points.Count - 1;
            Polyline.Points[n] = new Point(currentMousePos.X, currentMousePos.Y);
        }
        public override void EndDraw(Point endPoint)
        {
            if (Polyline.Points.Count <= 2)
            {
                Polyline.Points[Polyline.Points.Count - 1] = new Point(endPoint.X, endPoint.Y);
                return;
            }
            Polyline.Points.RemoveAt(Polyline.Points.Count - 1);
            EndDrawFigure?.Invoke(this);
        }
        public override void AddPoint(Point clickPosition)
        {
            Polyline.Points[Polyline.Points.Count - 1] = clickPosition;
            Polyline.Points.Add(clickPosition);
        }
        public override Shape GetShape()
        {
            return Polyline;
        }
        public override void Show()
        {
            Polyline.Visibility = Visibility.Visible;
        }
        public override void Hide()
        {
            Polyline.Visibility = Visibility.Hidden;
        }

    }
}
