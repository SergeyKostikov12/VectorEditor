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
    enum ShapeType { Rectangle, Line}
    class FigureObject
    {
        private string name; //--
        private Point pivotPoint; //--
        private Size size; //--
        private Point centerPoint; //--
        private Point rotatePoint; //--
        private Polyline polyline; //--
        private Rectangle outline;
        private Brush fill; //--

        public ShapeType ShapeType;

        public string Name { get => name; set => name = value; }
        public Point FirstPoint { get => pivotPoint; }
        public Point CenterPoint { get => centerPoint; }
        public Polyline Shape { get => polyline; }
        public Rectangle Outline { get => outline; }
        public Size Size { get => size; }
        public Brush Fill { get => fill; }
        public Point RotatePoint { get => rotatePoint; }

        public FigureObject(string _name, ShapeType _shapeType, Point _firstPoint, Point _secondPoint, Canvas _workPlace)
        {
            name = _name;
            ShapeType = _shapeType;
            SetNewShape(_shapeType, _firstPoint, _secondPoint);
            DefineGizmoPoints();
            DefineOutline();
            polyline.StrokeThickness = 2;
            _workPlace.Children.Add(polyline);
            polyline.Stroke = Brushes.Black;
            polyline.Visibility = Visibility.Visible;
        }
        public FigureObject()
        {

        }

        private void DefineOutline()
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                Rectangle rect = new Rectangle();
                rect.Height = polyline.Height + 10;
                rect.Width = polyline.Width + 10;
                rect.StrokeThickness = 1;
                rect.Stroke = Brushes.Red;
                rect.StrokeDashArray = new DoubleCollection() { 5, 5 };
                outline = rect;
            }
        }
        private void SetNewShape(ShapeType shapeType, Point firstPoint, Point secondPoint)
        {
            if (shapeType == ShapeType.Rectangle)
            {
                pivotPoint.X = Math.Min(firstPoint.X, secondPoint.X);
                pivotPoint.Y = Math.Min(firstPoint.Y, secondPoint.Y);

                size.Height = Math.Max(firstPoint.X, secondPoint.X) - pivotPoint.X;
                size.Width = Math.Max(firstPoint.Y, secondPoint.Y) - pivotPoint.Y;

                Point point1 = new Point(pivotPoint.X, pivotPoint.Y + size.Width);
                Point point2 = new Point(point1.X + size.Height, point1.Y);
                Point point3 = new Point(point2.X, point2.Y - size.Width);
                Polyline line = new Polyline();
                line.Name = name;
                line.Points.Add(pivotPoint);
                line.Points.Add(point1);
                line.Points.Add(point2);
                line.Points.Add(point3);
                line.Points.Add(pivotPoint);
                polyline = line;
                Canvas.SetTop(polyline, pivotPoint.X);
                Canvas.SetLeft(polyline, pivotPoint.Y);
            }
            else
            {
                Polyline line = new Polyline();
                line.Points.Add(firstPoint);
                line.Points.Add(secondPoint);
                polyline = line;
            }
        }
        private void DefineGizmoPoints()
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                centerPoint = new Point(pivotPoint.X + polyline.Height / 2, pivotPoint.Y + polyline.Width / 2);
                rotatePoint = new Point(centerPoint.X + 50, centerPoint.Y);
            }
        }
        public void RotateFigure(Point newPosition)
        {
            Vector v0 = new Vector(pivotPoint.X - rotatePoint.X, pivotPoint.Y - rotatePoint.Y);
            Vector v1 = GetVector(newPosition);
            double angle = (Vector.AngleBetween(v0, v1))*Math.PI/180;

           for (int i = 0; i<polyline.Points.Count; i++)
            {
                float X =(float)((polyline.Points[i].X - centerPoint.X) * Math.Cos(angle) - (polyline.Points[i].Y - centerPoint.Y) * Math.Sin(angle) + centerPoint.X);
                float Y = (float)((polyline.Points[i].X - centerPoint.X) * Math.Sin(angle) + (polyline.Points[i].Y - centerPoint.Y) * Math.Cos(angle) + centerPoint.Y);
                polyline.Points[i] = new Point(X, Y);
            }
        }
        private Vector GetVector(Point newPos)
        {
            Vector vector = new Vector(pivotPoint.X - newPos.X, pivotPoint.Y - newPos.Y);
            return vector;
        }
        public void MoveFigure(Point newPosition)
        {
            Canvas.SetTop(polyline, newPosition.X);
            Canvas.SetLeft(polyline, newPosition.Y);
            pivotPoint = newPosition;
        }
        public void FillRect(Brush brush)
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                fill = brush;
                polyline.Fill = fill;
            }
            else return;
        }
        public void AddLine(Point point)
        {
            if (ShapeType == ShapeType.Line)
            {
                polyline.Points.Add(point);
            }
            else return;
        }
        public void SetLineThickness(int value)
        {
            polyline.StrokeThickness = value;
        }
        public void RemoveShape(string name) //Надо будет проверить!!!!!!!!!!!
        {
            var par = (Canvas)polyline.Parent;
            par.Children.Remove(polyline);
            par.Children.Remove(outline);
            par.UpdateLayout();
        }
    }
}
