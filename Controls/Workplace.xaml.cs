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
        private delegate void LeftMouseButtonUpEventHandler(Point position);
        private delegate void RightMouseButtonDownEventHandler(Point position);
        private delegate void LeftMouseButtonClickEventHandler(Point position);

        private event LeftMouseButtonUpEventHandler LeftUp;
        private event RightMouseButtonDownEventHandler RightDown;
        private event LeftMouseButtonClickEventHandler LeftClick;

        private Shadow shadow;
        private Figure selectedFigure;
        private DrawingMode drawingMode;
        private List<Figure> allFigures = new List<Figure>();

        private Point rightMouseButtonDownPos = new Point(0, 0);
        private Point currentMousePos = new Point(0, 0);
        private Point scrollPoint = new Point(0, 0);

        public Workplace()
        {
            InitializeComponent();
        }

        public List<Figure> GetAllFigures()
        {
            return allFigures;
        }
        public void LoadWorkplace(List<Figure> figures) 
        {
            if (figures.Count == 0) return;
            ClearWorkplace();
            DeselectFigure();
            allFigures = figures;
            AddToWorkplace(figures);
            SetFigureListEventSubscription(figures);
        }

        public void ReadyDrawLine()
        {
            DeselectFigure();
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            drawingMode = DrawingMode.LineMode;
            CreateShadow();
        }
        public void ReadyDrawRectangle()
        {
            DeselectFigure();
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            drawingMode = DrawingMode.RectangleMode;
            CreateShadow();
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
            var position = e.GetPosition(WorkPlaceCanvas);

            if (e.ClickCount == 2)
            {
                LeftClick?.Invoke(position);
                return;
            }

            if (shadow == null && drawingMode != DrawingMode.None)
            {
                CreateShadow();
            }

            if(shadow != null)
            {
                shadow.LeftMouseButtonDown(position);
                return;
            }
            if (selectedFigure != null)
            {
                selectedFigure.LeftMouseButtonDown(position);
                return;
            }

            foreach (var figure in allFigures)
            {
                figure.LeftMouseButtonDown(position);
                if (selectedFigure != null && figure == selectedFigure)
                    return;
            }
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightMouseButtonDownPos = e.GetPosition(WorkPlaceCanvas);
            WorkPlaceCanvas.Cursor = Cursors.Arrow;
            drawingMode = DrawingMode.None;

            if(shadow != null)
            {
                shadow.RightMouseButtonDown(e.GetPosition(WorkPlaceCanvas));
                return;
            }

            RightDown?.Invoke(e.GetPosition(WorkPlaceCanvas));
            selectedFigure = null;
            RemoveShadow();
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(shadow != null)
            {
                shadow.LeftMouseButtonUp(e.GetPosition(WorkPlaceCanvas));
                return;
            }

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
            if(shadow != null)
            {
                shadow.MouseMove(e.GetPosition(WorkPlaceCanvas));
                return;
            }
            selectedFigure?.MouseMove(e.GetPosition(WorkPlaceCanvas));
        }

        private void CreateShadow()
        {
            shadow = Shadow.Create(drawingMode);
            AddToWorkplace(shadow.GetShape());
            SetShadowEventSubscription();
        }
        private void Shadow_EndDrawShadow(Figure figure)
        {
            Figure fig = figure;
            RemoveShadow();
            allFigures.Add(fig);
            SetFigureEventSubscription(fig);
            AddToWorkplace(fig.GetShapes());
        }
        private void Figure_SelectFigure(Figure sender)
        {
            if (selectedFigure == null)
            {
                selectedFigure = sender;
                sender.ShowOutline();
                UpZPosition(sender);
                return;
            }
            sender.Deselect();
            selectedFigure = null;
        }

        private void UpZPosition(Figure sender)
        {
            allFigures.MoveToLast(sender);
            var shapes = sender.GetShapes();
            foreach (var shape in shapes)
            {
                WorkPlaceCanvas.Children.Remove(shape);
                WorkPlaceCanvas.Children.Add(shape);
            }
        }

        private void Figure_AddAdditionalElement(Shape element)
        {
            AddToWorkplace(element);
        }

        private void SetShadowEventSubscription()
        {
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
            LeftUp += figure.LeftMouseButtonUp;
            RightDown += figure.RightMouseButtonDown;
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
            shadow.EndDrawShadodw -= Shadow_EndDrawShadow;
            shadow = null;
        }
        private void MoveWorkPlace()
        {
            scrollPoint.X = Scroll.HorizontalOffset - rightMouseButtonDownPos.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rightMouseButtonDownPos.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
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
        
        private void DeselectFigure()
        {
            if (selectedFigure == null)
                return;

            selectedFigure.Deselect();
            selectedFigure = null;
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
