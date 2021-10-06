using GraphicEditor.Functionality;
using GraphicEditor.Functionality.Shadows;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Controls
{
    public partial class Workplace : UserControl
    {
        private WorkplaceCondition Condition;
        private ClickDispatcher ClickDispatcher;
        private Figure selectedFigure;
        private List<Figure> allFigures;

        private IDraw shadow;

        private Point leftMouseButtonDownPos;
        private Point leftMouseButtonUpPos;
        private Point rightMouseButtonDownPos;
        private Point currentMousePos;
        private Point scrollPoint;

        public Workplace()
        {
            InitializeComponent();
            Condition = new WorkplaceCondition();
            ClickDispatcher = new ClickDispatcher(Condition);
            allFigures = new List<Figure>();
            scrollPoint = new Point(0, 0);
        }

        public List<Figure> GetAllFigures()
        {
            return allFigures;
        }
        public void LoadWorkplace(List<Figure> figures)
        {
            List<Figure> fig = CloneList(figures);
            if (fig.Count == 0) return;
            ClearWorkplace();
            DeselectFigure();
            allFigures = fig;
            AddToWorkplace(fig);
            Condition.ResetCondition();
        }
        public void ReadyDrawLine()
        {
            DeselectFigure();
            Condition.DrawingMode = DrawingMode.LineMode;
            WorkPlaceCanvas.Cursor = Cursors.Cross;
        }
        public void ReadyDrawRectangle()
        {
            DeselectFigure();
            Condition.DrawingMode = DrawingMode.RectangleMode;
            WorkPlaceCanvas.Cursor = Cursors.Cross;
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
            allFigures.Remove(selectedFigure);
            leftMouseButtonDownPos = new Point();
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


        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            leftMouseButtonDownPos = e.GetPosition(WorkPlaceCanvas);
            if (e.ClickCount == 2)
            {
                ExecuteDoubleClick();
                return;
            }
            ClickDispatcher.LMB_Down(e.Timestamp);
            ExecuteMouseInput();
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightMouseButtonDownPos = e.GetPosition(WorkPlaceCanvas);
            ClickDispatcher.RMB_Down();
            ExecuteMouseInput();
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            leftMouseButtonUpPos = e.GetPosition(WorkPlaceCanvas);
            ClickDispatcher.LMB_UP(e.Timestamp);
            ExecuteMouseInput();
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.GetPosition(WorkPlaceCanvas);
            ClickDispatcher.Move();
            ExecuteMouseInput();
        }

        private void ExecuteMouseInput()
        {
            switch (Condition.DrawingMode)
            {
                case DrawingMode.StartDrawRectangle:
                    StartDrawRectangle();
                    break;
                case DrawingMode.StartDrawLine:
                    StartDrawLine();
                    break;
                case DrawingMode.DrawRectangleProcess:
                    shadow.Draw(currentMousePos);
                    break;
                case DrawingMode.DrawSingleLineProcess:
                    shadow.Draw(currentMousePos);
                    break;
                case DrawingMode.DrawPolylineProcess:
                    shadow.Draw(currentMousePos);
                    break;
                case DrawingMode.EndDrawRectangle:
                    shadow.EndDraw(leftMouseButtonUpPos);
                    CreateRectangle();
                    break;
                case DrawingMode.EndDrawSingleLine:
                    shadow.EndDraw(leftMouseButtonUpPos);
                    CreateSingleLine();
                    break;
                case DrawingMode.EndDrawPolyline:
                    shadow.EndDraw(currentMousePos);
                    CreatePolyline();
                    break;
                default:
                    break;
            }
            switch (Condition.Action)
            {
                case Action.None:
                    break;
                case Action.AddPoint:
                    shadow.AddPoint(leftMouseButtonUpPos);
                    Condition.Action = Action.None;
                    break;
                case Action.SelectFigure:
                    SelectFigure();
                    break;
                case Action.MoveMarker:
                    DragFigure(currentMousePos);
                    break;
                case Action.MoveWorkplace:
                    MoveWorkPlace();
                    break;
                case Action.RelizeMarker:
                    ExecuteRelize();
                    break;
                case Action.SelectMarker:
                    SelectMarker();
                    break;
                case Action.Deselect:
                    DeselectFigure();
                    break;
                default:
                    break;
            }
        }

        private void StartDrawLine()
        {
            shadow = new ShadowLine();
            shadow.StartDraw(leftMouseButtonDownPos);
            shadow.Show();
            WorkPlaceCanvas.Children.Add(shadow.GetShape());
        }
        private void StartDrawRectangle()
        {
            shadow = new ShadowRectangle();
            shadow.StartDraw(leftMouseButtonDownPos);
            shadow.Show();
            WorkPlaceCanvas.Children.Add(shadow.GetShape());
        }
        private void CreateRectangle()
        {
            Figure figure = new RectangleFigure(leftMouseButtonDownPos, leftMouseButtonUpPos);
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
            allFigures.Add(figure);
            shadow.Hide();
            WorkPlaceCanvas.Children.Remove(shadow.GetShape());
            Condition.ResetCondition();
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
        }
        private void CreatePolyline()
        {
            Figure figure = new LineFigure(shadow.GetShape());
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
            allFigures.Add(figure);
            shadow.Hide();
            WorkPlaceCanvas.Children.Remove(shadow.GetShape());
            Condition.ResetCondition();
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
        }
        private void CreateSingleLine()
        {
            Figure figure = new LineFigure(leftMouseButtonDownPos, leftMouseButtonUpPos);
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
            allFigures.Add(figure);
            shadow.Hide();
            WorkPlaceCanvas.Children.Remove(shadow.GetShape());
            Condition.ResetCondition();
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
        }
        private void DragFigure(Point position)
        {
            if (selectedFigure == null)
                return;
            if (!selectedFigure.IsMarkerSelect())
                return;
            selectedFigure.MoveMarker(position);
        }
        private void MoveWorkPlace()
        {
            scrollPoint.X = Scroll.HorizontalOffset - rightMouseButtonDownPos.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rightMouseButtonDownPos.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
            Condition.ResetCondition();
        }

        private void SelectMarker()
        {
            selectedFigure.SelectMarker(leftMouseButtonDownPos);
        }
        private void SelectFigure()
        {
            if (selectedFigure != null)
            {
                selectedFigure.SelectMarker(leftMouseButtonDownPos);
                Condition.IsFigureSelected = true;

                if (selectedFigure.IsMarkerSelect())
                    return;
            }

            foreach (var figure in allFigures)
            {
                if (figure.SelectLine(leftMouseButtonDownPos))
                {
                    SetSelectedFigure(figure);
                    figure.ShowOutline();
                    Condition.IsFigureSelected = true;
                }
                else
                {
                    figure.HideOutline();
                    Condition.IsFigureSelected = false;
                }
            }
        }
        private void SetSelectedFigure(Figure figure)
        {
            selectedFigure = figure;
        }
        private void DeselectFigure()
        {
            if (selectedFigure == null)
                return;

            selectedFigure.HideOutline();
            selectedFigure.DeselectFigure();
            selectedFigure = null;
            Condition.ResetCondition();
        }
        private void ExecuteDoubleClick()
        {
            Rectangle marker = selectedFigure.InsertPoint(leftMouseButtonUpPos);
            AddToWorkplace(marker);
            return;
        }
        private void ExecuteRelize()
        {
            if (selectedFigure == null)
                return;
            Condition.Action = Action.None;
            selectedFigure.ExecuteRelizeMarker(leftMouseButtonUpPos);
        }
        private void AddToWorkplace(Rectangle rect)
        {
            if (rect == null)
                return;

            WorkPlaceCanvas.Children.Add(rect);
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
        private void ClearWorkplace()
        {
            foreach (var figure in allFigures)
            {
                figure.Collapse();
            }
            WorkPlaceCanvas.Children.Clear();
            allFigures.Clear();
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
    }
}
