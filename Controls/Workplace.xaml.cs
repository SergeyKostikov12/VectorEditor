using GraphicEditor.Functionality;
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
        private WorkplaceShadow shadow;
        private WorkplaceCondition condition;
        private ClickDispatcher ClickDispatcher;
        private Figure selectedFigure;
        private List<Figure> allFigures = new List<Figure>();
        //private Point firstClickLMB;
        //private Point firstClickRMB;
        private Point scrollPoint = new Point(0, 0);

        private IDraw shadowX;

        private Point leftMouseButtonDownPos;
        private Point leftMouseButtonUpPos;
        private Point rightMouseButtonDownPos;
        private Point currentMousePos;

        private int timeDown;
        private int timeUp;

        public Workplace()
        {
            InitializeComponent();
            shadow = new WorkplaceShadow(WorkPlaceCanvas);
            condition = new WorkplaceCondition();
            ClickDispatcher = new ClickDispatcher(condition);
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
            condition.ResetCondition();
        }
        //public void ReadyDrawLine()
        //{
        //    condition.DrawingMode = DrawingMode.DrawLineMode;
        //    WorkPlaceCanvas.Cursor = Cursors.Cross;
        //    DeselectFigure();
        //}
        //public void ReadyDrawRectangle()
        //{
        //    condition.DrawingMode = DrawingMode.DrawRectMode;
        //    WorkPlaceCanvas.Cursor = Cursors.Cross;
        //    DeselectFigure();
        //}
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
        public Figure GetSelectedFigure()
        {
            return selectedFigure;
        }
        public int GetFigureLineWidth()
        {
            return selectedFigure.StrokeWidth;
        }


        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Point clickPosition = e.GetPosition(WorkPlaceCanvas);
            leftMouseButtonDownPos = e.GetPosition(WorkPlaceCanvas);
            ClickDispatcher.LMB_Down(e.Timestamp);
            ExecuteMouseInput();

            //condition.MouseDown = true;
            //if (e.ClickCount == 2 && selectedFigure != null)
            //{
            //    Rectangle marker = selectedFigure.InsertPoint(clickPosition);
            //    AddToWorkplace(marker);
            //    return;
            //}

            //if (firstClickLMB.X == 0 && firstClickLMB.Y == 0)
            //{
            //    firstClickLMB = clickPosition;
            //}
            //if (condition.DrawingMode == DrawingMode.DrawRectMode)
            //{
            //    condition.Action = Actions.DrawRect;
            //    shadow.StartDrawRectShadow(clickPosition);
            //    firstClickLMB = clickPosition;
            //}
            //else if (condition.DrawingMode == DrawingMode.DrawLineMode)
            //{
            //    condition.Action = Actions.DrawLine;
            //    if (shadow.GetShadowLine().Points.Count == 0)
            //    {
            //        shadow.DrawLineShadow();
            //        shadow.SetLIneFirstPoint(firstClickLMB);
            //        shadow.SetLineSecondPoint(clickPosition);
            //    }
            //}
            //else if (condition.DrawingMode == DrawingMode.None)
            //{
            //    condition.Action = DetermindAction(clickPosition);
            //}
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //firstClickRMB = e.GetPosition(WorkPlaceCanvas);
            rightMouseButtonDownPos = e.GetPosition(WorkPlaceCanvas);
            ClickDispatcher.RMB_Down();
            ExecuteMouseInput();

            //WorkPlaceCanvas.Cursor = Cursors.Arrow;
            //DeselectFigure();
            //if (condition.Action == Action.DrawLine)
            //{
            //    shadow.RemoveLastPoint();
            //    CreatePolyline(shadow.GetShadowLine());
            //    firstClickLMB = new Point();
            //    shadow.Clear();
            //}
            //condition.ResetCondition();
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Point endPoint = e.GetPosition(WorkPlaceCanvas);
            leftMouseButtonUpPos = e.GetPosition(WorkPlaceCanvas);
            ClickDispatcher.LMB_UP(e.Timestamp);
            ExecuteMouseInput();


            //condition.MouseUp = true;
            //if (condition.Action == Action.DrawRect)
            //{
            //    condition.Action = Action.None;
            //    CreateRectangle(endPoint);
            //    firstClickLMB = new Point();
            //    shadow.Clear();
            //}
            //else if (condition.Action == Action.DrawLine)
            //{
            //    if (condition.IsDrawingLine())
            //    {
            //        condition.ResetMouseState();

            //        if (shadow.GetShadowLine().Points.Count > 2)
            //            return;

            //        condition.Action = Action.None;
            //        CreateLine(firstClickLMB, endPoint);
            //        firstClickLMB = new Point();
            //        shadow.Clear();
            //    }
            //    else
            //    {
            //        shadow.AddPoint(endPoint);
            //    }
            //}
            //else if (condition.Action == Action.MovePoint)
            //{
            //    ExecuteRelize(endPoint);
            //}

        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.GetPosition(WorkPlaceCanvas);
            ClickDispatcher.Move();
            ExecuteMouseInput();


            //if (Mouse.LeftButton == MouseButtonState.Pressed)
            //{
            //    if (condition.Action == Action.DrawRect)
            //    {
            //        shadow.DrawRectShadow(currentMousePos);
            //    }
            //    else if (condition.Action == Action.DrawLine)
            //    {
            //        if (AllowDistance(currentMousePos))
            //        {
            //            condition.MouseDrag = true;
            //        }
            //        shadow.DrawLastPointShadowtLine(currentMousePos);
            //    }
            //    else if (condition.Action == Action.MovePoint)
            //    {
            //        DragFigure(currentMousePos);
            //    }
            //}

            //if (condition.Action == Action.DrawLine)
            //{
            //    shadow.DrawLastPointShadowtLine(currentMousePos);
            //}

            //if (Mouse.RightButton == MouseButtonState.Pressed)
            //{
            //    MoveWorkPlace(Scroll, firstClickRMB, currentMousePos);
            //}
        }
        private void ExecuteMouseInput()
        {
            switch (condition.DrawingMode)
            {
                case DrawingMode.None:
                    break;
                case DrawingMode.RectangleMode:
                    break;
                case DrawingMode.LineMode:
                    break;
                case DrawingMode.StartDrawRectangle:
                    shadowX = new ShadowRect();//
                    break;
                case DrawingMode.StartDrawLine:
                    shadowX = new ShadowRect();//
                    break;
                case DrawingMode.DrawRectangleProcess:
                    shadowX.Draw();//
                    break;
                case DrawingMode.DrawSingleLineProcess:
                    break;
                case DrawingMode.DrawPolylineProcess:
                    break;
                case DrawingMode.EndDrawRectangle:
                    break;
                case DrawingMode.EndDrawSingleLine:
                    break;
                case DrawingMode.EndDrawPolyline:
                    break;
                default:
                    break;
            }
            switch (condition.Action)
            {
                case Action.None:
                    break;
                case Action.AddPoint:
                    break;
                case Action.SelectFigure:
                    break;
                case Action.MoveMarker:
                    break;
                case Action.MoveWorkplace:
                    break;
                default:
                    break;
            }
        }

        private void CreateRectangle(Point endPoint)
        {
            Figure figure = new RectangleFigure(firstClickLMB, endPoint);
            WorkPlaceCanvas.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Add(marker);
            }
            allFigures.Add(figure);
        }
        private void ExecuteRelize(Point endPoint)
        {
            if (selectedFigure == null)
                return;

            selectedFigure.ExecuteRelizeMarker(endPoint);
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
        private bool AllowDistance(Point currentMousePos)
        {
            Point delta = firstClickLMB.AbsDeltaTo(currentMousePos);
            return delta.X > 5 || delta.Y > 5;
        }
        private void DragFigure(Point position)
        {
            if (selectedFigure == null)
                return;

            selectedFigure.MoveMarker(position);
        }
        private void DeselectFigure()
        {
            if (selectedFigure == null)
                return;

            selectedFigure.HideOutline();
            selectedFigure.DeselectFigure();
            selectedFigure = null;
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
            allFigures.Add(figure);
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
            allFigures.Add(figure);
        }
        private void MoveWorkPlace(ScrollViewer Scroll, Point rMB_firstPoint, Point currentMousePos)
        {
            scrollPoint.X = Scroll.HorizontalOffset - rMB_firstPoint.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rMB_firstPoint.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
        }
        private void SetSelectedFigure(Figure figure)
        {
            selectedFigure = figure;
        }
        private void ClearWorkplace()
        {
            WorkPlaceCanvas.Children.Clear();
            allFigures.Clear();
        }
        private Action DetermindAction(Point clickPosition)
        {
            if (selectedFigure != null)
            {
                selectedFigure.SelectMarker(clickPosition);
                return Action.MovePoint;
            }

            foreach (var figure in allFigures)
            {
                if (figure.SelectMarker(clickPosition) == true)
                {
                    SetSelectedFigure(figure);
                    figure.ShowOutline();
                    return Action.Ready;
                }
                else if (figure.SelectLine(clickPosition) == true)
                {
                    SetSelectedFigure(figure);
                    figure.ShowOutline();
                    return Action.Ready;
                }
                else figure.HideOutline();
            }
            return Action.None;
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
