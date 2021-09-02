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

        internal void DrawRectShadow(Point lMB_ClickPosition)
        {
            throw new NotImplementedException();
        }

        internal void DrawLineShadow(Point lMB_ClickPosition)
        {
            throw new NotImplementedException();
        }

        internal void SetSecondPoint(Point currentMousePos)
        {
            throw new NotImplementedException();
        }
    }
}
