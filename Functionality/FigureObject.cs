using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;



namespace GraphicEditor.Functionality
{
    public class FigureObject
    {
        private string name;//-
        // private readonly int selectedPoint;
        private Point anchorPoint;//-
        private Point centerPoint;//-
        private Point rotatePoint;//-
        private Point outlinePivotPoint;
        private Size size;//-
        private Rectangle centerGizmo;//_
        private Rectangle outline;//-
        private List<Rectangle> gizmos = new List<Rectangle>();//-
        private Polyline polyline;//--
        private SolidColorBrush fill;
        private Canvas canvas;
        private SolidColorBrush lineColor;

        public FigureType ShapeType;

        public string Name { get => name; set => name = value; }
        public double StrokeWidth { get => polyline.StrokeThickness; set => polyline.StrokeThickness = value; }
        public Point CenterPoint { get => centerPoint; set => centerPoint = value; }
        public Point RotatePoint { get => rotatePoint; set => rotatePoint = value; }
        public Rectangle Outline { get => outline; set => outline = value; }
        public Size Size { get => size; set => size = value; }
        public Polyline Polyline { get => polyline; set => polyline = value; }
        public SolidColorBrush Fill { get => fill; set { fill = value; Polyline.Fill = value; } }
        public SolidColorBrush LineColor
        {
            get => lineColor;
            set
            {
                lineColor = value;
                polyline.Stroke = lineColor;
                GetGizmoColor(lineColor);
            }
        }
        public Point OutlinePivotPoint { get => outlinePivotPoint; set => outlinePivotPoint = value; }
        public Point AnchorPoint { get => anchorPoint; set => anchorPoint = value; }
        public Rectangle CenterGizmo { get => centerGizmo; set => centerGizmo = value; }
        [XmlArray("Gizmos"), XmlArrayItem(typeof(Rectangle), ElementName = "rect")]
        public List<Rectangle> Gizmos { get => gizmos; set => gizmos = value; }

        public FigureObject(SLFigure sLFigure, Canvas _workPlace)
        {
            //canvas = _workPlace;
            //polyline = ParseFromArray(sLFigure.Polyline);
            //Name = sLFigure.Name;
            //CenterPoint = ParsePoint(sLFigure.Scale);
            //RotatePoint = ParsePoint(sLFigure.RotatePoint);
            //ShapeType = (FigureType)sLFigure.FigureTypeNumber;////////////////
            //Size = ParseSize(sLFigure.Size);
            //StrokeWidth = Convert.ToDouble(sLFigure.LineStrokeThinkness);
            //polyline.Visibility = Visibility.Visible;
            //AnchorPoint = ParsePoint(sLFigure.MovePoint);
            //DefineOutline();
            //DefineGizmoPoints();
            //Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sLFigure.FillColor));
            //LineColor = new SolidColorBrush
            //{
            //    Color = (Color)ColorConverter.ConvertFromString(sLFigure.LineColor)
            //};
            //canvas.Children.Add(polyline);
        }
        public FigureObject(string _name, FigureType _shapeType, Point _firstPoint, Point _secondPoint, Canvas _workPlace)
        {
            name = _name;
            ShapeType = _shapeType;
            canvas = _workPlace;
            lineColor = Brushes.Black;
            SetNewShape(_shapeType, _firstPoint, _secondPoint);
            DefineGizmoPoints();
            DefineOutline();
            polyline.StrokeThickness = 2;
            _workPlace.Children.Add(polyline);
            polyline.Stroke = Brushes.Black;
            polyline.Visibility = Visibility.Visible;
            Fill = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }
        public FigureObject(string _name, FigureType _shapeType, Polyline _polyline, Canvas _workPlace)
        {
            name = _name;
            ShapeType = _shapeType;
            canvas = _workPlace;
            lineColor = Brushes.Black;
            SetNewShape(_shapeType, _polyline);
            DefineGizmoPoints();
            polyline.StrokeThickness = 2;
            _workPlace.Children.Add(polyline);
            polyline.Stroke = Brushes.Black;
            polyline.Visibility = Visibility.Visible;
            Fill = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
            CenterGizmo = new Rectangle();
            Outline = new Rectangle();
        }

