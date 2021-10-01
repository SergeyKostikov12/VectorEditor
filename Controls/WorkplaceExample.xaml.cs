using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphicEditor.Controls
{
    /// <summary>
    /// Логика взаимодействия для WorkplaceExample.xaml
    /// Этот класс создан как Альтернатива Workplace.xaml.cs
    /// </summary>
    public partial class WorkplaceExample : UserControl
    {
        private Condition_ Condition = new Condition_();
        private Figure SelectedFigure;

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
            return SelectedFigure;
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
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
            WorkPlaceCanvas.Children.Remove(selectedFigure.GetShape());
            var markers = selectedFigure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Remove(marker);
            }
            AllFigures.Remove(selectedFigure);
            firstClickLMB = new Point();
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
            Point clickPosition = e.GetPosition(WorkPlaceCanvasExample);
            if (e.ClickCount == 2)
            {
                InsertPolylinePoint(clickPosition);
            }
            if (Condition.Mode == Mode.DrawRectMode)
            {
                StartDrawRectangle(clickPosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawLineMode)
            {
                StartDrawLine(clickPosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawPolyline)
            {
                StartDrawPolyline(clickPosition);
                return;
            }
            else if (Condition.Mode == Mode.DrawPolylineProcess)
            {
                AddPolylinePoint(clickPosition);
                return;
            }
            SelectFigure(clickPosition);
        }
        private void WorkPlaceCanvasExample_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;
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
            throw new NotImplementedException();
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
    }
}
