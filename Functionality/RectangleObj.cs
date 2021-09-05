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
    public class RectangleObj : FigureObj
    {
        private MarkerPoint moveMarker;
        private MarkerPoint rotateMarker;
        private MarkerPoint scaleMarker;
        private Polyline rectangle;

        public MarkerPoint SelectedMarker;
        public MarkerPoint MovePoint { get => moveMarker; set => moveMarker = value; }
        public MarkerPoint RotatePoint { get => rotateMarker; set => rotateMarker = value; }
        public MarkerPoint ScalePoint { get => scaleMarker; set => scaleMarker = value; }
        public Polyline Rectangle { get => rectangle; set => rectangle = value; }

        public RectangleObj(Point firstPoint, Point secondPoint)
        {
            CreateRectangle(firstPoint, secondPoint);
            DefineAnchorPoints(firstPoint, secondPoint);
            double w = (Rectangle.Points[3].X - Rectangle.Points[0].X) / 2;
            double h = (Rectangle.Points[1].Y - Rectangle.Points[0].Y) / 2;
            MovePoint = new MarkerPoint(new Point(AnchorPoint.X + w, AnchorPoint.Y + h));
            DefineMarkers();
            Rectangle.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            
        }
        public RectangleObj(SLFigure sLFigure)
        {
            Rectangle = sLFigure.Polyline.ParsePolylineFromArray();
            StrokeWidth = Convert.ToInt32(sLFigure.LineStrokeThinkness);
            Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(sLFigure.FillColor));
            LineColor = new SolidColorBrush
            {
                Color = (Color)ColorConverter.ConvertFromString(sLFigure.LineColor)
            };
            //AnchorPoint = sLFigure.AnchorPoint.ParsePoint();
            MovePoint = new MarkerPoint(sLFigure.MovePoint.ParsePoint());
            DefineMarkers();
        }


        public override void MoveMarker(Point position)
        {
            if (SelectedMarker != null && SelectedMarker.Equals(MovePoint))
            {
                MoveRectangle(position);
            }
            else if (SelectedMarker != null && SelectedMarker.Equals(RotatePoint))
            {
                RotateRectangle(position);
            }
            else if (SelectedMarker != null && SelectedMarker.Equals(ScalePoint))
            {
                ScaleRectangle(position);
            }
        }
        private void ScaleRectangle(Point position)
        {
            int stroke = StrokeWidth;
            ScaleTransform scale = new ScaleTransform();
            scale.CenterX = MovePoint.Point.X;
            scale.CenterY = MovePoint.Point.Y;
            double delta = position.X - ScalePoint.X;
            scale.ScaleX = 1 + delta / 100;
            scale.ScaleY = 1 + delta/ 100;
            ScalePoint.Move(position);

            for (int i = 0; i < Rectangle.Points.Count; i++)
            {
                Rectangle.Points[i] = scale.Transform(Rectangle.Points[i]);
            }
        }
        private void RotateRectangle(Point position)
        {
            //RotateTransform rotate = new RotateTransform();
            RotateTransform rotate = new RotateTransform();

            rotate.CenterX = MovePoint.X;
            rotate.CenterY = MovePoint.Y;
            Vector CA = new Vector(MovePoint.X - RotatePoint.X, MovePoint.Y - RotatePoint.Y);
            Vector CB = new Vector(MovePoint.X - position.X, MovePoint.Y - position.Y);
            double angle = Vector.AngleBetween(CA, CB) * Math.PI / 180;
            rotate.Angle = angle;
            Point tmpRP = rotate.Transform(RotatePoint.Point);
            Point tmpSP = rotate.Transform(ScalePoint.Point);

            for (int i = 0; i < Rectangle.Points.Count; i++)
            {
                Rectangle.Points[i] = rotate.Transform(Rectangle.Points[i]);
            }


            //A_corner = rotate.Transform(A_corner);
            //B_corner = rotate.Transform(B_corner);
            //C_corner = rotate.Transform(C_corner);
            //D_corner = rotate.Transform(D_corner);
            //rectangleRotate += angle;

            //RectangleO.RenderTransform = transform;//new RotateTransform(rectangleRotate, RectangleO.Width / 2, RectangleO.Height / 2);
            //outlineRectangle.RenderTransform = transform; //new RotateTransform(rectangleRotate, outlineRectangle.Width / 2, outlineRectangle.Height / 2);
            //RectangleO.UpdateLayout();
            RotatePoint.Move(tmpRP);
            ScalePoint.Move(tmpSP);
        }
        private void MoveRectangle(Point position)
        {
            Point delta = MovePoint.Point.DeltaTo(position);
            MovePoint.Move(position);
            AnchorPoint = new Point(AnchorPoint.X + delta.X, AnchorPoint.Y + delta.Y);
            RotatePoint.Move(new Point(RotatePoint.X + delta.X, RotatePoint.Y + delta.Y));
            ScalePoint.Move(new Point(ScalePoint.X + delta.X, ScalePoint.Y + delta.Y));
            for(int i = 0; i<Rectangle.Points.Count; i++)
            {
                Rectangle.Points[i] = new Point(Rectangle.Points[i].X + delta.X, Rectangle.Points[i].Y + delta.Y);
            }
        }


        public override void ShowOutline()
        {
            MovePoint.Show();
            RotatePoint.Show();
            ScalePoint.Show();
        }
        public override void HideOutline()
        {
            MovePoint.Hide();
            RotatePoint.Hide();
            ScalePoint.Hide();
        }
        public override void PlacingInWorkPlace(Canvas canvas)
        {
            canvas.Children.Add(Rectangle);
            canvas.Children.Add(MovePoint.Marker);
            canvas.Children.Add(RotatePoint.Marker);
            canvas.Children.Add(ScalePoint.Marker);
        }
        public override bool SelectMarker(Point point)
        {
            if (moveMarker.Point.ItInsideCircle(point, StrokeWidth))
            {
                SelectedMarker = moveMarker;
                return true;
            }
            else if (rotateMarker.Point.ItInsideCircle(point, StrokeWidth))
            {
                SelectedMarker = rotateMarker;
                return true;
            }
            else if (scaleMarker.Point.ItInsideCircle(point, StrokeWidth))
            {
                SelectedMarker = scaleMarker;
                return true;
            }
            else
            {
                SelectedMarker = null;
                return false;
            }
        }
        public override bool SelectLine(Point point)
        {
            Point A = Rectangle.Points[0];
            Point B = Rectangle.Points[1];
            Point C = Rectangle.Points[2];
            Point D = Rectangle.Points[3];
            Point O = point;

            if (O.ItIntersect(A, B, StrokeWidth) ||
                O.ItIntersect(B, C, StrokeWidth) ||
                O.ItIntersect(C, D, StrokeWidth) ||
                O.ItIntersect(D, A, StrokeWidth))
            {
                return true;
            }
            else return false;
        }
        public override void DeselectFigure()
        {
            HideOutline();
            SelectedMarker = null;
        }

        protected override int GetStrokeWidth()
        {
            return (int)Rectangle.StrokeThickness;
        }
        protected override void SetStrokeWidth(int value)
        {
            Rectangle.StrokeThickness = value;
        }
        protected override SolidColorBrush GetFill()
        {
            return (SolidColorBrush)Rectangle.Fill;
        }
        protected override void SetFill(SolidColorBrush brush)
        {
            Rectangle.Fill = brush;
        }
        public override void DeleteFigureFromWorkplace(Canvas canvas)
        {
            canvas.Children.Remove(Rectangle);
            canvas.Children.Remove(MovePoint.Marker);
            canvas.Children.Remove(RotatePoint.Marker);
            canvas.Children.Remove(ScalePoint.Marker);
        }

        private void DefineAnchorPoints(Point firstPoint, Point secondPoint) 
        {
            double xMin = Math.Min(firstPoint.X, secondPoint.X);
            double yMin = Math.Min(firstPoint.Y, secondPoint.Y);
            AnchorPoint = new Point(xMin, yMin);
        }
        private void DefineMarkers()
        {
            MovePoint.SetMarkerSize(StrokeWidth);
            RotatePoint = new MarkerPoint(new Point(MovePoint.X, MovePoint.Y - 50));
            RotatePoint.SetMarkerSize(StrokeWidth);
            ScalePoint = new MarkerPoint(new Point(MovePoint.X + 100, MovePoint.Y));
            ScalePoint.SetMarkerSize(StrokeWidth);
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
            Rectangle = line;
            Rectangle.Visibility = Visibility.Visible;
            Rectangle.Stroke = Brushes.Black;
            Rectangle.StrokeThickness = 1;
            Rectangle.StrokeEndLineCap = PenLineCap.Square;
            StrokeWidth = 1;
        }
        protected override SolidColorBrush GetLineColor()
        {
            SolidColorBrush brush = (SolidColorBrush)Rectangle.Stroke;
            return brush;
        }
        protected override void SetLineColor(SolidColorBrush colorBrush)
        {
            Rectangle.Stroke = colorBrush;
        }
        public override void ExecuteRelize(Point position)
        {
            RotatePoint.Move(new Point(MovePoint.X, MovePoint.Y - 50));
            ScalePoint.Move(new Point(MovePoint.X + 100, MovePoint.Y));
        }
    }
}
