using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality.Shadows
{
    public class RectangleShadow : Shadow
    {
        private Shape rectangle;
        private bool isDrawing;
        private int request;


        public Point FirstPoint { get; set; }
        public Point LastPoint { get; set; }

        public override event EndDrawFigureEventHandler EndDrawShadodw;

        public RectangleShadow()
        {
            rectangle = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };
        }

        public override void LeftMouseButtonDown(Point position)
        {
            if (!isDrawing)
            {
                isDrawing = true;
            }
            StartDraw(position);
        }
        public override void LeftMouseButtonUp(Point position)
        {
            isDrawing = false;
            EndDraw(position);
            Hide();
        }
        public override void RightMouseButtonDown(Point position)
        {
            isDrawing = false;
            Hide();
        }
        public override void MouseMove(Point position)
        {
            if (!isDrawing)
                return;
            Draw(position);
        }
        public override Shape GetShape()
        {
            request++;
            if (request == 3)
                ConvertToShape();

            return rectangle;
        }

        public void StartDraw(Point point)
        {
            FirstPoint = point;
            Show();
        }
        public void Draw(Point currentMousePos)
        {
            double xTop = Math.Max(FirstPoint.X, currentMousePos.X);
            double yTop = Math.Max(FirstPoint.Y, currentMousePos.Y);

            double xMin = Math.Min(FirstPoint.X, currentMousePos.X);
            double yMin = Math.Min(FirstPoint.Y, currentMousePos.Y);

            rectangle.Height = yTop - yMin;
            rectangle.Width = xTop - xMin;

            Canvas.SetLeft(rectangle, xMin);
            Canvas.SetTop(rectangle, yMin);
        }
        public void EndDraw(Point endPoint)
        {
            LastPoint = endPoint;
            EndDrawShadodw?.Invoke(this);
            return;
        }
        public void Show()
        {
            if (isDrawing)
            {
                rectangle.Visibility = Visibility.Visible;
            }
        }
        public void Hide()
        {
            rectangle.Visibility = Visibility.Hidden;
        }

        private void ConvertToShape()
        {
            double xTop = Math.Max(FirstPoint.X, LastPoint.X);
            double yTop = Math.Max(FirstPoint.Y, LastPoint.Y);
            double xMin = Math.Min(FirstPoint.X, LastPoint.X);
            double yMin = Math.Min(FirstPoint.Y, LastPoint.Y);

            Point AnchorPoint = new Point(xMin, yMin);
            Point point1 = new Point(xMin, yTop);
            Point point2 = new Point(xTop, yTop);
            Point point3 = new Point(xTop, yMin);

            Polyline line = new Polyline();
            line.Points.Add(AnchorPoint);
            line.Points.Add(point1);
            line.Points.Add(point2);
            line.Points.Add(point3);
            line.Points.Add(AnchorPoint);

            rectangle = line;
        }
    }
}
