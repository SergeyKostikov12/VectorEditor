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
        private Canvas workplace;
        private ScrollViewer scroll;
        private WorkplaceShadow Shadow;
        private WorkplaceCondition Condition = new WorkplaceCondition();
        private FigureProcess FigureProcess;
        public List<Figure> AllFigures = new List<Figure>();
        private Point firstClickLMB;
        private Point firstClickRMB;
        private Figure selectedFigure;

        internal object GetFigures()
        {
            throw new NotImplementedException();
        }

        private Point scrollPoint = new Point(0, 0);

        //Workplace_Class workplace;
        public Workplace()
        {
            InitializeComponent();
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
                FigureProcess.ExecuteRelize(endPoint);
            }

        }



        internal void ExecuteDoubleClick(Point clickPosition)
        {


            Rectangle rect = selectedFigure.ExecuteDoubleClick(clickPosition);
            AddToWorkplace(rect);
        }
        internal void MouseLeftButtonDownClick(Point point)
        {
            Point clickPosition = point;
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
                    Shadow.SetLIneFirstPoint(clickPosition);
                    Shadow.SetLineSecondPoint(clickPosition);
                }
            }
            else if (Condition.ButtonPressed == ButtonPressed.None)
            {
                Condition.Action = DetermindAction(clickPosition);
            }
        }
        internal void MouseRightButtonDownClick()
        {
            workplace.Cursor = Cursors.Arrow;
            DeselectFigure();
            if (Condition.Action == Actions.DrawLine)
            {
                Shadow.RemoveLastPoint();
                CreatePolyline(Shadow.GetShadowLine());
                Shadow.Clear();
            }
            Condition.ResetCondition();
        }
        internal void Move(Point currentMousePos)
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
                    FigureProcess.Drag(currentMousePos);
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


        internal void LoadWorkplace()
        {
            Condition.ButtonPressed = ButtonPressed.Load;
            Serializator serializator = new Serializator();
            serializator.Load();
            var list = serializator.GetFiguresList();
            if (list.Count == 0) return;
            DeselectFigure();
            ClearCanvas();
            AllFigures = list;
            AddToWorkplace(list);
        }

        private void AddToWorkplace(List<Figure> figures)
        {
            foreach (var figure in figures)
            {
                workplace.Children.Add(figure.GetShape());
                var markers = figure.GetMarkers();
                foreach (var marker in markers)
                {
                    workplace.Children.Add(marker);
                }
            }
        }

        internal void SaveWorkplace()
        {
            Condition.ButtonPressed = ButtonPressed.Save;
            DeselectFigure();
            Serializator serializator = new Serializator();
            serializator.Save(AllFigures);
        }

        internal void StartDrawLine()///
        {
            Condition.ButtonPressed = ButtonPressed.Line;
            DeselectFigure();
            workplace.Cursor = Cursors.Cross;
        }

        internal void StartDrawRectangle()
        {
            Condition.ButtonPressed = ButtonPressed.Rect;
            workplace.Cursor = Cursors.Cross;
            DeselectFigure();
        }

        internal void SetLMB_ClickPosition(Point LMB_ClickPositionPoint)
        {
            firstClickLMB = LMB_ClickPositionPoint;
        }

        internal void MovingWorkPlace(ScrollViewer Scroll, Point rMB_firstPoint, Point currentMousePos)
        {
            scrollPoint.X = Scroll.HorizontalOffset - rMB_firstPoint.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rMB_firstPoint.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
        }

        internal void DeselectFigure()
        {
            if (selectedFigure == null) return;
            selectedFigure.HideOutline();
            selectedFigure.DeselectFigure();
            selectedFigure = null;
        }

        internal void AddToWorkplace(Rectangle rect)
        {
            if (rect == null) return;
            workplace.Children.Add(rect);
        }

        private void PlacingInWorkPlace(Figure figure)
        {
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
        }
        private void SetSelectedFigure(Figure figure)
        {
            selectedFigure = figure;
        }
        internal void DeleteFigure()
        {
            if (selectedFigure == null)
            {
                MessageBox.Show("Сначала выделите объект!");
                return;
            }
            workplace.Cursor = Cursors.Arrow;
            workplace.Children.Remove(selectedFigure.GetShape());
            var markers = selectedFigure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Remove(marker);
            }
            AllFigures.Remove(selectedFigure);
            DeselectFigure();
        }
        internal void CreateRect(Point endPoint)
        {
            Figure figure = new RectangleFigure(firstClickLMB, endPoint);
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }
        internal void CreateLine(Point firstPoint, Point endPoint)
        {
            Figure figure = new LineFigure(firstPoint, endPoint);
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
            //AllFigures.Add(figure);
        }
        internal void CreatePolyline(Polyline shadowLine)
        {
            shadowLine.Points.RemoveAt(0);
            Figure figure = new LineFigure(shadowLine);
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }
        internal Actions DetermindAction(Point clickPosition)
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
        public Figure GetSelectedFigure()
        {
            return selectedFigure;
        }
        public void ClearCanvas()
        {
            workplace.Children.Clear();
            AllFigures.Clear();
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
    }
}
