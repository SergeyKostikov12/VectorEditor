using GraphicEditor.Functionality;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using System.Windows.Shapes;

namespace GraphicEditor
{
    public enum CursorType { Crosshair, Arrow, Hand }
    public enum FigureType { Rectangle, Line }

    public partial class MainWindow : Window
    {
        public WorkplaceProcess Process;
        public WorkplaceCondition Condition;
        public WorkplaceShadow Shadow;
        public ColorPicker ColoRPicker = new ColorPicker();
        public WidthPicker WidthPicker = new WidthPicker();

        //private int pointNumber;
        //private Rectangle shadowRect;
        //private Polyline shadowLine;
        private Point LMB_ClickPosition;
        //private Point firstPointRMB;
        private Point scrollPoint = new Point(0, 0);
        private Point currentMousePos;
        //private Point tempPosition;
        //private string selectedPolylineName;
        //private FigureObject selectedFigure;


        public List<FigureObject> allFigures = new List<FigureObject>();
        public FiguresList FiguresList;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWorkplaceProcessors();
        }

        private void InitializeWorkplaceProcessors()
        {
            Process = new WorkplaceProcess(WorkPlace);
            Condition = new WorkplaceCondition();
            Shadow = new WorkplaceShadow(WorkPlace);
        }

        private void ButtonPress(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string buttonName = btn.Name;

            if (buttonName == "LoadBtn")
            {
                Condition.ButtonPressed = ButtonPressed.Load;
                Process.DeselectFigure();
                Process.LoadWorkplace();
                //DeselectAllPolylines();
                //Condition.buttonPressedFlag = ButtonPressed.None;
                //LoadWorkPlace();
            }
            else if (buttonName == "SaveBtn")
            {
                Condition.ButtonPressed = ButtonPressed.Save;
                Process.DeselectFigure();
                Process.SaveWorkplace();
                //DeselectAllPolylines();
                //Condition.buttonPressedFlag = ButtonPressed.None;
                //SaveWorkPlace();
            }
            else if (buttonName == "RectBtn")
            {
                Condition.ButtonPressed = ButtonPressed.Rect;
                Process.DeselectFigure();
                SetCursor(CursorType.Crosshair);
                //DeselectAllPolylines();
                //Condition.buttonPressedFlag = ButtonPressed.Rect;
            }
            else if (buttonName == "LineBtn")
            {
                Condition.ButtonPressed = ButtonPressed.Line;
                DeselectAllPolylines();
                SetCursor(CursorType.Crosshair);
                //Condition.buttonPressedFlag = ButtonPressed.Line;
            }
            else if (buttonName == "DeleteBtn")
            {
                Process.DeleteFigure();
                SetCursor(CursorType.Arrow);

                //if (selectedPolylineName != null)
                //{
                //    DeleteFigure();
                //    widthPicker.Visibility = Visibility.Hidden;
                //    coloRPicker.Visibility = Visibility.Hidden;
                //}
            }
            else if (buttonName == "LineWidthBtn")
            {

                if (Process.SelectedFigure != null)
                {
                    ColoRPicker.Visibility = Visibility.Hidden;
                    PropertyPanel.Content = WidthPicker;
                    SetWidth();
                }
            }
            else if (buttonName == "ColorBtn")
            {
                if (Process.SelectedFigure != null)
                {
                    PropertyPanel.Content = ColoRPicker;
                    WidthPicker.Visibility = Visibility.Hidden;
                    ColoRPicker.BtnPressed = ButtonPressed.Color;
                    SetStyle();
                }
            }
            else if (buttonName == "FillBtn")
            {
                if (Process.SelectedFigure != null)
                {
                    PropertyPanel.Content = ColoRPicker;
                    ColoRPicker.BtnPressed = ButtonPressed.Fill;
                    SetStyle();
                }
                else MessageBox.Show("Сначала выделите объект!");
            }

        }
        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LMB_ClickPosition = e.GetPosition(WorkPlace);
            Process.LMB_ClickPosition = LMB_ClickPosition;
            //tempPosition = LMB_Position;

            if(Condition.ButtonPressed == ButtonPressed.Rect)
            {
                Shadow.DrawRectShadow(LMB_ClickPosition);
            }
            else if(Condition.ButtonPressed == ButtonPressed.Line)
            {
                Condition.Action = Actions.DrawPolyline;
                Shadow.DrawLineShadow(LMB_ClickPosition);
                Shadow.FirstPoint = LMB_ClickPosition;
            }


            if (e.ClickCount == 2)
            {
                Process.ExecuteDoubleClick(LMB_ClickPosition);
            }