        public int GetPointNear(Point position)
        {
            for (int i = 0; i < polyline.Points.Count; i++)
            {
                Point delta = polyline.Points[i].AbsDeltaTo(position);
                if (delta.X <= 5 && delta.Y <= 5)
                {
                    int number = i + 1;
                    return number;
                }
                else continue;
            }

            return 0;
        }
        public Point MoveFigure(Point firstPoint, Point newPoint)
        {
            if (ShapeType == FigureType.Rectangle)
            {
                Point deltaPoint = firstPoint.DeltaTo(newPoint); //new Point(newPoint.X - firstPoint.X, newPoint.Y - firstPoint.Y);

                Canvas.SetLeft(outline, OutlinePivotPoint.X + deltaPoint.X);
                Canvas.SetTop(outline, OutlinePivotPoint.Y + deltaPoint.Y);
                Canvas.SetLeft(CenterGizmo, centerPoint.X - 5 + deltaPoint.X);
                Canvas.SetTop(CenterGizmo, centerPoint.Y - 5 + deltaPoint.Y);
                centerPoint = NewPointPosition(centerPoint, deltaPoint);
                AnchorPoint = NewPointPosition(AnchorPoint, deltaPoint);
                OutlinePivotPoint = NewPointPosition(OutlinePivotPoint, deltaPoint);

                for (int i = 0; i < polyline.Points.Count; i++)
                {
                    polyline.Points[i] = NewPointPosition(polyline.Points[i], deltaPoint);
                }
                polyline.UpdateLayout();
                return newPoint;
            }
            else return firstPoint;

        }//-
        public Point MovePointToNewPosition(int pointNumber, Point fistPoint, Point position)
        {
            Point deltaPoint = fistPoint.DeltaTo(position);

            polyline.Points[pointNumber - 1] = NewPointPosition(polyline.Points[pointNumber - 1], deltaPoint);
            Rectangle rect = Gizmos[pointNumber - 1];
            Point currentGizmoPos = GetGizmoPosition(rect);
            Point newPos = NewPointPosition(currentGizmoPos, deltaPoint);
            SetGizmoPointPosition(rect, newPos, true);
            return position;
        }//-
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
            if (ShapeType == FigureType.Line)
            {
                polyline.Points.Add(point);
            }
            else return;
        }
        public void CollapsePoints(int pointNumber)
        {
            Point pt = polyline.Points[pointNumber - 1];

            for (int i = 0; i < polyline.Points.Count; i++)
            {
                if (i == pointNumber - 1) continue;
                else
                {
                    if (pt.Near(polyline.Points[i]))
                    {
                        if (Math.Abs((pointNumber - 1) - i) <= 1)
                        {
                            polyline.Points.RemoveAt(pointNumber - 1);
                            DeleteGizmo(pointNumber - 1);
                        }
                    }
                }
            }
            if (polyline.Points.Count == 1) DeletePolyline();
        }
        public void DeletePolyline()
        {
            if (ShapeType == FigureType.Rectangle)
            {
                canvas.Children.RemoveAt(NumberByName(name + "CenterGizmo") - 1);
                canvas.Children.RemoveAt(NumberByName(name + "Outline") - 1);
                name = null;
                CenterGizmo = null;
                canvas.Children.Remove(polyline);
                canvas.Children.Remove(outline);
                Fill = null;
                polyline = null;
                outline = null;
            }
            else
            {

                for (int i = 0; i < Gizmos.Count; i++)
                {
                    int n = NumberByName(Gizmos[i].Name);
                    if (n != 0)
                    {
                        canvas.Children.RemoveAt(n - 1);
                    }
                }
                Gizmos.Clear();
                name = null;
                CenterGizmo = null;
                canvas.Children.Remove(polyline);
                Fill = null;
                polyline = null;
                outline = null;
            }




            // name + "CenterGizmo";//det from canvas
            //  name + "Outline";
        }
        public void DrawCenterGizmo(bool value)
        {
            if (ShapeType == FigureType.Rectangle)
            {
                if (value)
                {
                    CenterGizmo.Visibility = Visibility.Visible;
                }
                else CenterGizmo.Visibility = Visibility.Hidden;
            }
        }
        public void DrawOutline(bool value)
        {
            //isOutlineVisible = value;

            if (ShapeType == FigureType.Line)
            {
                DrawLineGizmos(value);
            }
            else
            {
                if (value) outline.Visibility = Visibility.Visible;
                else outline.Visibility = Visibility.Hidden;
            }
        }
        public void FillRect(SolidColorBrush brush)
        {
            if (ShapeType == FigureType.Rectangle)
            {
                Fill = brush;
                polyline.Fill = fill;
            }
            else return;
        }
        public void RemoveShape(string name)
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

