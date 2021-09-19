using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public class WorkplaceShadow
    {
        private Rectangle shadowRect;
        private Polyline shadowLine;
        private Canvas workplace;
        private Point firstPoint;

        public WorkplaceShadow(Canvas _workplace)
        {
            workplace = _workplace;
            InitializeShadows();
        }

        private void InitializeShadows()
        {
            Rectangle rectangle = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };
            shadowRect = rectangle;
            Polyline line = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };

            shadowLine = line;
            workplace.Children.Add(shadowRect);
            workplace.Children.Add(shadowLine);
        }
        internal void StartDrawRectShadow(Point LMB_ClickPosition)
        {
            firstPoint = LMB_ClickPosition;
            shadowRect.Visibility = Visibility.Visible;
        }
        internal void DrawLineShadow()
        {
            shadowLine.Visibility = Visibility.Visible;
        }
        internal void SetLIneFirstPoint(Point _firstPoint)
        {
            firstPoint = _firstPoint;
            shadowLine.Points.Add(firstPoint);
            shadowLine.Points[0] = firstPoint;
            shadowLine.UpdateLayout();
        }
        internal void SetLineSecondPoint(Point point)
        {
            if (shadowLine.Points.Count < 2)
            {
                shadowLine.Points.Add(point);
            }
        }
        internal void DrawRectShadow(Point currentMousePos) 
        {
            double xTop = Math.Max(firstPoint.X, currentMousePos.X);
            double yTop = Math.Max(firstPoint.Y, currentMousePos.Y);

            double xMin = Math.Min(firstPoint.X, currentMousePos.X);
            double yMin = Math.Min(firstPoint.Y, currentMousePos.Y);

            shadowRect.Height = yTop - yMin;
            shadowRect.Width = xTop - xMin;

            Canvas.SetLeft(shadowRect, xMin);
            Canvas.SetTop(shadowRect, yMin);
        }
        internal void DrawLastPointShadowtLine(Point currentMousePos)
        {
            int n = shadowLine.Points.Count - 1;
            shadowLine.Points[n] = new Point(currentMousePos.X, currentMousePos.Y);
        }
        internal void AddPoint(Point clickPosition)
        {
            shadowLine.Points.Add(clickPosition);
        }
        internal void Clear()
        {
            shadowLine.Points.Clear();
            shadowLine.Visibility = Visibility.Hidden;
            shadowRect.Visibility = Visibility.Hidden;
        }
        internal void RemoveLastPoint()
        {
            shadowLine.Points.RemoveAt(shadowLine.Points.Count - 1);
        }
        public Polyline GetShadowLine()
        {
            return shadowLine;
        }
    }
}
