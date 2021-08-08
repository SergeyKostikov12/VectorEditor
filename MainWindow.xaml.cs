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
        private bool isMoving = false;
        private bool isRotating = false;
        private bool moveEnd = false;
        private bool isFirstPoint = true;
        private bool isPointSelect = false;

        private Rectangle shadowRect;
        private Polyline shadowLine;
        private Point firstPoint;
        private Point currentMousePos;
        private Point tempPosition;

        private string selectedPolylineName;
        private FigureObject selectedFigure;
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
                    DeselectAllPolylines();
                    buttonPressedFlag = BtnPressed.Rect;
                    SetCursor(CursorType.Crosshair);
                    break;
                case "LineBtn":
                    DeselectAllPolylines();
                    buttonPressedFlag = BtnPressed.Line;
                    SetCursor(CursorType.Crosshair);
                    break;
                case "MoveBtn":
                    if (selectedPolylineName != null)
                    {
                        if (selectedFigure.ShapeType == ShapeType.Rectangle)
                        {
                            buttonPressedFlag = BtnPressed.Move;
                            SetCursor(CursorType.Hand);
                        }
                        else MessageBox.Show("Перемещать линию не получится!");
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "RotateBtn":
                    if (selectedPolylineName != null)
                    {
                        if (selectedFigure.ShapeType == ShapeType.Rectangle)
                        {
                            buttonPressedFlag = BtnPressed.Rotate;
                            SetCursor(CursorType.Hand);
                            DrawGizmoForRotate();
                        }
                        else MessageBox.Show("Линию не получится вращать!");
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "ScaleBtn":
                    if (selectedPolylineName != null)
                    {
                        buttonPressedFlag = BtnPressed.Scale;
                        SetCursor(CursorType.Hand);
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "LineWidthBtn":
                    if (selectedPolylineName != null) buttonPressedFlag = BtnPressed.Width;
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "ColorBtn":
                    if (selectedPolylineName != null) buttonPressedFlag = BtnPressed.Color;
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "FillBtn":
                    if (selectedPolylineName != null) buttonPressedFlag = BtnPressed.Fill;
                    else MessageBox.Show("Сначала выделите объект!");
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
                if (shadowLine.Points.Count > 1)
                {
                    shadowLine.Points.RemoveAt(shadowLine.Points.Count - 1);
                    if (shadowLine.Points.Count > 1)
                    {
                        CreateNewFigure(shadowLine);
                    }
                }
            }
            else
            {
                SetCursor(CursorType.Arrow);
                buttonPressedFlag = BtnPressed.None;
                DeselectAllPolylines();
            }
        }
        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            firstPoint = e.GetPosition(WorkPlace);
            tempPosition = firstPoint;
            if (buttonPressedFlag == BtnPressed.Rect)
            {
                isDraw = true;
            }
            else if (buttonPressedFlag == BtnPressed.Line)
            {
                isDraw = true;
                AddPointToLineShadow(firstPoint);
            }
            else if (buttonPressedFlag == BtnPressed.Move)
            {
                isMoving = true;
            }
            else if (buttonPressedFlag == BtnPressed.Rotate)
            {
                isRotating = true;
            }
            else if (buttonPressedFlag == BtnPressed.None)
            {
                if (selectedPolylineName == null)
                {
                    string name = FindCollinearPoint();
                    if (name != null) SelectPolyline(name);
                }
                else if (selectedFigure.ShapeType == ShapeType.Line)
                {
                    GetPointsAround();
                }
                //DeselectAllPolylines();
            }
        }

        private void GetPointsAround()
        {
            Point pt = selectedFigure.GetPointNear(firstPoint);
            if (pt.X != 9999 && pt.Y != 0) isPointSelect = true;
        }

        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.GetPosition(WorkPlace);
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (buttonPressedFlag == BtnPressed.Rect) isDraw = true;
                if (buttonPressedFlag == BtnPressed.Move) isMoving = true;
                if (buttonPressedFlag == BtnPressed.Rotate) isRotating = true;
            }
            else if (buttonPressedFlag == BtnPressed.Line) isDraw = true;
            else
            {
                isDraw = false;
                isMoving = false;
            }
            DrawShadow();
            MoveFigure();
            RotateFigure();
            MovePoint();
            InfoConsole.Text = isDraw.ToString() + "\n" + currentMousePos + "\n" + selectedPolylineName;
        }

        private void MovePoint()
        {
            if (isPointSelect)
            {
                tempPosition = selectedFigure.MovePointToNewPosition(tempPosition, currentMousePos);
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
            if (buttonPressedFlag == BtnPressed.Move && isMoving)
            {
                isMoving = false;
            }
            if (buttonPressedFlag == BtnPressed.Rotate && isRotating)
            {
                isRotating = false;
            }
            if (isPointSelect)
            {
                isPointSelect = false;
            }
        }


        private void RotateFigure()
        {
            if (isRotating)
            {
                if (selectedFigure != null)
                {
                    tempPosition = selectedFigure.RotateFigure(tempPosition, currentMousePos);
                }
            }
        }
        private void MoveFigure()
        {
            if (isMoving)
            {
                if (selectedFigure != null)
                {
                    tempPosition = selectedFigure.MoveFigure(tempPosition, currentMousePos);
                }
            }
        }
        private void DrawGizmoForRotate()
        {
            selectedFigure.DrawCenterGizmo(true);
        }
        private void DeselectPolyline(string name)
        {
            foreach (var figure in allFigures)
            {
                if (name == figure.Name) figure.DrawOutline(false, WorkPlace);
            }
        }
        private void DeselectAllPolylines()
        {
            foreach (var figure in allFigures)
            {
                figure.DrawOutline(false, WorkPlace);
                figure.DrawCenterGizmo(false);
                selectedPolylineName = null;
                selectedFigure = null;
            }
        }
        private void SelectPolyline(string polylineName)
        {
            foreach (var figure in allFigures)
            {
                if (figure.Name == selectedPolylineName)
                {
                    figure.DrawOutline(true, WorkPlace);
                }
            }
        }
        private string FindCollinearPoint()
        {
            for (int i = 0; i < allFigures.Count; i++) //перебор на рабочем поле
            {
                FigureObject figure = allFigures[i];
                Point a = new Point();
                Point b = new Point();
                for (int j = 0; j < figure.Shape.Points.Count; j++)
                {
                    a = figure.Shape.Points[j];
                    b = figure.Shape.Points[j + 1];

                    if (AreCollinear(a, b, firstPoint))
                    {
                        selectedPolylineName = allFigures[i].Name;
                        selectedFigure = allFigures[i];
                        return selectedPolylineName;
                    }
                    if (j + 1 == figure.Shape.Points.Count - 1) break;
                }
            }
            if (selectedPolylineName == null) selectedFigure = null;
            return null;
        }
        private bool AreCollinear(Point A, Point B, Point C)
        {
            Vector CA = new Vector(A.X - C.X, A.Y - C.Y);
            Vector CB = new Vector(B.X - C.X, B.Y - C.Y);

            double angle = Math.Abs(Vector.AngleBetween(CA, CB));
            if (angle >= 175) return true;
            return false;
        }
        private void DrawShadow()
        {
            if (buttonPressedFlag == BtnPressed.Rect)
            {
                if (isDraw) shadowRect.Visibility = Visibility.Visible;
                else shadowRect.Visibility = Visibility.Hidden;
                DrawRectangleShadow();
            }
            else if (buttonPressedFlag == BtnPressed.Line)
            {
                if (isDraw)
                {
                    shadowLine.Visibility = Visibility.Visible;
                    DrawLineShadow();
                }
                else shadowLine.Visibility = Visibility.Hidden;
            }
        }
        private void AddPointToLineShadow(Point point)
        {
            if (!isFirstPoint)
            {
                shadowLine.Points[0] = point;
                shadowLine.Points.Add(currentMousePos);
                isFirstPoint = false;
            }
            else
            {
                int lastP = shadowLine.Points.Count - 1;
                Point tmpPoint = shadowLine.Points[lastP];
                shadowLine.Points.RemoveAt(lastP);
                shadowLine.Points.Add(point);
                shadowLine.Points.Add(currentMousePos);
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
        private void DrawLineShadow()
        {
            int n = shadowLine.Points.Count - 1;
            shadowLine.Points[n] = new Point(currentMousePos.X, currentMousePos.Y);
        }
        private void CreateNewFigure(Point endPoint)
        {
            int n = WorkPlace.Children.Count;
            string name = "Figure_" + allFigures.Count + 1;
            FigureObject figure = new FigureObject(name, ShapeType.Rectangle, firstPoint, endPoint, WorkPlace);
            allFigures.Add(figure);
        }
        private void CreateNewFigure(Polyline polyline)
        {
            int n = WorkPlace.Children.Count;
            string name = "Figure_" + allFigures.Count + 1;
            FigureObject figure = new FigureObject(name, ShapeType.Line, polyline, WorkPlace);
            allFigures.Add(figure);
            shadowLine.Visibility = Visibility.Hidden;
            shadowLine.Points.Clear();
            shadowLine.Points.Add(currentMousePos);
            isFirstPoint = true;
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
            rectangle.StrokeThickness = 1;
            rectangle.Visibility = Visibility.Hidden;
            shadowRect = rectangle;

            Polyline line = new Polyline();
            line.Stroke = Brushes.Blue;
            line.StrokeDashArray = new DoubleCollection() { 4, 4 };
            line.StrokeThickness = 1;
            line.Visibility = Visibility.Hidden;
            shadowLine = line;
            shadowLine.Points.Add(new Point(0, 0));

            WorkPlace.Children.Add(shadowRect);
            WorkPlace.Children.Add(shadowLine);
        }
        private FigureObject GetFigureByName(string selectedPolylineName)
        {
            foreach (var figure in allFigures)
            {
                if (figure.Name.Equals(selectedPolylineName)) return figure;
            }
            return null;
        }

    }
}
