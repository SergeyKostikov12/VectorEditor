using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicEditor
{
    /// <summary>
    /// Логика взаимодействия для Workplace.xaml
    /// </summary>
    public partial class Workplace : Page
    {
        private WorkplaceShadow Shadow;
        private WorkplaceCondition Condition;
        private Figure selectedFigure;
        public List<Figure> AllFigures = new List<Figure>();
        private Point firstClickLMB;
        private Point firstClickRMB;
        private Point scrollPoint = new Point(0, 0);


        public Workplace(WorkplaceCondition condition)
        {
            Condition = condition;
            InitializeComponent();
            Shadow = new WorkplaceShadow(WorkPlaceCanvas);
        }

        public Figure GetSelectedFigure()
        {
            return selectedFigure;
        }
        public void LoadWorkplace(List<Figure> figures)
        {
            if (figures.Count == 0) return;
            DeselectFigure();
            ClearCanvas();
            AllFigures = figures;
            AddToWorkplace(figures);
        }
        public List<Figure> GetAllFigures()
        {
            return AllFigures;
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
        public void StartDrawRectangle()
        {
            
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            DeselectFigure();
        }
        public void StartDrawLine()
        {
            DeselectFigure();
            WorkPlaceCanvas.Cursor = Cursors.Cross;
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
            if (selectedFigure != null)
            {
                selectedFigure.Fill = fillColor;
            }
            else
            {
                MessageBox.Show("Сначала выберите объект");
            }

        }

        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(WorkPlaceCanvas);
            if (e.ClickCount == 2)
            {
                ExecuteDoubleClick(clickPosition);
                return;
            }
            MouseLeftButtonDownClick(clickPosition);
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            firstClickRMB = e.GetPosition(WorkPlaceCanvas);
            MouseRightButtonDownClick();
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePos = e.GetPosition(WorkPlaceCanvas);

            Move(currentMousePos);

        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition(WorkPlaceCanvas);
            Condition.MouseUp = true;

            if (Condition.Action == Actions.DrawRect)
            {
                Condition.Action = Actions.None;
                CreateRect(endPoint);
                firstClickLMB = new Point();
                Shadow.Clear();
            }
            else if (Condition.Action == Actions.DrawLine)
            {
                if (Condition.IsDrawLine())
                {
                    if (Shadow.GetShadowLine().Points.Count <= 2)
                    {
                        Condition.Action = Actions.None;
                        CreateLine(firstClickLMB, endPoint);
                        firstClickLMB = new Point();
                        Shadow.Clear();
                    }
                    Condition.ResetMouseState();
                }
                else
                {
                    Shadow.AddPoint(endPoint);
                }
            }
            else if (Condition.Action == Actions.MovePoint)
            {
                ExecuteRelize(endPoint);
            }

        }

        private void ExecuteRelize(Point endPoint)
        {
            if (selectedFigure == null) return;
            selectedFigure.ExecuteRelize(endPoint);
        }
        private void ExecuteDoubleClick(Point clickPosition) 
        {
            Rectangle rect = selectedFigure.ExecuteDoubleClick(clickPosition);
            AddToWorkplace(rect);
        }
        private void MouseLeftButtonDownClick(Point point)
        {
            Point clickPosition = point;
            if(firstClickLMB.X == 0 && firstClickLMB.Y == 0)
            {
                firstClickLMB = point;
            }
            Condition.MouseDown = true;
            if (Condition.ButtonPressed == ButtonPressed.Rect)
            {
                Condition.Action = Actions.DrawRect;
                Shadow.StartDrawRectShadow(clickPosition);
                SetLMB_ClickPosition(clickPosition);
            }
            else if (Condition.ButtonPressed == ButtonPressed.Line)
            {
                Condition.Action = Actions.DrawLine;
                if (Shadow.GetShadowLine().Points.Count == 0)
                {
                    Shadow.DrawLineShadow();
                    Shadow.SetLIneFirstPoint(firstClickLMB);
                    Shadow.SetLineSecondPoint(clickPosition);
                }
            }
            else if (Condition.ButtonPressed == ButtonPressed.None)
            {
                Condition.Action = DetermindAction(clickPosition);
            }
        }
        private void MouseRightButtonDownClick()
        {
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
            DeselectFigure();
            if (Condition.Action == Actions.DrawLine)
            {
                Shadow.RemoveLastPoint();
                CreatePolyline(Shadow.GetShadowLine());
                firstClickLMB = new Point();
                Shadow.Clear();
            }
            Condition.ResetCondition();
        }
        private void Move(Point currentMousePos)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (Condition.Action == Actions.DrawRect)
                {
                    Shadow.DrawRectShadow(currentMousePos);
                }
                else if (Condition.Action == Actions.DrawLine)
                {
                    if (AlllowDistance(currentMousePos))
                    {
                        Condition.MouseDrag = true;
                    }
                    Shadow.DrawLastPointShadowtLine(currentMousePos);
                }
                else if (Condition.Action == Actions.MovePoint)
                {
                    Drag(currentMousePos);
                }
            }

            if (Condition.Action == Actions.DrawLine)
            {
                Shadow.DrawLastPointShadowtLine(currentMousePos);
            }

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                MovingWorkPlace(Scroll, firstClickRMB, currentMousePos);
            }
        }
        private void Drag(Point position)
        {
            if (selectedFigure != null)
            {
                selectedFigure.MoveMarker(position);
            }
        }
        private void AddToWorkplace(List<Figure> figures)
        {
            foreach (var figure in figures)
            {
                WorkPlaceCanvas.Children.Add(figure.GetShape());
                var markers = figure.GetMarkers();
                foreach (var marker in markers)
                {
                    WorkPlaceCanvas.Children.Add(marker);
                }
            }
        }
        private void SetLMB_ClickPosition(Point LMB_ClickPositionPoint)
        {
            firstClickLMB = LMB_ClickPositionPoint;
        }
        private void MovingWorkPlace(ScrollViewer Scroll, Point rMB_firstPoint, Point currentMousePos)
        {
            scrollPoint.X = Scroll.HorizontalOffset - rMB_firstPoint.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rMB_firstPoint.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
        }
        private void DeselectFigure()
        {
            if (selectedFigure == null) return;
            selectedFigure.HideOutline();
            selectedFigure.DeselectFigure();
            selectedFigure = null;
        }
        private void AddToWorkplace(Rectangle rect)
        {
            if (rect == null) return;
            WorkPlaceCanvas.Children.Add(rect);
        }
        private void PlacingInWorkPlace(Figure figure)
        {
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
        }
        private void SetSelectedFigure(Figure figure)
        {
            selectedFigure = figure;
        }
        private void CreateRect(Point endPoint)
        {
            Figure figure = new RectangleFigure(firstClickLMB, endPoint);
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }
        private void CreateLine(Point firstPoint, Point endPoint)
        {
            Figure figure = new LineFigure(firstPoint, endPoint);
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }
        private void CreatePolyline(Polyline shadowLine)
        {
            shadowLine.Points.RemoveAt(0);
            Figure figure = new LineFigure(shadowLine);
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }
        private void ClearCanvas()
        {
            WorkPlaceCanvas.Children.Clear();
            AllFigures.Clear();
        }
        private Actions DetermindAction(Point clickPosition)
        {
            if (selectedFigure == null)
            {
                foreach (var figure in AllFigures)
                {
                    if (figure.SelectMarker(clickPosition) == true)
                    {
                        SetSelectedFigure(figure);
                        figure.ShowOutline();
                        return Actions.Ready;
                    }
                    else if (figure.SelectLine(clickPosition) == true)
                    {
                        SetSelectedFigure(figure);
                        figure.ShowOutline();
                        return Actions.Ready;
                    }
                    else figure.HideOutline();
                }
            }
            else
            {
                selectedFigure.SelectMarker(clickPosition);
                return Actions.MovePoint;
            }

            return Actions.None;
        }
        private bool AlllowDistance(Point currentMousePos)
        {
            Point delta = firstClickLMB.AbsDeltaTo(currentMousePos);
            if (delta.X > 5 || delta.Y > 5)
            {
                return true;
            }
            else return false;
        }

       // private Rectangle ExecuteDoubleClick(Point point)
        //{
         //   if (selectedFigure == null) return null;

       //     Rectangle rect = selectedFigure.ExecuteDoubleClick(point);
       //     return rect;
      //  }
    }
}
