using GraphicEditor.Events;
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
        public event FigureSelectEventHandler FigureSelect;
        public event FigureDeselectEventHandler FigureDeselect;

        private delegate void LeftMouseButtonUpEventHandler(Point position);
        private delegate void RightMouseButtonDownEventHandler(Point position);

        private event LeftMouseButtonUpEventHandler LeftUp;
        private event RightMouseButtonDownEventHandler RightDown;

        private Shadow shadow;
        private Figure selectedFigure;
        private DrawingMode drawingMode;
        private List<Figure> allFigures = new List<Figure>();

        private Point rightMouseButtonDownPos = new Point(0, 0);
        private Point currentMousePos = new Point(0, 0);
        private Point previosMousePosition = new Point(0, 0);
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

        public void ReadyDrawFigure(DrawingMode mode)
        {
            WorkPlaceCanvas.Cursor = Cursors.Cross;
            drawingMode = mode;
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
            previosMousePosition = position;

            if (e.ClickCount == 2)
            {
                selectedFigure?.LeftMouseButtonClick(position);
                return;
            }

            if (shadow == null && drawingMode != DrawingMode.None)
            {
                CreateShadow();
            }

            if (shadow != null)
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

            if (shadow != null)
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
            if (shadow != null)
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
            if (shadow != null)
            {
                shadow.MouseMove(currentMousePos);
                return;
            }
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (previosMousePosition.Length(currentMousePos) > 100)
                {
                    var len = previosMousePosition.Length(currentMousePos);
                }
                var points = CalculateIntermediatePoints(previosMousePosition, currentMousePos);
                foreach (var point in points)
                {
                    selectedFigure?.MouseMove(point);
                }
                previosMousePosition = currentMousePos;
            }
        }


        private void CreateShadow()
        {
            shadow = Shadow.Create(drawingMode);
            AddToWorkplace(shadow.GetShape());
            SetShadowEventSubscription();
        }
        private void Shadow_EndDrawShadow(Shadow shadow)
        {
            RemoveShadow();
            Figure figure = Figure.Create(shadow);
            allFigures.Add(figure);
            SetFigureEventSubscription(figure);
            AddToWorkplace(figure.GetShapes());
        }
        //private void Shadow_EndDrawShadow(Figure figure)
        //{
        //    RemoveShadow();
        //    allFigures.Add(figure);
        //    SetFigureEventSubscription(figure);
        //    AddToWorkplace(figure.GetShapes());
        //}
        private void Figure_SelectFigure(Figure sender, FigureSelectEventArgs e)
        {
            if (selectedFigure == null)
            {
                selectedFigure = sender;
                FigureSelect?.Invoke(sender, e);
                sender.ShowOutline();
                UpZPosition(sender);
                return;
            }
            sender.Deselect();
            selectedFigure = null;
        }
        private void Figure_AddAdditionalElement(Shape element)
        {
            AddToWorkplace(element);
        }

        private Point[] CalculateIntermediatePoints(Point previosPosition, Point currentMousePos)
        {
            Point[] points = new Point[2] { previosPosition, currentMousePos };
            int precision = 10;
            for (int i = 0; i < precision; i++)
            {
                Point[] p = DivideArray(points);
                points = p;
            }
            return points;
        }
        private Point[] DivideArray(Point[] points)
        {
            int n = points.Length;
            Point[] newPoints = new Point[n * 2 - 1];
            int x = 0;
            for (int i = 0; i < newPoints.Length; i++)
            {
                if (i % 2 == 0)
                {
                    newPoints[i] = points[x];
                    x++;
                    continue;
                }
                newPoints[i] = new Point((points[x - 1].X + points[x].X) / 2, (points[x - 1].Y + points[x].Y) / 2);
            }
            return newPoints;
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
            figure.SelectFigure += Figure_SelectFigure;
            figure.DeselectFigure += Figure_DeselectFigure;
            figure.AddAdditionalElement += Figure_AddAdditionalElement;
        }

        private void Figure_DeselectFigure(Figure figure)
        {
            FigureDeselect?.Invoke(figure);
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
