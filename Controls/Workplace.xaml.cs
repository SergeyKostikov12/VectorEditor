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
        private delegate void LeftMouseButtonDownEventHandler(Point position);
        private delegate void LeftMouseButtonUpEventHandler(Point position);
        private delegate void RightMouseButtonDownEventHandler(Point position);
        private delegate void LeftMouseButtonClickEventHandler(Point position);
        private delegate void LeftMouseButtonMoveEventHandler(Point position);

        private event LeftMouseButtonDownEventHandler LeftDown;
        private event LeftMouseButtonUpEventHandler LeftUp;
        private event RightMouseButtonDownEventHandler RightDown;
        private event LeftMouseButtonClickEventHandler LeftClick;
        private event LeftMouseButtonMoveEventHandler MouseMove;

        private WorkplaceCondition Condition;
        private Figure selectedFigure;
        private List<Figure> allFigures;

        private ShadowFigure shadow;

        private Point leftMouseButtonDownPos;
        private Point leftMouseButtonUpPos;
        private Point rightMouseButtonDownPos;
        private Point currentMousePos;
        private Point scrollPoint;

        public Workplace()
        {
            InitializeComponent();
            Condition = new WorkplaceCondition();
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
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            shadow = new ShadowLine();
            SetShadowEventSubscription();
        }
        public void ReadyDrawRectangle()
        {
            DeselectFigure();
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            shadow = new ShadowRectangle();
            AddToWorkplace(shadow.GetShape());
            SetShadowEventSubscription();
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

        public void DeleteFigure()
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выделите объект!");
                return;
            }
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
            WorkPlaceCanvas.Children.Remove(selectedFigure.GetShapes());
            var markers = selectedFigure.GetMarkers();
            foreach (var marker in markers)
            {
                WorkPlaceCanvas.Children.Remove(marker);
            }
            allFigures.Remove(selectedFigure);
            leftMouseButtonDownPos = new Point();
            DeselectFigure();
        }


        private void SetShadowEventSubscription()
        {
            LeftDown += shadow.LeftMouseButtonDown;
            LeftUp += shadow.LeftMouseButtonUp;
            RightDown += shadow.RightMouseButtonDown;
            MouseMove += shadow.MouseMove;
            shadow.EndDrawFigure += Shadow_EndDrawFigure;
        }//todo
        private void Shadow_EndDrawFigure(object sender)
        {
            Figure figure;
            if (sender is RectangleFigure)
            {
                var tmp = (ShadowRectangle)shadow;
                figure = new RectangleFigure(tmp.FirstPoint, tmp.LastPoint);
            }
            else if (sender is ShadowLine)
            {
                var tmp = (ShadowLine)sender;
                figure = new LineFigure(tmp.Polyline);
            }
            else return;
            SetFigureEventSubscription(figure);
            WorkPlaceCanvas.Children.Add(figure.GetShapes());
        }
        private void SetFigureEventSubscription(Figure figure)
        {
            LeftDown += figure.LeftMouseButtonDown;
            LeftUp += figure.LeftMouseButtonUp;
            RightDown += figure.RightMouseButtonDown;
            MouseMove += figure.MouseMove;
            LeftClick += figure.LeftMouseButtonClick;
            figure.SelectFigure += Figure_SelectFigure;
        }
        private void Figure_SelectFigure(Figure sender)
        {
            selectedFigure = sender;
        }


        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                LeftClick?.Invoke(e.GetPosition(WorkPlaceCanvas));
                return;
            }
            LeftDown?.Invoke(e.GetPosition(WorkPlaceCanvas));
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
            RightDown?.Invoke(e.GetPosition(WorkPlaceCanvas));
            if (shadow == null) 
                return;
            WorkPlaceCanvas.Children.Remove(shadow?.GetShape());
            shadow.EndDrawFigure -= Shadow_EndDrawFigure;
            shadow = null;
            //rightMouseButtonDownPos = e.GetPosition(WorkPlaceCanvas);
            //ClickDispatcher.RMB_Down();
            //ExecuteMouseInput();
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LeftUp?.Invoke(e.GetPosition(WorkPlaceCanvas));
            //leftMouseButtonUpPos = e.GetPosition(WorkPlaceCanvas);
            //ClickDispatcher.LMB_UP(e.Timestamp);
            //ExecuteMouseInput();
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            MouseMove?.Invoke(e.GetPosition(WorkPlaceCanvas));
            //currentMousePos = e.GetPosition(WorkPlaceCanvas);
            //ClickDispatcher.Move();
            //ExecuteMouseInput();
        }

        private void AddToWorkplace(Shape shape)
        {
            if (shape == null)
                return;

            WorkPlaceCanvas.Children.Add(shape);
        }
        private void AddToWorkplace(List<Figure> figures) // TODO: Вывод списка Shapes
        {
            foreach (var figure in figures)
            {
                WorkPlaceCanvas.Children.Add(figure.GetShapes());
                var markers = figure.GetMarkers();
                foreach (var marker in markers)
                {
                    WorkPlaceCanvas.Children.Add(marker);
                }
            }
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
        private void ExecuteRelize()
        {
            if (selectedFigure == null)
                return;
            Condition.Action = Action.None;
            selectedFigure.ExecuteRelizeMarker(leftMouseButtonUpPos);
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


        private void ExecuteDoubleClick()
        {
            Rectangle marker = selectedFigure.InsertPoint(leftMouseButtonUpPos);
            //AddToWorkplace(marker);
            return;
        }
        private void ExecuteMouseInput()
        {
            switch (Condition.DrawingMode)
            {
                //case DrawingMode.StartDrawRectangle:
                //    StartDrawRectangle();
                //    break;
                //case DrawingMode.StartDrawLine:
                //    StartDrawLine();
                //    break;
                //case DrawingMode.DrawRectangleProcess:
                //    shadow.Draw(currentMousePos);
                //    break;
                //case DrawingMode.DrawSingleLineProcess:
                //    shadow.Draw(currentMousePos);
                //    break;
                //case DrawingMode.DrawPolylineProcess:
                //    shadow.Draw(currentMousePos);
                //    break;
                //case DrawingMode.EndDrawRectangle:
                //    shadow.EndDraw(leftMouseButtonUpPos);
                //    CreateRectangle();
                //    break;
                //case DrawingMode.EndDrawSingleLine:
                //    shadow.EndDraw(leftMouseButtonUpPos);
                //    CreateSingleLine();
                //    break;
                //case DrawingMode.EndDrawPolyline:
                //    shadow.EndDraw(currentMousePos);
                //    CreatePolyline();
                //    break;
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
            WorkPlaceCanvas.Children.Add(figure.GetShapes());
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
            WorkPlaceCanvas.Children.Add(figure.GetShapes());
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
            WorkPlaceCanvas.Children.Add(figure.GetShapes());
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
    }
}
