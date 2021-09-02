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
        private Point scrollPoint = new Point(0, 0);
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
        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPosition = e.GetPosition(WorkPlace);
            Process.LMB_ClickPosition = LMB_ClickPosition;

            if (Condition.ButtonPressed == ButtonPressed.Rect)
            {
                Condition.Action = Actions.DrawRect;
                Shadow.StartDrawRectShadow(clickPosition);
            }
            else if (Condition.ButtonPressed == ButtonPressed.Line)
            {
                if (Condition.Action == Actions.DrawPolyline)
                {
                    Shadow.AddPoint(clickPosition);
                }
                else
                {
                    Condition.Action = Actions.DrawPolyline;
                    Shadow.DrawLineShadow(clickPosition);
                    Shadow.AddPoint(clickPosition);
                    Shadow.FirstPoint = clickPosition;
                }

            }
            else if (Condition.ButtonPressed == ButtonPressed.None)
            {
                Condition.Action = Process.DetermindAction(clickPosition);
            }

            if (e.ClickCount == 2)
            {
                Process.ExecuteDoubleClick(LMB_ClickPosition);
            }
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            RMB_firstPoint = e.GetPosition(WorkPlace);
            Condition.ResetCondition();

        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePos = e.GetPosition(WorkPlace);
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (Condition.Action == Actions.DrawRect)
                {
                    Shadow.DrawRectShadow(currentMousePos);
                }
                else if (Condition.ButtonPressed == ButtonPressed.Line)
                {
                    if (Condition.Action == Actions.DrawPolyline && Shadow.LineType != WorkplaceShadow.LineTypes.Polyline)
                    {
                        Condition.Action = Actions.DrawLine;
                        Shadow.LineType = WorkplaceShadow.LineTypes.Line;
                    }

                     if (Condition.Action == Actions.DrawLine)
                    {
                        Shadow.SetLineSecondPoint(currentMousePos);
                    }
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
                Process.Drag(currentMousePos);
            }
            else
            {
                if(Condition.Action == Actions.DrawPolyline)
                {
                    Shadow.DrawLastShadowtLine(currentMousePos);
                }
            }

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                WorkplaceProcess.MovingWorkPlace(RMB_firstPoint ,currentMousePos);
            }
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition(WorkPlace);

            if (Condition.Action == Actions.DrawRect)
            {
                Condition.Action = Actions.None;
                Process.CreateRect(endPoint);
            }
            else if (Condition.Action == Actions.DrawLine)
            {
                Condition.Action = Actions.None;
                Process.CreateLine(endPoint);
            }
            else if(Condition.Action == Actions.DrawPolyline)
            {
                Shadow.LineType = WorkplaceShadow.LineTypes.Polyline;
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
