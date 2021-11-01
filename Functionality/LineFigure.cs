using GraphicEditor.Events;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    class LineFigure : Figure
    {
        //public override event SelectFigureEventHandler SelectFigure;
        public override event FigureSelectEventHandler SelectFigure;
        public override event FigureDeselectEventHandler DeselectFigure;
        public override event AddAdditionalElementEventHandler AddAdditionalElement;

        private Polyline polyline;
        private MarkerPoint selectedMarker;
        private List<MarkerPoint> markers;
        private Point firstPosition;


        public LineFigure(Shape line)
        {
            var newLine = (Polyline)line;
            CreatePolyline();
            DefinePolyline(newLine);
            DefineMarkerPoints();
        }
        public LineFigure(SerializableFigure sLFigure)
        {
            CreatePolyline();
            DefinePolyline(sLFigure.Polyline.ParsePolylineFromArray());
            DefineMarkerPoints();
            polyline.StrokeThickness = Convert.ToInt32(sLFigure.LineStrokeThinkness);
            LineColor = new SolidColorBrush
            {
                Color = (Color)ColorConverter.ConvertFromString(sLFigure.LineColor)
            };
        }

        /// <summary>
        /// События
        /// </summary>
        /// <param></param>
        public override void LeftMouseButtonDown(Point position)
        {
            firstPosition = new Point(position.X, position.Y);
            if (!IsSelected)
            {
                SelectLine(position);
                return;
            }
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
            //if (Mouse.LeftButton != MouseButtonState.Pressed)
            //    return;
            if (selectedMarker != null)
            {
                MoveMarker(position);
            }
            else if(IsPointNearLine(firstPosition))
            {
                MoveLine(position);
                firstPosition = position;
            }
        }
        public override void LeftMouseButtonClick(Point position)
        {
            InsertPoint(position);
        }

        /// <summary>
        /// Для сериализации
        /// </summary>
        /// <returns></returns>
        public Polyline GetPolyline()
        {
            return polyline;
        }
        public List<MarkerPoint> GetMarkerPoints()
        {
            return markers;
        }


        /// <summary>
        /// Публичные методы
        /// </summary>
        /// <returns></returns>
        public override List<Shape> GetShapes()
        {
            List<Shape> shapes = new List<Shape>();
            shapes.Add(polyline);
            foreach (var marker in markers)
            {
                shapes.Add(marker.Marker);
            }
            return shapes;
        }
        public override void HideOutline()
        {
            foreach (var marker in markers)
            {
                marker.Hide();
            }
        }
        public override void Deselect()
        {
            HideOutline();
            selectedMarker = null;
            IsSelected = false;
            DeselectFigure?.Invoke(this);
        }
        public override void Collapse()
        {
            polyline.Visibility = Visibility.Collapsed;
            foreach (var marker in markers)
            {
                marker.Marker.Visibility = Visibility.Collapsed;
            }
        }


        /// <summary>
        /// Приватные методы
        /// </summary>
        /// 
        protected override void SelectLine(Point point)
        {
            if(IsPointNearLine(point))
            {
                IsSelected = true;
                SelectFigure?.Invoke(this, new FigureSelectEventArgs(StrokeWidth));
                return;
            }
            Deselect();
        }
        protected override void SelectMarker(Point point)
        {
            foreach (var marker in markers)
            {
                bool result = marker.Point.ItInsideCircle(point, StrokeWidth);
                if (result)
                {
                    selectedMarker = marker;
                    //ShowOutline();
                    return;
                }
                else
                {
                    selectedMarker = null;
                }
            }
        }
        public override void ShowOutline()
        {
            foreach (var marker in markers)
            {
                marker.Show();
            }
        }
        protected override void ExecuteRelizeMarker(Point position)
        {
            if (polyline.Points.Count <= 2) return;
            if (selectedMarker == null) return;
            int n = 0;
            for (int i = 0; i < markers.Count; i++)
            {
                if (!markers[i].Equals(selectedMarker)) continue;
                n = i;
                break;
            }
            if (n == 0 || n == markers.Count - 1)
            {
                if (n == 0)
                {
                    if (selectedMarker.Point.ItInsideCircle(polyline.Points[1], StrokeWidth))
                    {
                        selectedMarker.Hide();
                        markers.Remove(selectedMarker);
                        polyline.Points.RemoveAt(0);
                        selectedMarker = null;
                    }
                }
                if (n == polyline.Points.Count - 1)
                {
                    if (selectedMarker.Point.ItInsideCircle(polyline.Points[polyline.Points.Count - 2], StrokeWidth))
                    {
                        selectedMarker.Hide();
                        markers.Remove(selectedMarker);
                        polyline.Points.RemoveAt(polyline.Points.Count - 1);
                        selectedMarker = null;
                    }
                }
            }
            else
            {
                if (selectedMarker.Point.ItInsideCircle(markers[n - 1].Point, StrokeWidth))
                {
                    selectedMarker.Hide();
                    markers.Remove(selectedMarker);
                    polyline.Points.RemoveAt(n);
                    selectedMarker = null;
                }
                else if (selectedMarker.Point.ItInsideCircle(markers[n + 1].Point, StrokeWidth))
                {
                    selectedMarker.Hide();
                    markers.Remove(selectedMarker);
                    polyline.Points.RemoveAt(n);
                    selectedMarker = null;
                }
            }

        }
        protected override void MoveMarker(Point position)
        {
            if (selectedMarker == null)
                return;
            for (int i = 0; i < markers.Count; i++)
            {
                if (markers[i].Equals(selectedMarker))
                {
                    markers[i].Move(position);
                    polyline.Points[i] = position;
                }
            }
        }


        protected override int GetStrokeWidth()
        {
            return (int)polyline.StrokeThickness;
        }
        protected override void SetStrokeWidth(int value)
        {
            polyline.StrokeThickness = value;
            RefreshMarkerPoints();
        }
        protected override SolidColorBrush GetFill()
        {
            MessageBox.Show("Невозможно получить Кисть заливки!!!");
            return null;
        }
        protected override SolidColorBrush GetLineColor()
        {
            SolidColorBrush brush = (SolidColorBrush)polyline.Stroke;
            return brush;
        }
        protected override void SetLineColor(SolidColorBrush colorBrush)
        {
            polyline.Stroke = colorBrush;
        }
        protected override void SetFill(SolidColorBrush brush)
        {
            MessageBox.Show("Невозможно залить Линию!!!");
        }

        private void MoveLine(Point position)
        {
            var delta = firstPosition.DeltaTo(position);
            foreach (var marker in markers)
            {
                var pt = new Point(marker.Point.X + delta.X, marker.Point.Y + delta.Y);
                marker.Move(pt);
            }
            for(int i = 0; i<polyline.Points.Count; i++)
            {
                polyline.Points[i] = new Point(polyline.Points[i].X + delta.X, polyline.Points[i].Y + delta.Y);
            }
        }
        private bool IsPointNearLine(Point point)
        {
            for (int i = 0; i < markers.Count - 1; i++)
            {
                if (point.AbsAngleBetweenPoints(markers[i].Point, markers[i + 1].Point) > 60)
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
        private void InsertPoint(Point position)
        {
            for (int i = 0; i < polyline.Points.Count - 1; i++)
            {
                Point A = polyline.Points[i];
                Point B = polyline.Points[i + 1];
                if (position.ItIntersect(A, B, StrokeWidth))
                {
                    polyline.Points.Insert(i + 1, position);
                    MarkerPoint marker = new MarkerPoint(position);
                    marker.SetMarkerSize(StrokeWidth);
                    markers.Insert(i + 1, marker);
                    marker.Show();
                    AddAdditionalElement?.Invoke(marker.Marker);
                    return;
                }
            }
            return;
        }
        private void CreatePolyline()
        {
            FigureType = FigureType.Line;
            markers = new List<MarkerPoint>();
            polyline = new Polyline
            {
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round
            };
        }
        private void DefineMarkerPoints()
        {
            foreach (var point in polyline.Points)
            {
                markers.Add(new MarkerPoint(point));
            }
            polyline.StrokeThickness = StrokeWidth;
            polyline.Visibility = Visibility.Visible;
            polyline.Stroke = Brushes.Black;
        }
        private void RefreshMarkerPoints()
        {
            for (int i = 0; i < markers.Count; i++)
            {
                markers[i].SetMarkerSize(StrokeWidth);
                markers[i].Move(polyline.Points[i]);
            }
        }
        private void DefinePolyline(Polyline line)
        {
            foreach (var point in line.Points)
            {
                polyline.Points.Add(point);
            }
        }
    }
}
