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
        private string name;
        private int selectedPoint;
        private int strokeWidth;
        private Point pivotPoint;
        private Point centerPoint;
        private Point rotatePoint;
        private Point outlinePivotPoint;
        private Size size;
        private Rectangle centerGizmo;
        private Rectangle outline;
        private List<Rectangle> gizmos = new List<Rectangle>();
        private Polyline polyline;
        private Brush fill;
        private Canvas canvas;

        public ShapeType ShapeType;

        public string Name { get => name; set => name = value; }
        public int StrokeWidth { get => strokeWidth; set => strokeWidth = value; }
        public Point FirstPoint { get => pivotPoint; }
        public Point CenterPoint { get => centerPoint; }
        public Point RotatePoint { get => rotatePoint; }
        public Rectangle Outline { get => outline; }
        public Size Size { get => size; }
        public Polyline Shape { get => polyline; }
        public Brush Fill { get => fill; }

        public FigureObject(string _name, ShapeType _shapeType, Point _firstPoint, Point _secondPoint, Canvas _workPlace)
        {
            name = _name;
            ShapeType = _shapeType;
            canvas = _workPlace;
            SetNewShape(_shapeType, _firstPoint, _secondPoint);
            DefineGizmoPoints();
            DefineOutline();
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
            DefineGizmoPoints();
            polyline.StrokeThickness = 2;
            _workPlace.Children.Add(polyline);
            polyline.Stroke = Brushes.Black;
            polyline.Visibility = Visibility.Visible;
        }

        public Point GetPointNear(Point position, out int number)
        {
            for (int i = 0; i < polyline.Points.Count; i++)
            {
                Point delta = new Point(Math.Abs(position.X - polyline.Points[i].X), Math.Abs(position.Y - polyline.Points[i].Y));
                if (delta.X <= 5 && delta.Y <= 5)
                {
                    selectedPoint = i;
                    number = i+1;
                    return polyline.Points[i];
                }
                else continue;
            }
            number = 0;
            return new Point(9999, 0);
        }
        public Point MoveFigure(Point firstPoint, Point newPoint)
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                Point deltaPoint = new Point(newPoint.X - firstPoint.X, newPoint.Y - firstPoint.Y);

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
                return newPoint;
            }
            else return firstPoint;

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
        public Point ScaleFigure(Point firstPoint, Point newPoint)
        {
            ScaleTransform scale = new ScaleTransform();
            scale.CenterX = centerPoint.X;
            scale.CenterY = centerPoint.Y;
            double delta = newPoint.Y - firstPoint.Y;

            scale.ScaleX = 1 - delta / 1000;
            scale.ScaleY = 1 - delta / 1000;
            for (int i = 0; i < polyline.Points.Count; i++)
            {
                polyline.Points[i] = scale.Transform(polyline.Points[i]);
            }
            RedrawOutLine();
            return newPoint;
        }

        public void AddLine(Point point)
        {
            if (ShapeType == ShapeType.Line)
            {
                polyline.Points.Add(point);
            }
            else return;
        }
        public void CollapsePoints(Point point)
        {
            for (int i = 0; i < polyline.Points.Count; i++)
            {
                Point delta = DeltaPoint(polyline.Points[i], point);
                if (delta.X <= 5 && delta.Y <= 5)
                {
                    selectedPoint = i;

                    if (i == 0)
                    {
                        Point deltaNextPoint = DeltaPoint(polyline.Points[i], polyline.Points[i + 1]);
                        if (deltaNextPoint.X <= 5 && deltaNextPoint.Y <= 5)
                        {
                            polyline.Points.RemoveAt(0);
                            DeleteGizmo(0);
                        }
                    }
                    else if (i == polyline.Points.Count - 1)
                    {
                        Point deltaPreviosPoint = DeltaPoint(polyline.Points[i], polyline.Points[i - 1]);
                        if (deltaPreviosPoint.X <= 5 && deltaPreviosPoint.Y <= 5)
                        {
                            polyline.Points.RemoveAt(polyline.Points.Count - 1);
                            DeleteGizmo(polyline.Points.Count);
                        }
                    }
                    else if (i != polyline.Points.Count - 1 && i != 0)
                    {
                        Point deltaNextPoint = DeltaPoint(polyline.Points[i], polyline.Points[i + 1]);
                        Point deltaPreviosPoint = DeltaPoint(polyline.Points[i], polyline.Points[i - 1]);
                        if (deltaNextPoint.X <= 5 && deltaNextPoint.Y <= 5)
                        {
                            polyline.Points.RemoveAt(i);
                            DeleteGizmo(i);
                        }
                        else if (deltaPreviosPoint.X <= 5 && deltaPreviosPoint.Y <= 5)
                        {
                            polyline.Points.RemoveAt(i);
                            DeleteGizmo(i);
                        }
                    }



                }
                else continue;
            }
            if (polyline.Points.Count == 1) DeletePolyline();
        }
        public void DeletePolyline()
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                canvas.Children.RemoveAt(NumberByName(name + "CenterGizmo") - 1);
                canvas.Children.RemoveAt(NumberByName(name + "Outline") - 1);
                name = null;
                centerGizmo = null;
                canvas.Children.Remove(polyline);
                canvas.Children.Remove(outline);
                polyline = null;
                outline = null;
                fill = null;
            }
            else
            {

                for (int i = 0; i < gizmos.Count; i++)
                {
                    int n = NumberByName(gizmos[i].Name);
                    if (n != 0)
                    {
                        canvas.Children.RemoveAt(n - 1);
                    }
                }
                gizmos.Clear();
                name = null;
                centerGizmo = null;
                canvas.Children.Remove(polyline);
                polyline = null;
                outline = null;
                fill = null;
            }




            // name + "CenterGizmo";//det from canvas
            //  name + "Outline";
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
        public void DrawOutline(bool value)
        {
            //isOutlineVisible = value;

            if (ShapeType == ShapeType.Line)
            {
                DrawLineGizmos(value);
            }
            else
            {
                if (value) outline.Visibility = Visibility.Visible;
                else outline.Visibility = Visibility.Hidden;
            }
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
        public void RemoveShape(string name) //Надо будет проверить!!!!!!!!!!!
        {
            var par = (Canvas)polyline.Parent;
            par.Children.Remove(polyline);
            par.Children.Remove(outline);
            par.UpdateLayout();
        }
        public void SetLineThickness(int value)
        {
            polyline.StrokeThickness = value;
        }
        private int NumberByName(string name)
        {
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                try
                {
                    Rectangle _1 = (Rectangle)canvas.Children[i];
                    if (_1.Name == name)
                    {
                        return i + 1;
                    }
                }
                catch { }
            }
            return 0;
        }
        private string GetRectByName(string name)
        {
            foreach (var gizmo in gizmos)
            {
                if (gizmo.Name == name) return name;
            }
            return "None";
        }
        private Point DeltaPoint(Point first, Point second)
        {
            return new Point(Math.Abs(second.X - first.X), Math.Abs(second.Y - first.Y));
        }
        private Point GetGizmoPosition(Rectangle gizmo)
        {
            Point point = new Point
            {
                X = Canvas.GetLeft(gizmo),
                Y = Canvas.GetTop(gizmo)
            };
            return point;
        }
        private Point NewPointPosition(Point target, Point newPosition) => target = new Point(target.X + newPosition.X, target.Y + newPosition.Y);
        private void DefineGizmoPoints()
        {
            if (ShapeType == ShapeType.Rectangle)
            {
                centerPoint = new Point(pivotPoint.X + size.Height / 2, pivotPoint.Y + size.Width / 2);
                rotatePoint = new Point(centerPoint.X + 50, centerPoint.Y);

                Rectangle rect = new Rectangle
                {
                    Name = name + "CenterGizmo",
                    Height = 10,
                    Width = 10,
                    Fill = Brushes.Red,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Visibility = Visibility.Hidden
                };
                centerGizmo = rect;
                canvas.Children.Add(centerGizmo);
                SetGizmoPointPosition(centerGizmo, centerPoint);
            }
            else
            {
                for (int i = 0; i < polyline.Points.Count; i++)
                {
                    Rectangle rect = new Rectangle
                    {
                        Name = name + "Gizmo" + i.ToString(),
                        Height = 10,
                        Width = 10,
                        Fill = Brushes.Red,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Visibility = Visibility.Hidden
                    };
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
                centerPoint = new Point(pivotPoint.X + (xTop - xMin) / 2, pivotPoint.Y + (yTop - yMin) / 2);
                rotatePoint = new Point(centerPoint.X + 50, centerPoint.Y);
            }
        }
        private void DefineOutline()
        {
            Rectangle rect = new Rectangle
            {
                Name = name + "Outline",
                Width = size.Height + 10,
                Height = size.Width + 10,
                StrokeThickness = 1,
                Stroke = Brushes.Red,
                StrokeDashArray = new DoubleCollection() { 5, 5 },
                Visibility = Visibility.Hidden
            };
            outline = rect;
            canvas.Children.Add(outline);
            outlinePivotPoint = new Point(pivotPoint.X - 5, pivotPoint.Y - 5);
            Canvas.SetLeft(outline, pivotPoint.X - 5);
            Canvas.SetTop(outline, pivotPoint.Y - 5);
        }
        private void DeleteGizmo(int number)
        {
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                try
                {
                    Rectangle rect = (Rectangle)canvas.Children[i];
                    if (rect.Name == gizmos[number].Name)
                    {
                        canvas.Children.RemoveAt(i);
                        gizmos.RemoveAt(number);
                    }
                }
                catch
                {
                }
            }
        }
        private void DrawLineGizmos(bool value)
        {
            if (value)
            {
                foreach (var item in canvas.Children)
                {
                    try
                    {
                        Rectangle rect = (Rectangle)item;
                        if (rect.Name == GetRectByName(rect.Name))
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

            outline.Height = yTop - yMin + 10;
            outline.Width = xTop - xMin + 10;

            outlinePivotPoint = new Point(xMin, yMin);
            Canvas.SetLeft(outline, xMin - 5);
            Canvas.SetTop(outline, yMin - 5);
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
        private void SetNewShape(ShapeType shapeType, Point firstPoint, Point secondPoint)
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
            Polyline line = new Polyline { Name = name };
            line.Points.Add(pivotPoint);
            line.Points.Add(point1);
            line.Points.Add(point2);
            line.Points.Add(point3);
            line.Points.Add(pivotPoint);
            polyline = line;
            Canvas.SetTop(canvas, pivotPoint.Y);
            Canvas.SetLeft(canvas, pivotPoint.X);
        }
        private void SetGizmoPointPosition(Rectangle rect, Point centerGizmo)
        {
            Canvas.SetLeft(rect, centerGizmo.X - 5);
            Canvas.SetTop(rect, centerGizmo.Y - 5);
        }
        private void SetGizmoPointPosition(Rectangle rect, Point centerGizmo, bool pivot)
        {
            Canvas.SetLeft(rect, centerGizmo.X);
            Canvas.SetTop(rect, centerGizmo.Y);
        }
        public void AddPointFromDoubleClick(Point pt)
        {
            if(AreCollinear(pt))
            {
                AddPointToLine(pt);
            }
            // polyline.Points.Insert
        }
        private bool AreCollinear(Point point)
        {
            for (int i = 0; i < polyline.Points.Count - 1; i++)
            {
                Point A = polyline.Points[i];
                Point B = polyline.Points[i + 1];

                Vector CA = new Vector(A.X - point.X, A.Y - point.Y);
                Vector CB = new Vector(B.X - point.X, B.Y - point.Y);

                double angle = Math.Abs(Vector.AngleBetween(CA, CB));
                if (angle >= 175) return true;
            }
                return false;
        }
        private void AddPointToLine(Point point)
        {
            polyline.Points.Insert(GetCollinearPosition(point), point);
            gizmos.Insert(GetCollinearPosition(point), CreateGizmoPoint(point));
        }
        private int GetCollinearPosition(Point point)
        {
            for (int i = 0; i < polyline.Points.Count - 1; i++)
            {
                Point A = polyline.Points[i];
                Point B = polyline.Points[i + 1];

                Vector CA = new Vector(A.X - point.X, A.Y - point.Y);
                Vector CB = new Vector(B.X - point.X, B.Y - point.Y);

                double angle = Math.Abs(Vector.AngleBetween(CA, CB));
                if (angle >= 175) return i+1;
            }
            return 0;
        }
        private Rectangle CreateGizmoPoint(Point position)
        {
            Rectangle rect = new Rectangle
            {
                Name = name + "Gizmo" + gizmos.Count.ToString(),
                Height = 10,
                Width = 10,
                Fill = Brushes.Red,
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Visibility = Visibility.Visible
            };
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, position.X - 5);
            Canvas.SetTop(rect, position.Y-5);
            return rect;
        }  /////////////////////////////////////////
    }
}
