using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public class RectangleFigure : Figure
    {
        public override event SelectFigureEventHandler SelectFigure;
        public override event DeselectFigureEventHandler DeselectFigure;
        public override event AddAdditionalElementEventHandler AddAdditionalElement;

        private MarkerPoint moveMarker;
        private MarkerPoint rotateMarker;
        private MarkerPoint scaleMarker;
        private Polyline rectangle;
        private MarkerPoint SelectedMarker;


        public RectangleFigure(Point firstPoint, Point secondPoint)
        {
            CreateRectangle(firstPoint, secondPoint);
            DefineAnchorPoints(firstPoint, secondPoint);
            double w = (rectangle.Points[3].X - rectangle.Points[0].X) / 2;
            double h = (rectangle.Points[1].Y - rectangle.Points[0].Y) / 2;
            moveMarker = new MarkerPoint(new Point(AnchorPoint.X + w, AnchorPoint.Y + h));
            rotateMarker = new MarkerPoint(new Point(moveMarker.Point.X - 50, moveMarker.Point.Y));
            DefineMarkers();
            rectangle.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            rectangle.StrokeEndLineCap = PenLineCap.Square;
        }
        public RectangleFigure(SerializableFigure sLFigure)
        {
            rectangle = sLFigure.Polyline.ParsePolylineFromArray();
            Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sLFigure.FillColor));
            LineColor = new SolidColorBrush
            {
                Color = (Color)ColorConverter.ConvertFromString(sLFigure.LineColor)
            };
            rectangle.StrokeEndLineCap = PenLineCap.Square;
            moveMarker = new MarkerPoint(sLFigure.GetMovePoint().ParsePoint());
            rotateMarker = new MarkerPoint(sLFigure.GetRotatePoint().ParsePoint());
            DefineMarkers();
            StrokeWidth = Convert.ToInt32(sLFigure.LineStrokeThinkness);
        }


        public override void LeftMouseButtonDown(Point position)
        {
            if (!IsSelected)
            {
                SelectLine(position);
                return;
            }

            if (IsSelected)
                SelectMarker(position);
        }
        public override void LeftMouseButtonUp(Point position)
        {
            ExecuteRelizeMarker(position);
        }
        public override void RightMouseButtonDown(Point position)
        {
            Deselect();
        }
        public override void MouseMove(Point position)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;
            MoveMarker(position);
        }
        public override void LeftMouseButtonClick(Point position)
        {
            return;
        }

        /// <summary>
        /// Для сериализации
        /// </summary>
        /// <returns></returns>
        public Point GetMoveMarker()
        {
            return moveMarker.Point;
        }
        public Point GetRotateMarker()
        {
            return rotateMarker.Point;
        }
        public Polyline GetRectangle()
        {
            return rectangle;
        }

        /// <summary>
        /// Внешние методы
        /// </summary>
        /// <returns></returns>
        public override List<Shape> GetShapes()
        {
            List<Shape> shapes = new List<Shape>();
            shapes.Add(rectangle);
            shapes.Add(moveMarker.Marker);
            shapes.Add(rotateMarker.Marker);
            shapes.Add(scaleMarker.Marker);
            return shapes;
        }
        public override void Collapse()
        {
            rectangle.Visibility = Visibility.Collapsed;
            moveMarker.Marker.Visibility = Visibility.Collapsed;
            rotateMarker.Marker.Visibility = Visibility.Collapsed;
            scaleMarker.Marker.Visibility = Visibility.Collapsed;
        }
        public override void Deselect()
        {
            HideOutline();
            SelectedMarker = null;
            IsSelected = false;
            DeselectFigure?.Invoke(this);
        }
        public override void HideOutline()
        {
            moveMarker.Hide();
            rotateMarker.Hide();
            scaleMarker.Hide();
        }

        /// <summary>
        /// Внутренние методы
        /// </summary>
        /// <param name="point"></param>
        protected override void ExecuteRelizeMarker(Point position)
        {
            SelectedMarker = null;
        }
        protected override void SelectLine(Point point)
        {
            Point A = rectangle.Points[0];
            Point B = rectangle.Points[1];
            Point C = rectangle.Points[2];
            Point D = rectangle.Points[3];
            Point O = point;

            if (O.ItIntersect(A, B, StrokeWidth) ||
                O.ItIntersect(B, C, StrokeWidth) ||
                O.ItIntersect(C, D, StrokeWidth) ||
                O.ItIntersect(D, A, StrokeWidth))
            {
                Select();
                return;
            }
            else
            {
                Deselect();
            }
        }
        protected override void MoveMarker(Point position)
        {
            if (SelectedMarker != null && SelectedMarker.Equals(moveMarker))
            {
                MoveRectangle(position);
            }
            else if (SelectedMarker != null && SelectedMarker.Equals(rotateMarker))
            {
                RotateRectangle(position);
            }
            else if (SelectedMarker != null && SelectedMarker.Equals(scaleMarker))
            {
                ScaleRectangle(position);
            }
        }
        protected override void SelectMarker(Point point)
        {
            if (moveMarker.Point.ItInsideCircle(point, StrokeWidth))
            {
                SelectedMarker = moveMarker;
                //Select();
            }
            else if (rotateMarker.Point.ItInsideCircle(point, StrokeWidth))
            {
                SelectedMarker = rotateMarker;
                //Select();
            }
            else if (scaleMarker.Point.ItInsideCircle(point, StrokeWidth))
            {
                SelectedMarker = scaleMarker;
                //Select();
            }
            else
            {
                SelectedMarker = null;
                //Deselect();
            }
        }


        public override void ShowOutline()
        {
            moveMarker.Show();
            rotateMarker.Show();
            scaleMarker.Show();
        }
        protected override int GetStrokeWidth()
        {
            return (int)rectangle.StrokeThickness;
        }
        protected override void SetStrokeWidth(int value)
        {
            rectangle.StrokeThickness = value;
            RefreshMarkerPoints();
        }
        protected override SolidColorBrush GetFill()
        {
            return (SolidColorBrush)rectangle.Fill;
        }
        protected override SolidColorBrush GetLineColor()
        {
            SolidColorBrush brush = (SolidColorBrush)rectangle.Stroke;
            return brush;
        }
        protected override void SetFill(SolidColorBrush brush)
        {
            rectangle.Fill = brush;
        }
        protected override void SetLineColor(SolidColorBrush colorBrush)
        {
            rectangle.Stroke = colorBrush;
        }


        private void Select()
        {
            IsSelected = true;
            //ShowOutline();
            SelectFigure?.Invoke(this);
        }
        private void CreateRectangle(Point firstPoint, Point secondPoint)
        {
            FigureType = FigureType.Rectangle;
            double xTop = Math.Max(firstPoint.X, secondPoint.X);
            double yTop = Math.Max(firstPoint.Y, secondPoint.Y);
            double xMin = Math.Min(firstPoint.X, secondPoint.X);
            double yMin = Math.Min(firstPoint.Y, secondPoint.Y);

            AnchorPoint = new Point(xMin, yMin);
            Point point1 = new Point(xMin, yTop);
            Point point2 = new Point(xTop, yTop);
            Point point3 = new Point(xTop, yMin);
            Polyline line = new Polyline();
            line.Points.Add(AnchorPoint);
            line.Points.Add(point1);
            line.Points.Add(point2);
            line.Points.Add(point3);
            line.Points.Add(AnchorPoint);
            rectangle = line;
            rectangle.Visibility = Visibility.Visible;
            rectangle.Stroke = Brushes.Black;
            rectangle.StrokeThickness = 1;
            rectangle.StrokeEndLineCap = PenLineCap.Square;
            //StrokeWidth = 1;
        }
        private void DefineAnchorPoints(Point firstPoint, Point secondPoint)
        {
            double xMin = Math.Min(firstPoint.X, secondPoint.X);
            double yMin = Math.Min(firstPoint.Y, secondPoint.Y);
            AnchorPoint = new Point(xMin, yMin);
        }
        private void DefineMarkers()
        {
            moveMarker.SetMarkerSize(StrokeWidth);
            rotateMarker.SetMarkerSize(StrokeWidth);
            scaleMarker = new MarkerPoint(rectangle.Points[0]);
            scaleMarker.SetMarkerSize(StrokeWidth);
        }
        private void RefreshMarkerPoints()
        {
            moveMarker.SetMarkerSize(StrokeWidth);
            rotateMarker.SetMarkerSize(StrokeWidth);
            scaleMarker.SetMarkerSize(StrokeWidth);
        }
        private void MoveRectangle(Point position)
        {
            Point delta = moveMarker.Point.DeltaTo(position);
            moveMarker.Move(position);
            AnchorPoint = new Point(AnchorPoint.X + delta.X, AnchorPoint.Y + delta.Y);
            rotateMarker.Move(new Point(rotateMarker.Point.X + delta.X, rotateMarker.Point.Y + delta.Y));
            scaleMarker.Move(new Point(scaleMarker.Point.X + delta.X, scaleMarker.Point.Y + delta.Y));
            for (int i = 0; i < rectangle.Points.Count; i++)
            {
                rectangle.Points[i] = new Point(rectangle.Points[i].X + delta.X, rectangle.Points[i].Y + delta.Y);
            }
        }
        private void ScaleRectangle(Point position)
        {
            double dopusk = moveMarker.Point.AngleBetweenPoints(rotateMarker.Point, position);
            if (dopusk < 0 || dopusk > 90) return;
            Polyline tmp = new Polyline();
            foreach (var point in rectangle.Points)
            {
                tmp.Points.Add(new Point(point.X, point.Y));
            }
            Point tmpAxis = new Point(moveMarker.Point.X - 100, moveMarker.Point.Y);
            double angle = moveMarker.Point.AngleBetweenPoints(rotateMarker.Point, tmpAxis);
            RotateTransform rotate = new RotateTransform
            {
                CenterX = moveMarker.Point.X,
                CenterY = moveMarker.Point.Y,
                Angle = angle
            };

            tmp.Points[0] = position;
            tmp.Points[4] = position;
            for (int i = 0; i < rectangle.Points.Count; i++)
            {
                Point O = rotate.Transform(tmp.Points[i]);
                tmp.Points[i] = O;
            }
            rotateMarker.Move(rotate.Transform(rotateMarker.Point));
            tmp.Points[1] = new Point(tmp.Points[0].X, tmp.Points[1].Y);
            tmp.Points[3] = new Point(tmp.Points[3].X, tmp.Points[0].Y);

            double height = tmp.Points[0].Length(tmp.Points[1]);
            double width = tmp.Points[0].Length(tmp.Points[3]);
            Point centre = new Point(tmp.Points[0].X + width / 2, tmp.Points[0].Y + height / 2);

            Point offset = centre.DeltaTo(moveMarker.Point);
            TranslateTransform translate = new TranslateTransform
            {
                X = offset.X,
                Y = offset.Y
            };
            for (int i = 0; i < tmp.Points.Count; i++)
            {
                Point pt = translate.Transform(tmp.Points[i]);
                tmp.Points[i] = pt;
            }
            centre = translate.Transform(centre);

            moveMarker.Move(centre);
            rotate.CenterX = centre.X;
            rotate.CenterY = centre.Y;
            rotate.Angle = -angle;
            for (int i = 0; i < tmp.Points.Count; i++)
            {
                Point O = rotate.Transform(tmp.Points[i]);
                tmp.Points[i] = O;
            }
            rotateMarker.Move(rotate.Transform(rotateMarker.Point));
            for (int i = 0; i < rectangle.Points.Count; i++)
            {
                rectangle.Points[i] = tmp.Points[i];
            }
            scaleMarker.Move(rectangle.Points[0]);
        }
        private void RotateRectangle(Point position)
        {
            RotateTransform rotate = new RotateTransform
            {
                CenterX = moveMarker.Point.X,
                CenterY = moveMarker.Point.Y
            };
            Vector CA = new Vector(moveMarker.Point.X - rotateMarker.Point.X, moveMarker.Point.Y - rotateMarker.Point.Y);
            Vector CB = new Vector(moveMarker.Point.X - position.X, moveMarker.Point.Y - position.Y);
            double angle = Vector.AngleBetween(CA, CB) * Math.PI / 180;
            rotate.Angle = angle;
            Point tmpRP = rotate.Transform(rotateMarker.Point);
            Point TL = rotate.Transform(scaleMarker.Point);
            for (int i = 0; i < rectangle.Points.Count; i++)
            {
                rectangle.Points[i] = rotate.Transform(rectangle.Points[i]);
            }
            rotateMarker.Move(tmpRP);
            scaleMarker.Move(TL);
        }

    }
}
