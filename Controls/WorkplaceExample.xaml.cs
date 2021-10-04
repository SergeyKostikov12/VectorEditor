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
        private Point leftClickPosition;
        private Point RightClickPosition;

        private Rectangle rectangleShadow;
        private Line lineShadow;
        private Polyline polylineShadow;

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
            WorkPlaceCanvasExample = new Point();
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
            Condition.Click(MouseAction.LeftButtonDown);
            leftClickPosition = e.GetPosition(WorkPlaceCanvasExample);
            if (e.ClickCount == 2)
            {
                InsertPolylinePoint(leftClickPosition);
            }

            if (Condition.Mode == Mode.DrawRectMode)
            {
                StartDrawRectangle(leftClickPosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawLineMode)
            {
                StartDrawLine(leftClickPosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawPolyline)
            {
                StartDrawPolyline(leftClickPosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawPolylineProcess)
            {
                AddPolylinePoint(leftClickPosition);
                return;
            }
            SelectFigure(leftClickPosition);
        }
        private void WorkPlaceCanvasExample_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Condition.Click(MouseAction.Move);
                Point currentPosition = e.GetPosition(WorkPlaceCanvasExample);
                if (Condition.Mode == Mode.DrawRectMode)
                {
                    DrawingRectangle(currentPosition);
                }
                else if (Condition.Mode == Mode.DrawLineMode)
                {
                    DrawingLine(currentPosition);
                }
                else if (Condition.Mode == Mode.DrawPolylineProcess)
                {
                    DrawingPolyline(currentPosition);
                }
                else if (Condition.Mode == Mode.MoveMarker)
                {
                    MovingMarker(currentPosition);
                }
            }

        }
        private void WorkPlaceCanvasExample_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Condition.Click(MouseAction.LeftButtonUp);
            Point clickPosition = e.GetPosition(WorkPlaceCanvasExample);
            if (Condition.Mode == Mode.DrawRectMode)
            {
                EndDrawRectangle(clickPosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawLineMode)
            {
                EndDrawLine(clickPosition);
                return;
            }
        }
        private void WorkPlaceCanvasExample_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Condition.Click(MouseAction.RightButtonDown);
            if (Condition.Mode == Mode.DrawPolylineProcess)
            {
                EndDrawPolyline();
            }
            Condition.Reset();
        }


        private void InsertPolylinePoint(Point clickPosition)
        {
            throw new NotImplementedException();
        }
        private void StartDrawRectangle(Point clickPosition)
        {
            Condition.Mode = Mode.DrawRectProcess;
            InitializeRectShadow(clickPosition);
        }

        private void InitializeRectShadow(Point clickPosition)
        {
            rectangleShadow = new Rectangle();
            rectangleShadow.Stroke = Brushes.Blue;
            
        }

        private void StartDrawLine(Point clickPosition)
        {
            throw new NotImplementedException();
        }
        private void StartDrawPolyline(Point clickPosition)
        {
            throw new NotImplementedException();
        }
        private void AddPolylinePoint(Point clickPosition)
        {
            throw new NotImplementedException();
        }
        private void SelectFigure(Point clickPosition)
        {
            throw new NotImplementedException();
        }


        private void DrawingRectangle(Point currentPosition)
        {
            throw new NotImplementedException();
        }
        private void DrawingLine(Point currentPosition)
        {
            throw new NotImplementedException();
        }
        private void DrawingPolyline(Point currentPosition)
        {
            throw new NotImplementedException();
        }
        private void MovingMarker(Point currentPosition)
        {
            throw new NotImplementedException();
        }


        private void EndDrawRectangle(Point clickPosition)
        {
            throw new NotImplementedException();
        }
        private void EndDrawLine(Point clickPosition)
        {
            throw new NotImplementedException();
        }

        private void EndDrawPolyline()
        {
            throw new NotImplementedException();
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
    }
}
