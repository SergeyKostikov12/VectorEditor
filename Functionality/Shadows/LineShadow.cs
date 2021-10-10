using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality.Shadows
{
    public class LineShadow : ShadowFigure
    {
        public Polyline Polyline { get; set; }
        public override event EndDrawFigureEventHandler EndDrawShadodw;

        private bool isStarted;
        private bool isSingleLine;
        public LineShadow()
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
            if (!isStarted)
            {
                StartDraw(position);
            }
        }
        public override void LeftMouseButtonUp(Point position)
        {
            if (isSingleLine)
            {
                EndDraw(position);
                Hide();
            }
            else AddPoint(position);
        }
        public override void RightMouseButtonDown(Point position)
        {
            if (!isStarted) 
                return;
            EndDraw(position);
        }
        public override void MouseMove(Point position)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && isStarted)
            {
                if (Polyline.Points.Count == 1)
                {
                    isSingleLine = true;
                }
            }
            Draw(position);
        }

        public override void StartDraw(Point point)
        {
            Polyline.Points.Add(point);
            Polyline.Visibility = Visibility.Visible;
            isStarted = true;
        }
        public override void Draw(Point currentMousePos)
        {
            if (!isStarted)
                return;
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
                EndDrawShadodw?.Invoke(this);
                Reset();
            }
            else
            {
                Polyline.Points.RemoveAt(Polyline.Points.Count - 1);
                EndDrawShadodw?.Invoke(this);
                Reset();
            }
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

        private void Reset()
        {
            Polyline.Points.Clear();
            isStarted = false;
            isSingleLine = false;
            Hide();
        }
    }
}
