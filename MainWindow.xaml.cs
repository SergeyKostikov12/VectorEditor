using GraphicEditor.Functionality;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GraphicEditor
{
    public enum BtnPressed { None, Load, Save, Rect, Line, Move, Rotate, Scale, Width, Color, Fill }
    enum CursorType { Crosshair, Arrow, Hand }
    public partial class MainWindow : Window
    {
        private BtnPressed buttonPressedFlag;
        private bool isDraw = false;
        private bool isMoving = false;
        private bool isRotating = false;
        private bool isScaling = false;
        private bool isFirstPoint = true;
        private bool isLineSelect = false;
        private bool isPointMove = false;
        private int pointNumber;
        private ColorPicker coloRPicker = new ColorPicker();
        private WidthPicker widthPicker = new WidthPicker();

        private Rectangle shadowRect;
        private Polyline shadowLine;
        private Point firstPoint;
        private Point currentMousePos;
        private Point tempPosition;
        //private Brush lineColor;
        //private Frame frame;

        private string selectedPolylineName;
        private FigureObject selectedFigure;

        public List<FigureObject> allFigures = new List<FigureObject>();

        public FiguresList FiguresList;

        //public Frame Frame { get => PropertyPanel; /*set => frame = value; */}

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
                    DeselectAllPolylines();
                    //buttonPressedFlag = BtnPressed.Load;
                    ClearCanvas();
                    LoadWorkPlace();
                    break;
                case "SaveBtn":
                    DeselectAllPolylines();
                    //buttonPressedFlag = BtnPressed.Save;
                    SaveWorkPlace();
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
                            DrawGizmoForRotate();
                        }
                        else MessageBox.Show("Перемещать линию не получится!");
                        coloRPicker.Visibility = Visibility.Hidden;
                        widthPicker.Visibility = Visibility.Hidden;
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
                        coloRPicker.Visibility = Visibility.Hidden;
                        widthPicker.Visibility = Visibility.Hidden;
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "ScaleBtn":
                    if (selectedPolylineName != null)
                    {
                        if (selectedFigure.ShapeType == ShapeType.Rectangle)
                        {
                            buttonPressedFlag = BtnPressed.Scale;
                            SetCursor(CursorType.Hand);
                            DrawGizmoForRotate();
                        }
                        else MessageBox.Show("Линию вращать не получится!");
                        coloRPicker.Visibility = Visibility.Hidden;
                        widthPicker.Visibility = Visibility.Hidden;
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "DeleteBtn":
                    if (selectedPolylineName != null)
                    {
                        SetCursor(CursorType.Arrow);
                        DeleteFigure();
                        widthPicker.Visibility = Visibility.Hidden;
                        coloRPicker.Visibility = Visibility.Hidden;
                    }
                    break;
                case "LineWidthBtn":
                    if (selectedPolylineName != null)
                    {
                        coloRPicker.Visibility = Visibility.Hidden;
                        PropertyPanel.Content = widthPicker;
                        //buttonPressedFlag = BtnPressed.Width;
                        SetWidth();
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "ColorBtn":
                    if (selectedPolylineName != null)
                    {
                        widthPicker.Visibility = Visibility.Hidden;
                        PropertyPanel.Content = coloRPicker;
                        coloRPicker.BtnPressed = BtnPressed.Color;
                        //buttonPressedFlag = BtnPressed.Color;
                        SetStyle();
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
                case "FillBtn":
                    if (selectedPolylineName != null)
                    {
                        if (selectedFigure.ShapeType == ShapeType.Rectangle)
                        {
                            PropertyPanel.Content = coloRPicker;
                            coloRPicker.BtnPressed = BtnPressed.Fill;
                            SetStyle();
                        }
                        else MessageBox.Show("Линию нельзя залить!");
                        //buttonPressedFlag = BtnPressed.Fill;
                    }
                    else MessageBox.Show("Сначала выделите объект!");
                    break;
            }
        }



        private void SaveWorkPlace()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".vec"; // Default file extension
            dlg.Filter = "Vector documents (.vec)|*.vec"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                // сохраняем текст в файл
                CreateSaveList(allFigures);

                XmlSerializer formatter = new XmlSerializer(FiguresList.GetType());

                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, FiguresList);
                }
                MessageBox.Show("Файл успешно сохранен!");
            }
        }

        private void CreateSaveList(List<FigureObject> list)
        {
            FiguresList = new FiguresList();
            List<SLFigure> tmp = new List<SLFigure>();
            for (int i = 0; i < list.Count; i++)
            {
                SLFigure sLFigure = new SLFigure();
                sLFigure = sLFigure.CreateSLFigureFromFigureObject(allFigures[i]);
                tmp.Add(sLFigure);
            }
            FiguresList.Figures = tmp;
        }

        private void LoadWorkPlace()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vector Files(*.vec)|*.vec|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            var xmlSerializer = new XmlSerializer(typeof(FiguresList));
            if (openFileDialog.ShowDialog() != null)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            FiguresList sL = (FiguresList)xmlSerializer.Deserialize(myStream);
                            FiguresList = sL;
                        }
                    }
            CreateFiguresFromList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void CreateFiguresFromList()
        {
            for (int i = 0; i<FiguresList.Figures.Count; i++)
            {
                FigureObject figureObject = new FigureObject(FiguresList.Figures[i], WorkPlace);
                allFigures.Add(figureObject);
            }
        }

        private void ClearCanvas()
        {
            WorkPlace.Children.Clear();
            InitializeShadows();
        }


        private void SetWidth()
        {
            widthPicker.Figure = selectedFigure;
            widthPicker.Visibility = Visibility.Visible;
        }
        private void SetStyle()
        {
            coloRPicker.FigureObject = selectedFigure;
            coloRPicker.Visibility = Visibility.Visible;
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.GetPosition(WorkPlace);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (buttonPressedFlag == BtnPressed.Rect) isDraw = true;
                if (buttonPressedFlag == BtnPressed.Move) isMoving = true;
                if (buttonPressedFlag == BtnPressed.Rotate) isRotating = true;
                if (buttonPressedFlag == BtnPressed.Scale) isScaling = true;
                if (isLineSelect) isPointMove = true;
            }
            else if (buttonPressedFlag == BtnPressed.Line) isDraw = true;
            else
            {
                isDraw = false;
                isMoving = false;
                isScaling = false;
                isPointMove = false;
            }
            DrawShadow();
            MoveFigure();
            MovePoint();
            RotateFigure();
            ScaleFigure();
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
            else if (buttonPressedFlag == BtnPressed.Scale)
            {
                isScaling = true;
            }
            else if (buttonPressedFlag == BtnPressed.None)
            {
                if (selectedPolylineName == null)
                {
                    string name = FindCollinearPoint();
                    if (name != null)
                    {
                        SelectPolyline(name);
                        if (selectedFigure.ShapeType == ShapeType.Line) isLineSelect = true;
                    }
                }
                else if (isLineSelect)
                {
                    GetPointsAround();
                }
            }

            if (e.ClickCount == 2)
            {
                if (selectedPolylineName != null && selectedFigure.ShapeType == ShapeType.Line)
                {
                    selectedFigure.AddPointFromDoubleClick(firstPoint);
                    isPointMove = false;
                }
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
            if (isPointMove)
            {
                isPointMove = false;
                int n = selectedFigure.GetPointNear(endPoint);
                if (n != 0)
                {
                    if (pointNumber != 0) selectedFigure.CollapsePoints(pointNumber);
                }
                pointNumber = 0;
            }
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (buttonPressedFlag == BtnPressed.Rect)
            {
                SetCursor(CursorType.Arrow);
                shadowRect.Visibility = Visibility.Hidden;
                buttonPressedFlag = BtnPressed.None;
                DeselectAllPolylines();
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
        private string FindCollinearPoint()
        {
            for (int i = 0; i < allFigures.Count; i++)
            {
                FigureObject figure = allFigures[i];
                if (figure.AreCollinear(firstPoint))
                {
                    return figure.Name;
                }
            }
            return null;
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
                shadowLine.Points.RemoveAt(lastP);
                shadowLine.Points.Add(point);
                shadowLine.Points.Add(currentMousePos);
            }
        }
        //private bool AreCollinear(Point A, Point B, Point C_center)
        //{
        //    Vector CA = new Vector(A.X - C_center.X, A.Y - C_center.Y);
        //    Vector CB = new Vector(B.X - C_center.X, B.Y - C_center.Y);

        //    double angle = Math.Abs(Vector.AngleBetween(CA, CB));
        //    if (angle >= 175) return true;
        //    return false;
        //}
        private void CheckValid()
        {
            List<int> tmpList = new List<int>();
            for (int i = 0; i < allFigures.Count; i++)
            {
                if (allFigures[i].Name == null)
                {
                    tmpList.Add(i);
                }
            }
            if (tmpList.Count != 0)
            {
                for (int i = 0; i < tmpList.Count; i++)
                {
                    int n = tmpList[i];
                    allFigures.RemoveAt(n);
                }
            }
        }
        private void CreateNewFigure(Point endPoint)
        {
            int _1 = WorkPlace.Children.Count;
            string name = "Figure_" + allFigures.Count + 1;
            FigureObject figure = new FigureObject(name, ShapeType.Rectangle, firstPoint, endPoint, WorkPlace);
            allFigures.Add(figure);
        }
        private void CreateNewFigure(Polyline polyline)
        {
            int _1 = WorkPlace.Children.Count;
            string name = "Figure_" + allFigures.Count + 1;
            FigureObject figure = new FigureObject(name, ShapeType.Line, polyline, WorkPlace);
            allFigures.Add(figure);
            shadowLine.Visibility = Visibility.Hidden;
            shadowLine.Points.Clear();
            shadowLine.Points.Add(currentMousePos);
            isFirstPoint = true;
        }
        private void DrawLineShadow()
        {
            int n = shadowLine.Points.Count - 1;
            shadowLine.Points[n] = new Point(currentMousePos.X, currentMousePos.Y);
        }
        private void DrawGizmoForRotate()
        {
            selectedFigure.DrawCenterGizmo(true);
        }
        private void DeleteFigure()
        {
            if (selectedFigure != null) selectedFigure.DeletePolyline();
            DeselectAllPolylines();
        }
        private void DeselectAllPolylines()
        {
            CheckValid();
            foreach (var figure in allFigures)
            {
                figure.DrawOutline(false);
                figure.DrawCenterGizmo(false);
            }
            selectedPolylineName = null;
            selectedFigure = null;
            isLineSelect = false;
            buttonPressedFlag = BtnPressed.None;
            coloRPicker.Visibility = Visibility.Hidden;
            widthPicker.Visibility = Visibility.Hidden;
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
        private void GetPointsAround()
        {
            if (selectedFigure.Name != null)
            {
                int n = selectedFigure.GetPointNear(firstPoint);
                if (n != 0)
                {
                    isPointMove = true;
                    pointNumber = n;
                }

            }
            else DeselectAllPolylines();
        }
        private void InitializeShadows()
        {
            Rectangle rectangle = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };
            shadowRect = rectangle;
            Polyline line = new Polyline
            {
                Stroke = Brushes.Blue,
                StrokeDashArray = new DoubleCollection() { 4, 4 },
                StrokeThickness = 1,
                Visibility = Visibility.Hidden
            };

            shadowLine = line;
            shadowLine.Points.Add(new Point(0, 0));

            WorkPlace.Children.Add(shadowRect);
            WorkPlace.Children.Add(shadowLine);
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
        private void MovePoint()
        {
            if (isLineSelect)
            {
                if (pointNumber != 0)
                {
                    tempPosition = selectedFigure.MovePointToNewPosition(pointNumber, tempPosition, currentMousePos);
                }
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
        private void SelectPolyline(string polylineName)
        {
            foreach (var figure in allFigures)
            {
                if (figure.Name == polylineName)
                {
                    figure.DrawOutline(true);
                    selectedFigure = figure;
                    selectedPolylineName = polylineName;
                }
            }
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
        private void ScaleFigure()
        {
            if (isScaling)
            {
                if (selectedFigure != null)
                {
                    tempPosition = selectedFigure.ScaleFigure(tempPosition, currentMousePos);
                }
            }
        }
    }
}
