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
    enum ShapeType { Rectangle, Line }
    class FigureObject
    {
        private string name; //--
        private Point pivotPoint; //--
        private Point maxPoint;
        private Size size; //--
        private Point centerPoint; //--
        private Rectangle centerGizmo; //---
        private List <Rectangle> gizmos = new List<Rectangle>();
        private Point rotatePoint; //--
        private Polyline polyline; //--
        private Rectangle outline;
        private Point outlinePivotPoint;
        private bool isOutlineVisible = false;
        private Brush fill; //--
        private int selectedPoint;
        private Canvas canvas;

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
            canvas = _workPlace;
            SetNewShape(_shapeType, _firstPoint, _secondPoint, _workPlace);
            DefineGizmoPoints(_workPlace);
            DefineOutline(_workPlace);
            polyline.StrokeThickness = 2;
            _workPlace.Children.Add(polyline);
            polyline.Stroke = Brushes.Black;
            polyline.Visibility = Visibility.Visible;
        }
        public FigureObject(string _name, ShapeType _shapeType, Polyline _polyline, Canvas _workPlace)
        {
            name = _name;
            ShapeType = _shapeType;
            canvas = _workPlace;
            SetNewShape(_shapeType, _polyline);
            DefineGizmoPoints(_workPlace);
            DefineOutline(_workPlace);
            polyline.StrokeThickness = 2;
            _workPlace.Children.Add(polyline);
            polyline.Stroke = Brushes.Black;
            polyline.Visibility = Visibility.Visible;
        }

        public void DrawCenterGizmo(bool value)
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                if (value)
                {
                    centerGizmo.Visibility = Visibility.Visible;
                }
                else centerGizmo.Visibility = Visibility.Hidden;
            }
        }
        public Point MoveFigure(Point firstPoint, Point newPosition)
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                Point deltaPoint = new Point(newPosition.X - firstPoint.X, newPosition.Y - firstPoint.Y);

                Canvas.SetLeft(outline, outlinePivotPoint.X + deltaPoint.X);
                Canvas.SetTop(outline, outlinePivotPoint.Y + deltaPoint.Y);
                Canvas.SetLeft(centerGizmo, centerPoint.X - 5 + deltaPoint.X);
                Canvas.SetTop(centerGizmo, centerPoint.Y - 5 + deltaPoint.Y);
                centerPoint = NewPointPosition(centerPoint, deltaPoint);
                pivotPoint = NewPointPosition(pivotPoint, deltaPoint);
                outlinePivotPoint = NewPointPosition(outlinePivotPoint, deltaPoint);

                for (int i = 0; i < polyline.Points.Count; i++)
                {
                    polyline.Points[i] = NewPointPosition(polyline.Points[i], deltaPoint);
                }
                polyline.UpdateLayout();
                return newPosition;
            }
            else return firstPoint;

        }
        public Point RotateFigure(Point firstPoint, Point newPoint)
        {
            Vector CA = new Vector(centerPoint.X - firstPoint.X, centerPoint.Y - firstPoint.Y);
            Vector CB = new Vector(centerPoint.X - newPoint.X, centerPoint.Y - newPoint.Y);
            double angle = Vector.AngleBetween(CA, CB) * Math.PI / 180;

            for (int i = 0; i < polyline.Points.Count; i++)
            {
                float X = (float)((polyline.Points[i].X - centerPoint.X) * Math.Cos(angle) - (polyline.Points[i].Y - centerPoint.Y) * Math.Sin(angle) + centerPoint.X);
                float Y = (float)((polyline.Points[i].X - centerPoint.X) * Math.Sin(angle) + (polyline.Points[i].Y - centerPoint.Y) * Math.Cos(angle) + centerPoint.Y);
                polyline.Points[i] = new Point(X, Y);
            }
            RedrawOutLine();
            return newPoint;
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
        public void DrawOutline(bool value, Canvas canvas)
        {
            isOutlineVisible = value;

            if (ShapeType == ShapeType.Line)
            {
                DrawLineGizmos(value, canvas);
            }
            else
            {
                if (value) outline.Visibility = Visibility.Visible;
                else outline.Visibility = Visibility.Hidden;
            }
        }
        public Point GetPointNear(Point position)
        {
            for (int i = 0; i<polyline.Points.Count; i++)
            {
                Point delta = new Point(Math.Abs(position.X - polyline.Points[i].X) , Math.Abs(position.Y - polyline.Points[i].Y));
                if (delta.X <= 5 && delta.Y <= 5)
                {
                    selectedPoint = i;
                    return polyline.Points[i];
                }
                else continue;
            }
            return new Point(9999,0);
        }
        public Point MovePointToNewPosition(Point fistPoint, Point position)
        {
            Point deltaPoint = new Point(position.X - fistPoint.X, position.Y - fistPoint.Y);
            polyline.Points[selectedPoint] = NewPointPosition(polyline.Points[selectedPoint], deltaPoint);
            Rectangle rect = gizmos[selectedPoint];
            Point currentGizmoPos = GetGizmoPosition(rect);
            Point newPos = NewPointPosition(currentGizmoPos, deltaPoint);
            SetGizmoPointPosition(rect, newPos, true);
            return position;
        }

        private Point GetGizmoPosition(Rectangle gizmo)
        {
            Point point = new Point();
            point.X = Canvas.GetLeft(gizmo);
            point.Y = Canvas.GetTop(gizmo);
            return point;
        }
        private void DrawLineGizmos(bool value, Canvas canvas)
        {
            if (value)
            {
               foreach(var item in canvas.Children)
                {
                    try
                    {
                        Rectangle rect = (Rectangle)item;
                        if(rect.Name == GetRectByName(rect.Name))
                        {
                            rect.Visibility = Visibility.Visible;
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                foreach (var item in canvas.Children)
                {
                    try
                    {
                        Rectangle rect = (Rectangle)item;
                        if (rect.Name == GetRectByName(rect.Name))
                        {
                            rect.Visibility = Visibility.Hidden;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        private string GetRectByName(string name)
        { 
            foreach(var gizmo in gizmos)
            {
                if (gizmo.Name == name) return name;
            }
            return "None";
        }
        private void SetNewShape(ShapeType shapeType, Polyline _polyline)
        {
            polyline = new Polyline();

            double xTop = double.MinValue;
            double yTop = double.MinValue;
            double xMin = double.MaxValue;
            double yMin = double.MaxValue;

            for (int i = 0; i < _polyline.Points.Count; i++)
            {
                polyline.Points.Add(_polyline.Points[i]);
                if (_polyline.Points[i].X > xTop) xTop = _polyline.Points[i].X;
                if (_polyline.Points[i].Y > yTop) yTop = _polyline.Points[i].Y;
                if (_polyline.Points[i].X < xMin) xMin = _polyline.Points[i].X;
                if (_polyline.Points[i].Y < yMin) yMin = _polyline.Points[i].Y;
            }
            pivotPoint = new Point(xMin, yMin);
            size = new Size(yTop - yMin, xTop - xMin);

        }
        private void SetNewShape(ShapeType shapeType, Point firstPoint, Point secondPoint, Canvas workspace)
        {
            double xTop = Math.Max(firstPoint.X, secondPoint.X);
            double yTop = Math.Max(firstPoint.Y, secondPoint.Y);

            double xMin = Math.Min(firstPoint.X, secondPoint.X);
            double yMin = Math.Min(firstPoint.Y, secondPoint.Y);

            pivotPoint.X = xMin;
            pivotPoint.Y = yMin;

            size.Height = xTop - xMin;
            size.Width = yTop - yMin;


            Point point1 = new Point(xMin, yTop);
            Point point2 = new Point(xTop, yTop);
            Point point3 = new Point(xTop, yMin);
            Polyline line = new Polyline();
            line.Name = name;
            line.Points.Add(pivotPoint);
            line.Points.Add(point1);
            line.Points.Add(point2);
            line.Points.Add(point3);
            line.Points.Add(pivotPoint);
            polyline = line;
            Canvas.SetTop(workspace, pivotPoint.Y);
            Canvas.SetLeft(workspace, pivotPoint.X);
        }
        private void DefineOutline(Canvas canvas)
        {
            Rectangle rect = new Rectangle();
            rect.Name = name + "Outline";
            rect.Width = size.Height + 10;
            rect.Height = size.Width + 10;
            rect.StrokeThickness = 1;
            rect.Stroke = Brushes.Red;
            rect.StrokeDashArray = new DoubleCollection() { 5, 5 };
            rect.Visibility = Visibility.Hidden;
            outline = rect;
            canvas.Children.Add(outline);
            outlinePivotPoint = new Point(pivotPoint.X - 5, pivotPoint.Y - 5);
            Canvas.SetLeft(outline, pivotPoint.X - 5);
            Canvas.SetTop(outline, pivotPoint.Y - 5);
        }
        private void RedrawOutLine()
        {
            double xTop = double.MinValue;
            double yTop = double.MinValue;
            double xMin = double.MaxValue;
            double yMin = double.MaxValue;

            for (int i = 0; i < polyline.Points.Count; i++)
            {
                if (polyline.Points[i].X > xTop) xTop = polyline.Points[i].X;
                if (polyline.Points[i].Y > yTop) yTop = polyline.Points[i].Y;
                if (polyline.Points[i].X < xMin) xMin = polyline.Points[i].X;
                if (polyline.Points[i].Y < yMin) yMin = polyline.Points[i].Y;
            }

            outline.Height = yTop - yMin;
            outline.Width = xTop - xMin;

            outlinePivotPoint = new Point(xMin, yMin);
            Canvas.SetLeft(outline, xMin);
            Canvas.SetTop(outline, yMin);
        }
        private void DefineGizmoPoints(Canvas canvas)
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                centerPoint = new Point(pivotPoint.X + size.Height / 2, pivotPoint.Y + size.Width / 2);
                rotatePoint = new Point(centerPoint.X + 50, centerPoint.Y);

                Rectangle rect = new Rectangle();
                rect.Name = name + "CenterGizmo";
                rect.Height = 10;
                rect.Width = 10;
                rect.Fill = Brushes.Red;
                rect.Stroke = Brushes.Black;
                rect.StrokeThickness = 1;
                rect.Visibility = Visibility.Hidden;
                centerGizmo = rect;
                canvas.Children.Add(centerGizmo);
                SetGizmoPointPosition(centerGizmo, centerPoint);
            }
            else
            {
                for (int i = 0; i < polyline.Points.Count; i++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Name = name + "Gizmo";
                    rect.Height = 10;
                    rect.Width = 10;
                    rect.Fill = Brushes.Red;
                    rect.Stroke = Brushes.Black;
                    rect.StrokeThickness = 1;
                    rect.Visibility = Visibility.Hidden;
                    SetGizmoPointPosition(rect, polyline.Points[i]);
                    gizmos.Add(rect);
                    canvas.Children.Add(rect);
                }

                double xTop = double.MinValue, yTop = double.MinValue;
                double xMin = double.MaxValue, yMin = double.MaxValue;
                for (int i = 0; i < polyline.Points.Count; i++)
                {
                    double x = polyline.Points[i].X;
                    double y = polyline.Points[i].Y;

                    if (x > xTop) xTop = x;
                    if (y > yTop) yTop = y;
                    if (x < xMin) xMin = x;
                    if (y < yMin) yMin = y;
                }
                pivotPoint = new Point(xMin, yMin);
                maxPoint = new Point(xTop, yTop);
                centerPoint = new Point(pivotPoint.X + (xTop - xMin) / 2, pivotPoint.Y + (yTop - yMin) / 2);
                rotatePoint = new Point(centerPoint.X + 50, centerPoint.Y);
            }
        }
        private void SetGizmoPointPosition(Rectangle rect, Point centerGizmo)
        {
            Canvas.SetLeft(rect, centerGizmo.X - 5);
            Canvas.SetTop(rect, centerGizmo.Y - 5);
        }
        private void SetGizmoPointPosition(Rectangle rect, Point centerGizmo,bool pivot)
        {
            Canvas.SetLeft(rect, centerGizmo.X);
            Canvas.SetTop(rect, centerGizmo.Y);
        }
        private Point NewPointPosition(Point target, Point newPosition)
        {
            return target = new Point(target.X + newPosition.X, target.Y + newPosition.Y);
        }
        private Vector GetVector(Point newPos)
        {
            Vector vector = new Vector(pivotPoint.X - newPos.X, pivotPoint.Y - newPos.Y);
            return vector;
        }

    }
}