            //if (buttonPressedFlag == ButtonPressed.Rect)
            //{
            //    isDraw = true;
            //}
            //else if (buttonPressedFlag == ButtonPressed.Line)
            //{
            //    isDraw = true;
            //    AddPointToLineShadow(LMB_ClickPosition);
            //}
            //else if (buttonPressedFlag == ButtonPressed.Move)
            //{
            //    isMoving = true;
            //}
            //else if (buttonPressedFlag == ButtonPressed.Rotate)
            //{
            //    isRotating = true;
            //}
            //else if (buttonPressedFlag == ButtonPressed.Scale)
            //{
            //    isScaling = true;
            //}
            //else if (buttonPressedFlag == ButtonPressed.None)
            //{
            //    if (selectedPolylineName == null)
            //    {
            //        string name = FindCollinearPoint();
            //        if (name != null)
            //        {
            //            SelectPolyline(name);
            //            if (selectedFigure.ShapeType == FigureType.Line) isLineSelect = true;
            //        }
            //    }
            //    else if (isLineSelect)
            //    {
            //        GetPointsAround();
            //    }
            //}

            //if (e.ClickCount == 2)
            //{
            //    if (selectedPolylineName != null && selectedFigure.ShapeType == FigureType.Line)
            //    {
            //        selectedFigure.AddPointFromDoubleClick(LMB_ClickPosition);
            //        isPointMove = false;
            //    }
            //}
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            firstPointRMB = e.GetPosition(WorkPlace);
            tempPosition = e.GetPosition(WorkPlace);
            if (buttonPressedFlag == ButtonPressed.Rect)
            {
                SetCursor(CursorType.Arrow);
                shadowRect.Visibility = Visibility.Hidden;
                buttonPressedFlag = ButtonPressed.None;
                DeselectAllPolylines();
            }
            else if (buttonPressedFlag == ButtonPressed.Line)
            {
                SetCursor(CursorType.Arrow);
                buttonPressedFlag = ButtonPressed.None;
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
                buttonPressedFlag = ButtonPressed.None;
                DeselectAllPolylines();
            }
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.GetPosition(WorkPlace);
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if(Condition.ButtonPressed == ButtonPressed.Rect)
                {
                    Condition.Action = Actions.DrawRect;
                }
                else if(Condition.ButtonPressed == ButtonPressed.Line)
                {
                    if(Condition.Action == Actions.DrawPolyline)
                    {
                        Condition.Action = Actions.DrawLine;
                    }
                    else if(Condition.Action == Actions.DrawLine)
                    {
                        Shadow.SetSecondPoint(currentMousePos);
                    }
                }
                else if(Condition.Action == Actions.MoveRect)
                {
                    Process.MoveRect(currentMousePos);
                }
                else if(Condition.Action == Actions.RotateRect)
                {
                    Process.RotateRect(currentMousePos);
                }
                else if(Condition.Action == Actions.ScaleRect)
                {
                    Process.ScaleRect(currentMousePos);
                }
                

                Process.Drag(currentMousePos);

