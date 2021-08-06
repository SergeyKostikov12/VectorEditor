using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace GraphicEditor
{
    enum CursorType { Crosshair, Arrow, Hand }
    enum BtnPressed { None, Load, Save, Rect, Line, Move, Rotate, Scale, Width, Color, Fill }
    public partial class MainWindow : Window
    {
        private CursorType cursorWorkPlace;
        private BtnPressed buttonPressedFlag;
        private bool isDraw = false;
        private Rectangle shadowRect;
        private Polyline shadowLine;
        private Point firstPoint;
        private Point currentMousePos;
        private List<FigureObject> allFigures = new List<FigureObject>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeShadows();
        }

        private void ButtonPress(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string buttonName = btn.Name;
            switch (buttonName)
            {
                case "LoadBtn":
                    buttonPressedFlag = BtnPressed.Load;

                    break;
                case "SaveBtn":
                    buttonPressedFlag = BtnPressed.Save;

                    break;
                case "RectBtn":
                    buttonPressedFlag = BtnPressed.Rect;
                    SetCursor(CursorType.Crosshair);
                    break;
                case "LineBtn":
                    buttonPressedFlag = BtnPressed.Line;
                    SetCursor(CursorType.Crosshair);
                    break;
                case "MoveBtn":
                    buttonPressedFlag = BtnPressed.Move;
                    break;
                case "RotateBtn":
                    buttonPressedFlag = BtnPressed.Rotate;

                    break;
                case "ScaleBtn":
                    buttonPressedFlag = BtnPressed.Scale;

                    break;
                case "LineWidthBtn":
                    buttonPressedFlag = BtnPressed.Width;

                    break;
                case "ColorBtn":
                    buttonPressedFlag = BtnPressed.Color;

                    break;
                case "FillBtn":
                    buttonPressedFlag = BtnPressed.Fill;

                    break;
            }
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (buttonPressedFlag == BtnPressed.Rect)
            {
                SetCursor(CursorType.Arrow);
                buttonPressedFlag = BtnPressed.None;
            }
            else if (buttonPressedFlag == BtnPressed.Line)
            {
                SetCursor(CursorType.Arrow);
                buttonPressedFlag = BtnPressed.None;
            }
        }
        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            firstPoint = e.GetPosition(WorkPlace);
            if (buttonPressedFlag == BtnPressed.Rect)
            {
                isDraw = true;
            }
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition(WorkPlace);
            if (buttonPressedFlag == BtnPressed.Rect && isDraw)
            {
                isDraw = false;
                CreateNewFigure(endPoint);
            }
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.GetPosition(WorkPlace);
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                isDraw = true;

            }
            else isDraw = false;

            DrawShadow();

            InfoConsole.Text = isDraw.ToString() + "\n" + currentMousePos;
        }
        private void DrawShadow()
        {
            if (buttonPressedFlag == BtnPressed.Rect)
            {
                if (isDraw) shadowRect.Visibility = Visibility.Visible;
                else shadowRect.Visibility = Visibility.Hidden;
                DrawRectangleShadow();
            }

        }
        private void DrawRectangleShadow()
        {
            double xTop = Math.Max(firstPoint.X, currentMousePos.X);
            double yTop = Math.Max(firstPoint.Y, currentMousePos.Y);

            double xMin = Math.Min(firstPoint.X, currentMousePos.X);
            double yMin = Math.Min(firstPoint.Y, currentMousePos.Y);

            shadowRect.Height = yTop - yMin;
            shadowRect.Width = xTop - xMin;

            Canvas.SetLeft(shadowRect, xMin);
            Canvas.SetTop(shadowRect, yMin);
        }
        private  void CreateNewFigure(Point endPoint)
        {
            int n = WorkPlace.Children.Count;
            string name = "Figure_" + allFigures.Count + 1;
            FigureObject figure = new FigureObject(name, ShapeType.Rectangle, firstPoint, endPoint, WorkPlace);
            allFigures.Add(figure);
            int a = WorkPlace.Children.Count;
        }
        private void SetCursor(CursorType cursorType)
        {
            switch (cursorType)
            {
                case CursorType.Hand:
                    WorkPlace.Cursor = Cursors.Hand;
                    break;
                case CursorType.Crosshair:
                    WorkPlace.Cursor = Cursors.Cross;
                    break;
                case CursorType.Arrow:
                    WorkPlace.Cursor = Cursors.Arrow;
                    break;
                default:
                    WorkPlace.Cursor = Cursors.Arrow;
                    break;
            }
        }
        private void InitializeShadows()
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Stroke = Brushes.Blue;
            rectangle.StrokeDashArray = new DoubleCollection() { 4, 4 };
            rectangle.StrokeThickness = 2;
            rectangle.Visibility = Visibility.Hidden;
            shadowRect = rectangle;

            Polyline line = new Polyline();
            line.Stroke = Brushes.Blue;
            line.StrokeDashArray = new DoubleCollection() { 4, 4 };
            line.StrokeThickness = 2;
            line.Visibility = Visibility.Hidden;
            shadowLine = line;

            WorkPlace.Children.Add(shadowRect);
            WorkPlace.Children.Add(shadowLine);
        }
    }
}
