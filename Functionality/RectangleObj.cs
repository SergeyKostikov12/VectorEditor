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
        private Rectangle rectangle;
        private Rectangle outlineRectangle;
        private Point outlineAnchorPoint;

        public MarkerPoint MovePoint { get => moveMarker; set => moveMarker = value; }
        public MarkerPoint RotatePoint { get => rotateMarker; set => rotateMarker = value; }
        public MarkerPoint ScalePoint { get => scaleMarker; set => scaleMarker = value; }
        public Rectangle Rectangle { get => rectangle; private set => rectangle = value; }
        public Rectangle OutlineRectangle { get => outlineRectangle; private set => outlineRectangle = value; }

        public RectangleObj(Point firstPoint, Point secondPoint)
        {
            CreateRectangle(firstPoint, secondPoint);
            CreateOutlineRectangle();
            DefineAnchorPoints(firstPoint,secondPoint);
            DefineMarkers();
            PlaceRectangle();
            PlaceOutlineRectangle();
        }

        public override void MoveFigure(Point newPosition)
        {
            AnchorPoint = AnchorPoint.Move(newPosition);
            outlineAnchorPoint = outlineAnchorPoint.Move(newPosition);
            PlaceRectangle();
            PlaceOutlineRectangle();
        }
        public override void ShowOutline()
        {
            OutlineRectangle.Visibility = Visibility.Visible;
            MovePoint.Show();
            RotatePoint.Show();
            ScalePoint.Show();
        }
        public override void HideOutline()
        {
            OutlineRectangle.Visibility = Visibility.Hidden;
            MovePoint.Hide();
            RotatePoint.Hide();
            ScalePoint.Hide();
        }
        public override void PlacingInWorkPlace(Canvas canvas)
        {
            canvas.Children.Add(Rectangle);
            canvas.Children.Add(OutlineRectangle);
            canvas.Children.Add(MovePoint.Marker);
            canvas.Children.Add(RotatePoint.Marker);
            canvas.Children.Add(ScalePoint.Marker);
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

        private void PlaceOutlineRectangle()
        {
            Canvas.SetLeft(OutlineRectangle, outlineAnchorPoint.X);
            Canvas.SetTop(OutlineRectangle, outlineAnchorPoint.Y);
        }
        private void PlaceRectangle()
        {
            Canvas.SetLeft(Rectangle, AnchorPoint.X);
            Canvas.SetTop(Rectangle, AnchorPoint.Y);
        }
        private void CreateOutlineRectangle()
        {
            Size size = new Size(Rectangle.Width+StrokeWidth/2 + 10, Rectangle.Height + StrokeWidth / 2 + 10);
            Rectangle rect = new Rectangle()
            {
                StrokeThickness = 1,
                Width = size.Width,
                Height = size.Height,
                Stroke = Brushes.Red,
                StrokeDashArray = new DoubleCollection() { 5, 5 },
                Visibility = Visibility.Hidden
            };
            OutlineRectangle = rect;
        }
        private void DefineAnchorPoints(Point firstPoint, Point secondPoint)
        {
            double xMin = Math.Min(firstPoint.X, secondPoint.X);
            double yMin = Math.Min(firstPoint.Y, secondPoint.Y);
            AnchorPoint = new Point(xMin, yMin);
            outlineAnchorPoint = new Point(xMin - StrokeWidth / 2 - 5, yMin - StrokeWidth / 2 - 5);
        }
        private void DefineMarkers()
        {
            MovePoint = new MarkerPoint(new Point(AnchorPoint.X + Rectangle.Width / 2, AnchorPoint.Y + Rectangle.Height / 2));
            MovePoint.SetMarkerSize(StrokeWidth);
            RotatePoint = new MarkerPoint(new Point(moveMarker.X, moveMarker.Y - 30));
            RotatePoint.SetMarkerSize(StrokeWidth);
            ScalePoint = new MarkerPoint(new Point(AnchorPoint.X+Rectangle.Width, AnchorPoint.Y+ Rectangle.Height));
            ScalePoint.SetMarkerSize(StrokeWidth);
        }
        private void CreateRectangle(Point firstPoint, Point secondPoint)
        {
            Rectangle rect = new Rectangle();
            Rectangle = rect;
            double xTop = Math.Max(firstPoint.X, secondPoint.X);
            double yTop = Math.Max(firstPoint.Y, secondPoint.Y);

            double xMin = Math.Min(firstPoint.X, secondPoint.X);
            double yMin = Math.Min(firstPoint.Y, secondPoint.Y);

            rect.Width = xTop - xMin;
            rect.Height = yTop - yMin;
            rect.Visibility = Visibility.Visible;
            rect.Stroke = Brushes.Black;
            rect.StrokeThickness = StrokeWidth;
        }

    }
}
