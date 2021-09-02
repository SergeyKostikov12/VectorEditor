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
    class LineObj : FigureObj
    {
        private List<MarkerPoint> lineMarkers;
        private Polyline polyline;

        public List<MarkerPoint> LineMarkers { get => lineMarkers; private set => lineMarkers = value; }
        public Polyline Polyline { get => polyline; private set => polyline = value; }

        public LineObj(Point firstPoint, Point secondPoint)
        {
            LineMarkers = new List<MarkerPoint>();
            Polyline = new Polyline();
            Polyline.Points.Add(firstPoint);
            Polyline.Points.Add(secondPoint);
            DefineMarkerPoints();
        }


        public LineObj(Polyline polyline)
        {
            LineMarkers = new List<MarkerPoint>();
            Polyline = new Polyline();
            DefinePolyline(polyline);
            DefineMarkerPoints();
        }



        public override void MoveFigure(Point position)
        {
        }
        public override void ShowOutline()
        {
            foreach (var marker in LineMarkers)
            {
                marker.Show();
            }
        }
        public override void HideOutline()
        {
            throw new NotImplementedException();
        }
        public override void PlacingInWorkPlace(Canvas canvas)
        {
            canvas.Children.Add(Polyline);
            foreach (var marker in LineMarkers)
            {
                canvas.Children.Add(marker.Marker);
            }
        }
        public override void AddPoint(Point point)
        {
            Polyline.Points.Add(point);
            LineMarkers.Add(CreateNewMarkerPoint(point));
        }
        public override void DeletePolyline()
        {

        }
        protected override int GetStrokeWidth()
        {
            return (int)Polyline.StrokeThickness;
        }
        protected override void SetStrokeWidth(int value)
        {
            Polyline.StrokeThickness = value;
            RefreshMarkerPoints();
        }
        protected override SolidColorBrush GetFill()
        {
            MessageBox.Show("Невозможно получить кисть заливки Линии!");
            return null;
        }
        protected override void SetFill(SolidColorBrush brush)
        {
            MessageBox.Show("Невозможно залить Линию!!!");
        }

        private void RefreshMarkerPoints()
        {
            for (int i = 0; i<LineMarkers.Count; i++)
            {
                LineMarkers[i].SetMarkerSize(StrokeWidth);
            }
        }
        private MarkerPoint CreateNewMarkerPoint(Point point)
        {
            MarkerPoint markerPoint = new MarkerPoint(point);
            markerPoint.SetMarkerSize(StrokeWidth);
            return markerPoint;
        }
        private void DefinePolyline(Polyline polyline)
        {
            foreach (var point in polyline.Points)
            {
                Polyline.Points.Add(point);
            }
        }
        private void DefineMarkerPoints()
        {
            foreach (var point in Polyline.Points)
            {
                lineMarkers.Add(new MarkerPoint(point));
            }
            Polyline.StrokeThickness = StrokeWidth;
            Polyline.Visibility = Visibility.Visible;
            Polyline.Stroke = Brushes.Black;
        }
    }
}
