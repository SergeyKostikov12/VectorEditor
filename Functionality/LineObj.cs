using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    class LineObj : FigureObj
    {
        private List<MarkerPoint> markers;
        private Polyline polyline;
        private Canvas canvas;

        public MarkerPoint SelectedMarker;
        public List<MarkerPoint> Markers { get => markers; private set => markers = value; }
        public Polyline Polyline { get => polyline; private set => polyline = value; }

        public LineObj(Polyline polyline)
        {
            CreatePolyline(polyline);
            DefinePolyline(polyline);
            DefineMarkerPoints();
        }
        public LineObj(Point firstPoint, Point secondPoint)
        {
            CreatePolyline(firstPoint, secondPoint);
            DefineMarkerPoints();
        }
        public LineObj (SLFigure sLFigure)
        {
            CreatePolyline(sLFigure.Polyline.ParsePolylineFromArray());
            DefinePolyline(sLFigure.Polyline.ParsePolylineFromArray());
            Polyline.StrokeThickness = Convert.ToInt32(sLFigure.LineStrokeThinkness);
            DefineMarkerPoints();
            LineColor = new SolidColorBrush
            {
                Color = (Color)ColorConverter.ConvertFromString(sLFigure.LineColor)
            };
        }

        public override void ShowOutline()
        {
            foreach (var marker in Markers)
            {
                marker.Show();
            }
        }
        public override void HideOutline()
        {
            foreach (var marker in Markers)
            {
                marker.Hide();
            }
        }
        public override void PlacingInWorkPlace(Canvas _canvas)
        {
            canvas = _canvas;
            canvas.Children.Add(Polyline);
            foreach (var marker in Markers)
            {
                canvas.Children.Add(marker.Marker);
            }
        }
        public override bool SelectMarker(Point point)
        {
            foreach (var marker in markers)
            {
                bool result = marker.Point.ItInsideCircle(point, StrokeWidth);
                if (result)
                {
                    SelectedMarker = marker;
                    ShowOutline();
                    return true;
                }
                else
                {
                    SelectedMarker = null;
                }
            }
            return false;
        }
        public override void DeleteFigureFromWorkplace(Canvas canvas)
        {
            foreach (var marker in markers)
            {
                canvas.Children.Remove(marker.Marker);
            }
            canvas.Children.Remove(polyline);
        }
        public override void MoveMarker(Point position)
        {
            for (int i = 0; i < markers.Count; i++)
            {
                if (markers[i].Equals(SelectedMarker))
                {
                    markers[i].Move(position);
                    Polyline.Points[i] = position;
                }
            }
        }
        public override void ExecuteRelize(Point position)
        {
            if (SelectedMarker != null)
            {
                for (int i = 0; i < polyline.Points.Count; i++)
                {
                    if (SelectedMarker.Point.ItInsideCircle(polyline.Points[i], StrokeWidth) && !SelectedMarker.Equals(markers[i]))
                    {
                        SelectedMarker.Hide();
                        markers.Remove(SelectedMarker);
                        polyline.Points.RemoveAt(i);
                        SelectedMarker = null;
                        break;
                    }
                }
                if (polyline.Points.Count == 1)
                {
                    polyline.Points.Clear();
                    polyline.Visibility = Visibility.Hidden;
                    markers[0].Hide();
                    markers.Clear();
                }
            }
        }
        public override void ExecuteDoubleClick(Point position)
        {
            for (int i = 0; i <polyline.Points.Count-1; i++)
            {
                Point A = polyline.Points[i];
                Point B = polyline.Points[i + 1];
                if(position.ItIntersect(A,B, StrokeWidth))
                {
                    polyline.Points.Insert(i+1, position);
                    MarkerPoint marker = new MarkerPoint(position);
                    marker.SetMarkerSize(StrokeWidth);
                    markers.Insert(i+1, marker);
                    marker.Show();
                    canvas.Children.Add(marker.Marker);
                    return;
                }
            }
        }
        public override bool SelectLine(Point point)
        {
            for (int i = 0; i < markers.Count - 1; i++)
            {
                if (point.AngleBetweenPoints(markers[i].Point, markers[i + 1].Point) > 60)
                {
                    double A = point.Length(markers[i + 1].Point);
                    double B = point.Length(markers[i].Point);
                    double C = markers[i].Point.Length(markers[i + 1].Point);
                    double p = (A + B + C) / 2;
                    double S = Math.Sqrt(p * (p - A) * (p - B) * (p - C));
                    double h = 2 * S / C;
                    if (h <= 10 + StrokeWidth)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
        public override void DeselectFigure()
        {
            HideOutline();
            SelectedMarker = null;
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
            MessageBox.Show("Невозможно получить Кисть заливки!!!");
            return null;
        }
        protected override void SetFill(SolidColorBrush brush)
        {
            MessageBox.Show("Невозможно залить Линию!!!");
        }
        protected override SolidColorBrush GetLineColor()
        {
            SolidColorBrush brush = (SolidColorBrush)Polyline.Stroke;
            return brush;
        }
        protected override void SetLineColor(SolidColorBrush colorBrush)
        {
            Polyline.Stroke = colorBrush;
        }

        private void CreatePolyline(Polyline polyline)
        {
            FigureType = FigureType.Line;
            Markers = new List<MarkerPoint>();
            Polyline = new Polyline();
        }
        private void CreatePolyline(Point firstPoint, Point secondPoint)
        {
            FigureType = FigureType.Line;
            Markers = new List<MarkerPoint>();
            Polyline = new Polyline();
            Polyline.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            Polyline.Points.Add(firstPoint);
            Polyline.Points.Add(secondPoint);
        }
        private void RefreshMarkerPoints()
        {
            for (int i = 0; i < Markers.Count; i++)
            {
                Markers[i].SetMarkerSize(StrokeWidth);
            }
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
                markers.Add(new MarkerPoint(point));
            }
            Polyline.StrokeThickness = StrokeWidth;
            Polyline.Visibility = Visibility.Visible;
            Polyline.Stroke = Brushes.Black;
        }
    }
}
