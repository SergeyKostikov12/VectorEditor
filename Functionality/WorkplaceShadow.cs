using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public class WorkplaceShadow
    {
        public enum LineTypes { Line, Polyline }

        public LineTypes LineType;
        private Rectangle shadowRect;
        private Polyline shadowLine;
        private Canvas workplace;
        private Point firstPoint;
        public bool FirstPointIsDefined
        {
            get
            {
                if (ShadowLine.Points.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Point FirstPoint { get => firstPoint; set => firstPoint = value; }


        public Rectangle ShadowRect { get => shadowRect; set => shadowRect = value; }
        public Polyline ShadowLine { get => shadowLine; set => shadowLine = value; }

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
            ShadowRect = rectangle;
            Polyline line = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };

            ShadowLine = line;
            //ShadowLine.Points.Add(new Point(0, 0));

            workplace.Children.Add(ShadowRect);
            workplace.Children.Add(ShadowLine);
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
            FirstPoint = _firstPoint;
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
        internal void DrawLineSecondPoint(Point currentMousePos)
        {
            shadowLine.Points[1] = currentMousePos;
        }

        internal void DrawRectShadow(Point currentMousePos) //TODO: 
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
    }
}
