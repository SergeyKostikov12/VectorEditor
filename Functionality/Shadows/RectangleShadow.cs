using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality.Shadows
{
    public class RectangleShadow : ShadowFigure
    {
        private Rectangle rectangle;
        private bool isDrawing;

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
            Show();
            Draw(position);
        }
        public override void StartDraw(Point point)
        {
            FirstPoint = point;
        }
        public override void Draw(Point currentMousePos)
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
        public override void EndDraw(Point endPoint)
        {
            LastPoint = endPoint;
            EndDrawShadodw?.Invoke(this);
            return;
        }
        public override void AddPoint(Point position)
        {
            return;
        }
        public override Shape GetShape()
        {
            return rectangle;
        }
        public override void Show()
        {
            if (isDrawing)
            {
                rectangle.Visibility = Visibility.Visible;
            }
        }
        public override void Hide()
        {
            rectangle.Visibility = Visibility.Hidden;
        }

    }
}
