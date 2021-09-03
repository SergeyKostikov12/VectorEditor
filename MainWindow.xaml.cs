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
    public partial class MainWindow : Window
    {
        public WorkplaceProcess WorkplaceProcess;
        public FigureProcess Process;
        public WorkplaceCondition Condition;
        public WorkplaceShadow Shadow;
        public ColorPicker ColoRPicker = new ColorPicker();
        public WidthPicker WidthPicker = new WidthPicker();

        private Point LMB_ClickPosition;
        private Point currentMousePos;



        private Point RMB_firstPoint;

        public MainWindow()
        {
            InitializeComponent();
            InitializeProcessors();
        }

        private void InitializeProcessors()
        {
            WorkplaceProcess = new WorkplaceProcess(WorkPlace);
            Process = new FigureProcess(WorkPlace, Condition);
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
                WorkplaceProcess.LoadWorkplace();
            }
            else if (buttonName == "SaveBtn")
            {
                Condition.ButtonPressed = ButtonPressed.Save;
                Process.DeselectFigure();
                WorkplaceProcess.SaveWorkplace();
            }
            else if (buttonName == "RectBtn")
            {
                Condition.ButtonPressed = ButtonPressed.Rect;
                Process.DeselectFigure();
                SetCursor(CursorType.Crosshair);
            }
            else if (buttonName == "LineBtn")
            {
                Condition.ButtonPressed = ButtonPressed.Line;
                Process.DeselectFigure();
                SetCursor(CursorType.Crosshair);
            }
            else if (buttonName == "DeleteBtn")
            {
                Process.DeleteFigure();
                SetCursor(CursorType.Arrow);
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
        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)///////////////////////////LMB_DOWN
        {
            LMB_ClickPosition = e.GetPosition(WorkPlace);
            Process.LMB_ClickPosition = LMB_ClickPosition;
            Condition.MouseDown = true;

            if (Condition.ButtonPressed == ButtonPressed.Rect)
            {
                Condition.Action = Actions.DrawRect;
                Shadow.StartDrawRectShadow(LMB_ClickPosition);
                Process.LMB_ClickPosition = LMB_ClickPosition;
            }
            else if (Condition.ButtonPressed == ButtonPressed.Line)
            {
                Condition.Action = Actions.DrawLine;
                if (Shadow.ShadowLine.Points.Count == 0)
                {
                    Shadow.DrawLineShadow();
                    Shadow.SetLIneFirstPoint(LMB_ClickPosition);
                    Shadow.SetLineSecondPoint(LMB_ClickPosition);
                }
            }
            else if (Condition.ButtonPressed == ButtonPressed.None)
            {
                Condition.Action = Process.DetermindAction(LMB_ClickPosition);
            }

            if (e.ClickCount == 2)
            {
                Process.ExecuteDoubleClick(LMB_ClickPosition);
            }
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)/////////////////////////RMB_DOWN
        {
            RMB_firstPoint = e.GetPosition(WorkPlace);
            SetCursor(CursorType.Arrow);
            if (Condition.Action == Actions.DrawLine)
            {
                Shadow.RemoveLastPoint();
                Process.CreatePolyline(Shadow.ShadowLine);
                Shadow.Clear();
            }
            Condition.ResetCondition();
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)//////////////////////////////////////MOVE
        {
            Point currentMousePos = e.GetPosition(WorkPlace);
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
                else if (Condition.Action == Actions.MoveRect)
                {
                    Process.MoveRect(currentMousePos);
                }
                else if (Condition.Action == Actions.RotateRect)
                {
                    Process.RotateRect(currentMousePos);
                }
                else if (Condition.Action == Actions.ScaleRect)
                {
                    Process.ScaleRect(currentMousePos);
                }
                else if (Condition.Action == Actions.MovePoint)
                {
                    Process.MovePoint(currentMousePos);
                }
                //Process.Drag(currentMousePos);
            }

            if (Condition.Action == Actions.DrawLine)
            {
                Shadow.DrawLastPointShadowtLine(currentMousePos);
            }


            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                WorkplaceProcess.MovingWorkPlace(Scroll, RMB_firstPoint, currentMousePos);
            }
        }

        private bool AlllowDistance(Point currentMousePos)
        {
            Point delta = LMB_ClickPosition.AbsDeltaTo(currentMousePos);
            if (delta.X > 5 || delta.Y > 5)
            {
                return true;
            }
            else return false;
        }

        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)///////////////LMB_UP
        {
            Point endPoint = e.GetPosition(WorkPlace);
            Condition.MouseUp = true;

            if (Condition.Action == Actions.DrawRect)
            {
                Condition.Action = Actions.None;
                Process.CreateRect(endPoint);
                Shadow.Clear();
            }
            else if (Condition.Action == Actions.DrawLine)
            {
                if (Condition.IsDrawLine())
                {
                    if (Shadow.ShadowLine.Points.Count <= 2)
                    {
                        Condition.Action = Actions.None;
                        Process.CreateLine(LMB_ClickPosition, endPoint);
                        Shadow.Clear();
                    }
                        Condition.ResetMouseState();
                }
                else
                {
                    Shadow.AddPoint(endPoint);
                }
            }
        }
        private void WorkPlace_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

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
        private void SetStyle()
        {
            ColoRPicker.Figure = Process.SelectedFigure;
            ColoRPicker.Visibility = Visibility.Visible;
        }
    }
}
