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
        public enum LineTypes { Line, Polyline}

        public LineTypes LineType;
        private Rectangle shadowRect;
        private Polyline shadowLine;
        private Canvas workplace;
        private Point firstPoint;
        private bool isFirstPoint = false;

        public Point FirstPoint
        {
            get => firstPoint;
            set
            {
                firstPoint = value;
                isFirstPoint = true;
            }
        }


        public Rectangle ShadowRect { get => shadowRect; set => shadowRect = value; }
        public Polyline ShadowLine { get => shadowLine; set => shadowLine = value; }

        public WorkplaceShadow(Canvas _workplace)
        {
            InitializeShadows();
            workplace = _workplace;
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
            ShadowLine.Points.Add(new Point(0, 0));

            workplace.Children.Add(ShadowRect);
            workplace.Children.Add(ShadowLine);
        }

        internal void StartDrawRectShadow(Point lMB_ClickPosition)
        {
            throw new NotImplementedException();
        }

        internal void DrawLineShadow(Point lMB_ClickPosition)
        {
            throw new NotImplementedException();
        }

        internal void SetLineSecondPoint(Point currentMousePos)
        {
            throw new NotImplementedException();
        }

        internal void DrawRectShadow(Point currentMousePos) //TODO: 
        {
            double xTop = Math.Max(LMB_ClickPosition.X, currentMousePos.X);
            double yTop = Math.Max(LMB_ClickPosition.Y, currentMousePos.Y);

            double xMin = Math.Min(LMB_ClickPosition.X, currentMousePos.X);
            double yMin = Math.Min(LMB_ClickPosition.Y, currentMousePos.Y);

            shadowRect.Height = yTop - yMin;
            shadowRect.Width = xTop - xMin;

            Canvas.SetLeft(shadowRect, xMin);
            Canvas.SetTop(shadowRect, yMin);
        }

        internal void DrawLastShadowtLine(Point currentMousePos)
        {
            int n = shadowLine.Points.Count - 1;
            shadowLine.Points[n] = new Point(currentMousePos.X, currentMousePos.Y);
        }

        internal void AddPoint(Point clickPosition)
        {
            throw new NotImplementedException();
        }
    }
}