                //if (buttonPressedFlag == ButtonPressed.Rect) isDraw = true;
                //if (buttonPressedFlag == ButtonPressed.Move) isMoving = true;
                //if (buttonPressedFlag == ButtonPressed.Rotate) isRotating = true;
                //if (buttonPressedFlag == ButtonPressed.Scale) isScaling = true;
                //if (isLineSelect) isPointMove = true;
            }
            //else if (buttonPressedFlag == ButtonPressed.Line) isDraw = true;
            //else
            //{
            //    isDraw = false;
            //    isMoving = false;
            //    isScaling = false;
            //    isPointMove = false;
            //}
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Process.MovingWorkPlace(currentMousePos);
                //isWorkplaceMoved = true;
            }
            //else isWorkplaceMoved = false;



            DrawShadow();
            //MoveFigure();
            //MovePoint();
            //RotateFigure();
            //ScaleFigure();
            //MoveWorkPlace();
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) //TODO: Связать отпускание кнопки мыши с Процессами
        {
            Point endPoint = e.GetPosition(WorkPlace);



            if (buttonPressedFlag == ButtonPressed.Rect && isDraw)
            {
                isDraw = false;
                CreateNewFigure(endPoint);
            }
            if (buttonPressedFlag == ButtonPressed.Move && isMoving)
            {
                isMoving = false;
            }
            if (buttonPressedFlag == ButtonPressed.Rotate && isRotating)
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
        private void WorkPlace_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            isWorkplaceMoved = false;
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
        private string FindCollinearPoint()
        {
            for (int i = 0; i < allFigures.Count; i++)
            {
                FigureObject figure = allFigures[i];
                if (figure.AreCollinear(LMB_ClickPosition))
                {
                    return figure.Name;
                }
            }
            return null;
        }
        private void ClearCanvas()
        {
            WorkPlace.Children.Clear();
            //InitializeShadows();
        }
        private void CreateFiguresFromList()
        {
            for (int i = 0; i < FiguresList.Figures.Count; i++)
            {
                FigureObject figureObject = new FigureObject(FiguresList.Figures[i], WorkPlace);
                allFigures.Add(figureObject);
            }
        }
        private void CreateNewFigure(Point endPoint)
        {
            FigureObj figure = new RectangleObj(LMB_ClickPosition, endPoint);
            figure.PlacingInWorkPlace(WorkPlace);
            //figure.ShowOutline();

            //int _1 = WorkPlace.Children.Count;
            //string name = "Figure_" + allFigures.Count + 1;
            //FigureObject figure = new FigureObject(name, FigureType.Rectangle, firstPointLMB, endPoint, WorkPlace);
            //allFigures.Add(figure);
        }
        private void CreateNewFigure(Polyline polyline)
        {
            FigureObj figure = new LineObj(polyline);
            figure.PlacingInWorkPlace(WorkPlace);
            //figure.ShowOutline();

            //int _1 = WorkPlace.Children.Count;
            //string name = "Figure_" + allFigures.Count + 1;
            //FigureObject figure = new FigureObject(name, FigureType.Line, polyline, WorkPlace);
            //allFigures.Add(figure);
            shadowLine.Visibility = Visibility.Hidden;
            shadowLine.Points.Clear();
            shadowLine.Points.Add(currentMousePos);
            isFirstPoint = true;
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
            buttonPressedFlag = ButtonPressed.None;
            ColoRPicker.Visibility = Visibility.Hidden;
            WidthPicker.Visibility = Visibility.Hidden;
        }
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
        private void DrawRectangleShadow()
        {
            double xTop = Math.Max(LMB_ClickPosition.X, currentMousePos.X);
            double yTop = Math.Max(LMB_ClickPosition.Y, currentMousePos.Y);

            double xMin = Math.Min(LMB_ClickPosition.X, currentMousePos.X);
            double yMin = Math.Min(LMB_ClickPosition.Y, currentMousePos.Y);

            shadowRect.Height = yTop - yMin;
            shadowRect.Width = xTop - xMin;

            Canvas.SetLeft(shadowRect, xMin);
            Canvas.SetTop(shadowRect, yMin);
        }
        private void DrawShadow()
        {
            if (buttonPressedFlag == ButtonPressed.Rect)
            {
                if (isDraw) shadowRect.Visibility = Visibility.Visible;
                else shadowRect.Visibility = Visibility.Hidden;
                DrawRectangleShadow();
            }
            else if (buttonPressedFlag == ButtonPressed.Line)
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
                int n = selectedFigure.GetPointNear(LMB_ClickPosition);
                if (n != 0)
                {
                    isPointMove = true;
                    pointNumber = n;
                }

            }
            else DeselectAllPolylines();
        }

        private void LoadWorkPlace()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vector Files(*.vec)|*.vec|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            var xmlSerializer = new XmlSerializer(typeof(FiguresList));
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
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
                    ClearCanvas();
                    CreateFiguresFromList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                WorkPlace.UpdateLayout();
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
        private void MoveWorkPlace()
        {
            if (isWorkplaceMoved)
            {
                isWorkplaceMoved = false;
                scrollPoint.X = Scroll.HorizontalOffset - firstPointRMB.DeltaTo(currentMousePos).X / 2;
                scrollPoint.Y = Scroll.VerticalOffset - firstPointRMB.DeltaTo(currentMousePos).Y / 2;
                Scroll.ScrollToHorizontalOffset(scrollPoint.X);
                Scroll.ScrollToVerticalOffset(scrollPoint.Y);
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
        private void SaveWorkPlace()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".vec"; // Default file extension
            dlg.Filter = "Vector documents (.vec)|*.vec"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                if (dlg.FileName.Length != 0)
                {
                    File.Delete(dlg.FileName);
                }
                string filename = dlg.FileName;
                CreateSaveList(allFigures);
                XmlSerializer formatter = new XmlSerializer(FiguresList.GetType());
                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, FiguresList);
                }
                MessageBox.Show("Файл успешно сохранен!");
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
        private void SetWidth()
        {
            WidthPicker.Figure = Process.SelectedFigure;
            WidthPicker.Visibility = Visibility.Visible;
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




        private void SetStyle()
        {
            ColoRPicker.Figure = Process.SelectedFigure;
            ColoRPicker.Visibility = Visibility.Visible;
        }
    }
}
