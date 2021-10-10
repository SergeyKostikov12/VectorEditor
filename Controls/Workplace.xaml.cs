using GraphicEditor.Functionality;
using GraphicEditor.Functionality.Shadows;
using System;
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
        private delegate void MouseMoveEventHandler(Point position);

        private event LeftMouseButtonDownEventHandler LeftDown;
        private event LeftMouseButtonUpEventHandler LeftUp;
        private event RightMouseButtonDownEventHandler RightDown;
        private event LeftMouseButtonClickEventHandler LeftClick;
        private event MouseMoveEventHandler MouseMoveEvent;

        private ShadowFigure shadow;
        private Figure selectedFigure;
        private List<Figure> allFigures;
        private DrawingMode drawingMode;

        private Point rightMouseButtonDownPos = new Point(0, 0);
        private Point currentMousePos = new Point(0, 0);
        private Point scrollPoint = new Point(0, 0);

        public Workplace()
        {
            InitializeComponent();
            allFigures = new List<Figure>();
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
            SetFigureListEventSubscription(fig);
        }


        public void ReadyDrawLine()
        {
            DeselectFigure();
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            drawingMode = DrawingMode.LineMode;
            shadow = new LineShadow();
            AddToWorkplace(shadow.GetShape());
            SetShadowEventSubscription();
        }
        public void ReadyDrawRectangle()
        {
            DeselectFigure();
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            drawingMode = DrawingMode.RectangleMode;
            shadow = new RectangleShadow();
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
            RemoveFromWorkplace();
            DeselectFigure();
        }

        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                LeftClick?.Invoke(e.GetPosition(WorkPlaceCanvas));
                return;
            }
            if (shadow == null && drawingMode == DrawingMode.RectangleMode)
            {
                ReadyDrawRectangle();
            }
            else if(shadow == null && drawingMode == DrawingMode.LineMode)
            {
                ReadyDrawLine();
            }
            LeftDown?.Invoke(e.GetPosition(WorkPlaceCanvas));
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightMouseButtonDownPos = e.GetPosition(WorkPlaceCanvas);
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
            drawingMode = DrawingMode.None;
            RightDown?.Invoke(e.GetPosition(WorkPlaceCanvas));
            RemoveShadow();
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LeftUp?.Invoke(e.GetPosition(WorkPlaceCanvas));
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.GetPosition(WorkPlaceCanvas);
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                MoveWorkPlace();
                return;
            }
            MouseMoveEvent?.Invoke(e.GetPosition(WorkPlaceCanvas));
        }

        private void Shadow_EndDrawShadow(object sender)
        {
            Figure figure;
            if (sender is RectangleShadow)
            {
                var tmp = (RectangleShadow)shadow;
                figure = new RectangleFigure(tmp.FirstPoint, tmp.LastPoint);
            }
            else if (sender is LineShadow)
            {
                var tmp = (LineShadow)sender;
                figure = new LineFigure(tmp.Polyline);
            }
            else return;
            RemoveShadow();
            allFigures.Add(figure);
            SetFigureEventSubscription(figure);
            AddToWorkplace(figure.GetShapes());
        }
        private void Figure_SelectFigure(Figure sender)
        {
            if (selectedFigure == null || sender == selectedFigure)
            {
                selectedFigure = sender;
                return;
            }
            selectedFigure = null;
            sender.Deselect();
        }
        private void Figure_AddAdditionalElement(Shape element)
        {
            AddToWorkplace(element);
        }

        private void SetShadowEventSubscription()
        {
            LeftDown += shadow.LeftMouseButtonDown;
            LeftUp += shadow.LeftMouseButtonUp;
            RightDown += shadow.RightMouseButtonDown;
            MouseMoveEvent += shadow.MouseMove;
            shadow.EndDrawShadodw += Shadow_EndDrawShadow;
        }
        private void SetFigureListEventSubscription(List<Figure> figures)
        {
            foreach (var figure in figures)
            {
                SetFigureEventSubscription(figure);
            }
        }
        private void SetFigureEventSubscription(Figure figure)
        {
            LeftDown += figure.LeftMouseButtonDown;
            LeftUp += figure.LeftMouseButtonUp;
            RightDown += figure.RightMouseButtonDown;
            MouseMoveEvent += figure.MouseMove;
            LeftClick += figure.LeftMouseButtonClick;
            figure.SelectFigure += Figure_SelectFigure;
            figure.AddAdditionalElement += Figure_AddAdditionalElement;
        }
        private void AddToWorkplace(Shape shape)
        {
            if (shape == null)
                return;

            WorkPlaceCanvas.Children.Add(shape);
        }
        private void AddToWorkplace(List<Figure> figures)
        {
            foreach (var figure in figures)
            {
                AddToWorkplace(figure.GetShapes());
            }
        }
        private void AddToWorkplace(List<Shape> shapes)
        {
            foreach (var shape in shapes)
            {
                WorkPlaceCanvas.Children.Add(shape);
            }
        }
        private void RemoveShadow()
        {
            if (shadow == null)
                return;
            WorkPlaceCanvas.Children.Remove(shadow?.GetShape());
            LeftDown -= shadow.LeftMouseButtonDown;
            LeftUp -= shadow.LeftMouseButtonUp;
            RightDown -= shadow.RightMouseButtonDown;
            MouseMoveEvent -= shadow.MouseMove;
            shadow.EndDrawShadodw -= Shadow_EndDrawShadow;
            shadow = null;
        }
        private void MoveWorkPlace()
        {
            scrollPoint.X = Scroll.HorizontalOffset - rightMouseButtonDownPos.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rightMouseButtonDownPos.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
            //Condition.ResetCondition();
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
        private void DeselectFigure()
        {
            if (selectedFigure == null)
                return;

            selectedFigure.HideOutline();
            selectedFigure.Deselect();
            selectedFigure = null;
            //Condition.ResetCondition();
        }
        private void RemoveFromWorkplace()
        {
            var shapes = selectedFigure.GetShapes();
            foreach (var shape in shapes)
            {
                WorkPlaceCanvas.Children.Remove(shape);
            }
            allFigures.Remove(selectedFigure);
        }
    }
}