        private static Size ParseSize(string deserealizedString)
        {
            return Size.Parse(deserealizedString.Replace(',', '.').Replace(';', ','));
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
            foreach (var gizmo in Gizmos)
            {
                if (gizmo.Name == name) return name;
            }
            return "None";
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
            if (ShapeType == FigureType.Rectangle)
            {
                centerPoint = new Point(AnchorPoint.X + size.Height / 2, AnchorPoint.Y + size.Width / 2);
                rotatePoint = new Point(centerPoint.X + 50, centerPoint.Y);

                Rectangle rect = new Rectangle
                {
                    Name = name + "CenterGizmo",
                    Height = 10,
                    Width = 10,
                    Fill = Brushes.Black,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Visibility = Visibility.Hidden
                };
                CenterGizmo = rect;
                canvas.Children.Add(CenterGizmo);
                SetGizmoPointPosition(CenterGizmo, centerPoint);
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
                        Fill = Brushes.Black,
                        Stroke = Brushes.Black,
                        StrokeThickness = 1,
                        Visibility = Visibility.Hidden
                    };
                    SetGizmoPointPosition(rect, polyline.Points[i]);
                    Gizmos.Add(rect);
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
                AnchorPoint = new Point(xMin, yMin);
                centerPoint = new Point(AnchorPoint.X + (xTop - xMin) / 2, AnchorPoint.Y + (yTop - yMin) / 2);
                rotatePoint = new Point(centerPoint.X + 50, centerPoint.Y);
            }
        }
        private void DefineOutline()
        {
            Rectangle rect = new Rectangle
            {
                Name = name + "Outline",
                StrokeThickness = 1,
                Width = Size.Height + 10,
                Height = Size.Width + 10,
                Stroke = Brushes.Red,
                StrokeDashArray = new DoubleCollection() { 5, 5 },
                Visibility = Visibility.Hidden
            };
            outline = rect;
            canvas.Children.Add(outline);
            OutlinePivotPoint = new Point(AnchorPoint.X - 5, AnchorPoint.Y - 5);
            Canvas.SetLeft(outline, AnchorPoint.X - 5);
            Canvas.SetTop(outline, AnchorPoint.Y - 5);
        } //-
        private void DeleteGizmo(int number)
        {
            for (int i = 0; i < canvas.Children.Count; i++)
            {
                try
                {
                    Rectangle rect = (Rectangle)canvas.Children[i];
                    if (rect.Name == Gizmos[number].Name)
                    {
                        canvas.Children.RemoveAt(i);
                        Gizmos.RemoveAt(number);
                        return;
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

            OutlinePivotPoint = new Point(xMin, yMin);
            Canvas.SetLeft(outline, xMin - 5);
            Canvas.SetTop(outline, yMin - 5);
        }
        private void SetNewShape(FigureType shapeType, Polyline _polyline)
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
            AnchorPoint = new Point(xMin, yMin);
            size = new Size(yTop - yMin, xTop - xMin);

        }
        private void SetNewShape(FigureType shapeType, Point firstPoint, Point secondPoint)
        {
            double xTop = Math.Max(firstPoint.X, secondPoint.X);
            double yTop = Math.Max(firstPoint.Y, secondPoint.Y);

            double xMin = Math.Min(firstPoint.X, secondPoint.X);
            double yMin = Math.Min(firstPoint.Y, secondPoint.Y);

            AnchorPoint = new Point(xMin, yMin);

            size.Height = xTop - xMin;
            size.Width = yTop - yMin;


            Point point1 = new Point(xMin, yTop);
            Point point2 = new Point(xTop, yTop);
            Point point3 = new Point(xTop, yMin);
            Polyline line = new Polyline { Name = name };
            line.Points.Add(AnchorPoint);
            line.Points.Add(point1);
            line.Points.Add(point2);
            line.Points.Add(point3);
            line.Points.Add(AnchorPoint);
            polyline = line;
            polyline.StrokeEndLineCap = PenLineCap.Square;
            Canvas.SetTop(canvas, AnchorPoint.Y);
            Canvas.SetLeft(canvas, AnchorPoint.X);
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
            if (AreCollinear(pt))
            {
                AddPointToLine(pt);
            }
            // polyline.Points.Insert
        }
        public bool AreCollinear(Point point)
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
            int n = GetCollinearPosition(point);
            polyline.Points.Insert(n, point);
            Gizmos.Insert(n, CreateGizmoPoint(point));
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
                if (angle >= 175) return i + 1;
            }
            return 0;
        }
        private Rectangle CreateGizmoPoint(Point position)
        {
            Rectangle rect = new Rectangle
            {
                Name = name + "Gizmo" + (Gizmos.Count + 1).ToString(),
                Height = 10,
                Width = 10,
                Fill = lineColor,
                Stroke = lineColor,
                StrokeThickness = 1,
                Visibility = Visibility.Visible
            };
            canvas.Children.Add(rect);
            Canvas.SetLeft(rect, position.X - 5);
            Canvas.SetTop(rect, position.Y - 5);
            return rect;
        }
        private void GetGizmoColor(Brush color)
        {
            foreach (var gizmo in Gizmos)
            {
                gizmo.Fill = color;
                gizmo.Stroke = color;
            }
        }
    }
}
