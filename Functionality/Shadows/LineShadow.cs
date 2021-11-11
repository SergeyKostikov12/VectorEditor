using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality.Shadows
{
    public class LineShadow : Shadow
    {
        public override event EndDrawFigureEventHandler EndDrawShadodw;

        private Polyline Polyline { get; set; }
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
        public override Shape GetShape()
        {
            return Polyline;
        }
        //public override Figure GetCreatedFigure()
        //{
        //    Figure figure = new LineFigure(Polyline);
        //    return figure;
        //}

        public void StartDraw(Point point)
        {
            Polyline.Points.Add(point);
            isStarted = true;
            Show();
        }
        public void Draw(Point currentMousePos)
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
        public void EndDraw(Point endPoint)
        {
            if (Polyline.Points.Count <= 2)
            {
                Polyline.Points[Polyline.Points.Count - 1] = new Point(endPoint.X, endPoint.Y);
                EndDrawShadodw?.Invoke(this);
                //EndDrawShadodw?.Invoke(GetCreatedFigure());
                Reset();
            }
            else
            {
                Polyline.Points.RemoveAt(Polyline.Points.Count - 1);
                EndDrawShadodw?.Invoke(this);
                //EndDrawShadodw?.Invoke(GetCreatedFigure());
                Reset();
            }
        }
        public void Show()
        {
            Polyline.Visibility = Visibility.Visible;
        }
        public void Hide()
        {
            Polyline.Visibility = Visibility.Hidden;
        }

        private void AddPoint(Point clickPosition)
        {
            Polyline.Points[Polyline.Points.Count - 1] = clickPosition;
            Polyline.Points.Add(clickPosition);
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
