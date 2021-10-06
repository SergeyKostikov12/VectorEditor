using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public abstract class Drawer
    {
        private Canvas Workplace;
        public Drawer(Canvas workplace)
        {
            Workplace = workplace;
        }
    }



    public class ShadowRectangle : IDraw
    {
        private Point firstPoint;
        private Rectangle rectangle;
        public ShadowRectangle()
        {
            rectangle = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
            };
        }
        public void StartDraw(Point point)
        {
            firstPoint = point;
        }

        public void Draw(Point currentMousePos)
        {
            double xTop = Math.Max(firstPoint.X, currentMousePos.X);
            double yTop = Math.Max(firstPoint.Y, currentMousePos.Y);

            double xMin = Math.Min(firstPoint.X, currentMousePos.X);
            double yMin = Math.Min(firstPoint.Y, currentMousePos.Y);

            rectangle.Height = yTop - yMin;
            rectangle.Width = xTop - xMin;

            Canvas.SetLeft(rectangle, xMin);
            Canvas.SetTop(rectangle, yMin);
        }

        public void EndDraw(Point endPoint)
        {
        }

        public void AddPoint(Point position)
        {
        }

        public Shape GetShape()
        {
            return rectangle;
        }

        public void Show()
        {
            rectangle.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            rectangle.Visibility = Visibility.Hidden;
        }
    }
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

    interface IDraw
    {
        void StartDraw(Point point);
        void Draw(Point currentPosition);
        void EndDraw(Point endPoint);
        void AddPoint(Point position);
        void Show();
        void Hide();
        Shape GetShape();
    }
}
