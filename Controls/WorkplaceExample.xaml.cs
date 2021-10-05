using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Controls
{
    /// <summary>
    /// Логика взаимодействия для WorkplaceExample.xaml
    /// Этот класс создан как Альтернатива Workplace.xaml.cs
    /// </summary>
    public partial class WorkplaceExample : UserControl
    {
        private List<Figure> AllFigures = new List<Figure>();
        private Condition_ Condition = new Condition_();
        private Figure selectedFigure;
        private Rectangle shadowRect;
        private Line shadowLine;
        private Polyline shadowPolyline;
        private Point leftMouseClickPosition;
        private Point rightMouseClickPosition;
        private Point scrollPoint = new Point(0, 0);


        public WorkplaceExample()
        {
            InitializeComponent();
        }

        public void ReadyDrawRectangle()
        {
            Condition.Mode = Mode.DrawRectMode;
        }
        public void ReadyDrawLine()
        {
            Condition.Mode = Mode.DrawLineMode;
        }
        public Figure GetSelectedFigure()
        {
            return selectedFigure;
        }
        public void LoadWorkplace(List<Figure> figures)
        {
            List<Figure> fig = CloneList(figures);
            if (fig.Count == 0) return;
            ClearWorkplace();
            DeselectFigure();
            AllFigures = fig;
            AddToWorkplace(fig);
            Condition.Reset();
        }
        public void DeleteFigure()
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выделите объект!");
                return;
            }
            WorkPlaceCanvasExample.Cursor = Cursors.Arrow;
            WorkPlaceCanvasExample.Children.Remove(selectedFigure.GetShape());
            var markers = selectedFigure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvasExample.Children.Remove(marker);
            }
            AllFigures.Remove(selectedFigure);
            leftMouseClickPosition = new Point();
            DeselectFigure();
        }
        public void SetFigureLineColor(SolidColorBrush lineColor)
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выберите объект");
                return;
            }
            selectedFigure.LineColor = lineColor;
        }
        public void SetFigureFillColor(SolidColorBrush fillColor)
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выберите объект");
                return;
            }
            selectedFigure.Fill = fillColor;
        }
        public void SetFigureWidth(int width)
        {
            if (selectedFigure == null) return;
            selectedFigure.StrokeWidth = width;
        }


        private void WorkPlaceCanvasExample_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(WorkPlaceCanvasExample);
            leftMouseClickPosition = clickPosition;
            if (e.ClickCount == 2 && selectedFigure != null)
            {
                InsertPolylinePoint(clickPosition);//+
                return;
            }

            if (Condition.Mode == Mode.DrawRectMode)
            {
                StartDrawRectangle(clickPosition);//+
                return;
            }
            else if (Condition.Mode == Mode.DrawLineMode)
            {
                StartDrawLine(clickPosition);//+
                return;
            }
            else if (Condition.Mode == Mode.DrawPolylineProcess)
            {
                AddPolylinePoint(clickPosition);//+
                return;
            }
            DeselectFigure();
            SelectFigure(clickPosition);
        }
        private void WorkPlaceCanvasExample_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(WorkPlaceCanvasExample);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (leftMouseClickPosition.Length(currentPosition) < 5) return;
                if (Condition.Mode == Mode.DrawRectProcess)
                {
                    DrawingRectangle(currentPosition);//+
                    return;
                }
                else if (Condition.Mode == Mode.DrawLineMode)
                {
                    if (shadowLine == null) return;
                    DrawingLine(currentPosition);//+
                    return;
                }
                else if (Condition.Mode == Mode.MoveMarker)
                {
                    MoveMarker(currentPosition);
                    return;
                }
            }

            if (Condition.Mode == Mode.DrawPolylineProcess)
            {
                DrawingPolyline(currentPosition);//+
                return;
            }

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                MoveWorkPlace(currentPosition);
            }
        }
        private void MoveWorkPlace(Point currentMousePos)
        {
            scrollPoint.X = ScrollExample.HorizontalOffset - rightMouseClickPosition.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = ScrollExample.VerticalOffset - rightMouseClickPosition.DeltaTo(currentMousePos).Y / 2;
            ScrollExample.ScrollToHorizontalOffset(scrollPoint.X);
            ScrollExample.ScrollToVerticalOffset(scrollPoint.Y);
        }
        private void WorkPlaceCanvasExample_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point relizePosition = e.GetPosition(WorkPlaceCanvasExample);
            if (Condition.Mode == Mode.DrawRectProcess)
            {
                EndDrawRectangle(relizePosition);//+
                return;
            }
            else if (Condition.Mode == Mode.DrawLineProcess)
            {
                EndDrawLine(relizePosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawLineMode)
            {
                RemoveShadowLine();
                StartDrawPolyline(relizePosition);//+
                return;
            }
            else if (Condition.Mode == Mode.MoveMarker)
            {
                ExecuteRelize(relizePosition);//+
            }
        }
        private void ExecuteRelize(Point relizePosition)
        {
            if (selectedFigure == null) return;
            selectedFigure.ExecuteRelize(relizePosition);
        }
        private void WorkPlaceCanvasExample_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightMouseClickPosition = e.GetPosition(WorkPlaceCanvasExample);
            if (Condition.Mode == Mode.DrawPolylineProcess)
            {
                EndDrawPolyline();//+
            }
            Condition.Reset();
        }



        private void InsertPolylinePoint(Point clickPosition)
        {
            Rectangle rect = selectedFigure.InsertPoint(clickPosition);
            AddToWorkplace(rect);
        }//+
        private void StartDrawRectangle(Point clickPosition)
        {
            leftMouseClickPosition = clickPosition;
            shadowRect = new Rectangle();
            shadowRect.Visibility = Visibility.Visible;
            shadowRect.Stroke = Brushes.Red;
            WorkPlaceCanvasExample.Children.Add(shadowRect);
            Condition.Mode = Mode.DrawRectProcess;
        }//+
        private void StartDrawLine(Point clickPosition)
        {
            shadowLine = new Line();
            shadowLine.Visibility = Visibility.Visible;
            shadowLine.Point1(clickPosition);
            shadowLine.Point2(clickPosition);
            shadowLine.Stroke = Brushes.Red;
            WorkPlaceCanvasExample.Children.Add(shadowLine);
        }//+
        private void StartDrawPolyline(Point clickPosition)
        {
            shadowPolyline = new Polyline();
            shadowPolyline.Visibility = Visibility.Visible;
            shadowPolyline.Points.Add(clickPosition);
            shadowPolyline.Points.Add(clickPosition);
            shadowPolyline.Stroke = Brushes.Red;
            WorkPlaceCanvasExample.Children.Add(shadowPolyline);
            Condition.Mode = Mode.DrawPolylineProcess;
        }//+
        private void AddPolylinePoint(Point clickPosition)
        {
            shadowPolyline.Points.Add(clickPosition);
        }//+
        private void SelectFigure(Point clickPosition)
        {
            if (selectedFigure == null)
            {
                foreach (var figure in AllFigures)
                {
                    if (figure.IsSelectMarker(clickPosition))
                    {
                        SetSelectedFigure(figure);
                        figure.ShowOutline();
                        Condition.Mode = Mode.MoveMarker;
                    }
                    else if (figure.IsSelectLine(clickPosition))
                    {
                        SetSelectedFigure(figure);
                        figure.ShowOutline();
                    }
                    else figure.HideOutline();
                }
            }
            else
            {
                selectedFigure.IsSelectMarker(clickPosition);
                Condition.Mode = Mode.MoveMarker;
            }
            Condition.Mode = Mode.None;
        }


        private void DrawingRectangle(Point currentPosition)
        {
            double xTop = Math.Max(leftMouseClickPosition.X, currentPosition.X);
            double yTop = Math.Max(leftMouseClickPosition.Y, currentPosition.Y);

            double xMin = Math.Min(leftMouseClickPosition.X, currentPosition.X);
            double yMin = Math.Min(leftMouseClickPosition.Y, currentPosition.Y);

            shadowRect.Height = yTop - yMin;
            shadowRect.Width = xTop - xMin;

            Canvas.SetLeft(shadowRect, xMin);
            Canvas.SetTop(shadowRect, yMin);
        }//+
        private void DrawingLine(Point currentPosition)
        {
            shadowLine.Point2(currentPosition);
            Condition.Mode = Mode.DrawLineProcess;
        }//+
        private void DrawingPolyline(Point currentPosition)
        {
            int lastPoint = shadowPolyline.Points.Count - 1;
            shadowPolyline.Points[lastPoint] = new Point(currentPosition.X, currentPosition.Y);
        }//+
        private void MoveMarker(Point currentPosition)
        {
            if (selectedFigure == null)
                return;
            selectedFigure.MoveMarker(currentPosition);
        }//+

        private void EndDrawRectangle(Point clickPosition)
        {
            if (leftMouseClickPosition.Length(clickPosition) < 5)
            {
                Condition.Mode = Mode.DrawRectMode;
                RemoveShadowRect();
                return;
            }
            CreateRectangle(clickPosition);
        }//+
        private void EndDrawLine(Point clickPosition)
        {
            if (leftMouseClickPosition.Length(clickPosition) < 5)
            {
                Condition.Mode = Mode.DrawLineMode;
                RemoveShadowLine();
                return;
            }
            CreateLine(clickPosition);
            RemoveShadowLine();
            Condition.Mode = Mode.DrawLineMode;
        }


        private void RemoveShadowRect()
        {
            WorkPlaceCanvasExample.Children.Remove(shadowRect);
            WorkPlaceCanvasExample.UpdateLayout();
            shadowRect = null;
        }
        private void CreateRectangle(Point clickPosition)
        {
            Figure figure = new RectangleFigure(leftMouseClickPosition, clickPosition);
            WorkPlaceCanvasExample.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvasExample.Children.Add(marker);
            }
            AllFigures.Add(figure);
            Condition.Mode = Mode.DrawRectMode;
        }


        private void CreateLine(Point clickPosition)
        {
            Figure figure = new LineFigure(leftMouseClickPosition, clickPosition);
            WorkPlaceCanvasExample.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvasExample.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }

        private void RemoveShadowLine()
        {
            WorkPlaceCanvasExample.Children.Remove(shadowLine);
            WorkPlaceCanvasExample.UpdateLayout();
            shadowLine = null;
        }

        private void EndDrawPolyline()
        {
            Condition.Mode = Mode.DrawLineMode;
            if (shadowPolyline.Points.Count == 2)
            {
                RemoveShadowPolyline();
                return;
            }
            CreatePolyline();
        }//+

        private void CreatePolyline()
        {
            Figure figure = new LineFigure(shadowPolyline);
            WorkPlaceCanvasExample.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvasExample.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }

        private void RemoveShadowPolyline()
        {
            Condition.Mode = Mode.DrawLineMode;
            WorkPlaceCanvasExample.Children.Remove(shadowPolyline);
        }

        private void AddToWorkplace(Rectangle rect)
        {
            if (rect == null) return;
            WorkPlaceCanvasExample.Children.Add(rect);
        }
        private void AddToWorkplace(List<Figure> figures)
        {
            foreach (var figure in figures)
            {
                WorkPlaceCanvasExample.Children.Add(figure.GetShape());
                var markers = figure.GetMarkers();
                foreach (var marker in markers)
                {
                    WorkPlaceCanvasExample.Children.Add(marker);
                }
            }
        }

        private void SetSelectedFigure(Figure figure)
        {
            selectedFigure = figure;
        }
        private List<Figure> CloneList(List<Figure> oldList)
        {
            List<Figure> newlist = new List<Figure>();
            foreach (var item in oldList)
            {
                newlist.Add(item);
            }
            return newlist;
        }
        private void ClearWorkplace()
        {
            WorkPlaceCanvasExample.Children.Clear();
            AllFigures.Clear();
        }
        private void DeselectFigure()
        {
            if (selectedFigure == null) return;
            selectedFigure.HideOutline();
            selectedFigure.DeselectFigure();
            selectedFigure = null;
        }

        public List<Figure> GetAllFigures()
        {
            return AllFigures;
        }
    }
}
