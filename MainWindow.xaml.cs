using GraphicEditor.Functionality;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private WorkplaceProcess WorkplaceProcess;
        private FigureProcess Process;
        private WorkplaceCondition Condition;
        private WorkplaceShadow Shadow;
        private ColorPicker ColoRPicker = new ColorPicker();
        private WidthPicker WidthPicker = new WidthPicker();

        private Point LMB_ClickPosition;
        private Point RMB_firstPoint;

        public MainWindow()
        {
            InitializeComponent();
            InitializeProcessors();
        }

        private void LoadButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Load;
            WorkplaceProcess.ClearCanvas();
            WorkplaceProcess.DeselectFigure();
            WorkplaceProcess.LoadWorkplace();
            WorkplaceShadow workplaceShadow = new WorkplaceShadow(WorkPlace);
            Shadow = workplaceShadow;
        }
        private void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Save;
            WorkplaceProcess.DeselectFigure();
            WorkplaceProcess.SaveWorkplace();
        }
        private void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Rect;
            WorkplaceProcess.DeselectFigure();
            SetCursor(CursorType.Crosshair);
        }
        private void LineButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Line;
            WorkplaceProcess.DeselectFigure();
            SetCursor(CursorType.Crosshair);
        }
        private void DeleteButtonPress(object sender, RoutedEventArgs e)
        {
            WorkplaceProcess.DeleteFigure();
            SetCursor(CursorType.Arrow);
        }
        private void StrokeWidthButtonPress(object sender, RoutedEventArgs e)
        {
            ColoRPicker.Visibility = Visibility.Hidden;
            PropertyPanel.Content = WidthPicker;
            SetWidth();
        }
        private void LineColorButtonPress(object sender, RoutedEventArgs e)
        {
            PaintLineColorButton();
        }
        private void FillButtonPress(object sender, RoutedEventArgs e)
        {
            PaintFillColorButton();
        }
        private void PaintLineColorButton()
        {
            Process.SetFigureLineColor((SolidColorBrush)LineColorField.Background);
        }
        private void PaintFillColorButton()
        {
           Process.SetFigureFillColor((SolidColorBrush)FillColorField.Background);
        }
        private void LineColorFieldButtonPress(object sender, RoutedEventArgs e)
        {
            PropertyPanel.Content = ColoRPicker;
            WidthPicker.Visibility = Visibility.Hidden;
            ColoRPicker.BtnPressed = ButtonPressed.Color;
            ColoRPicker.Visibility = Visibility.Visible;
            ColoRPicker.GetButtonColor(sender);
        }
        private void FillColorFieldButtonPress(object sender, RoutedEventArgs e)
        {
            PropertyPanel.Content = ColoRPicker;
            ColoRPicker.BtnPressed = ButtonPressed.Fill;
            ColoRPicker.Visibility = Visibility.Visible;
            ColoRPicker.GetButtonColor(sender);
        }
        private void WorkPlace_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            LMB_ClickPosition = e.GetPosition(WorkPlace);
            WorkplaceProcess.SetLMB_ClickPosition(LMB_ClickPosition);
            Condition.MouseDown = true;

            if (Condition.ButtonPressed == ButtonPressed.Rect)
            {
                Condition.Action = Actions.DrawRect;
                Shadow.StartDrawRectShadow(LMB_ClickPosition);
                WorkplaceProcess.SetLMB_ClickPosition(LMB_ClickPosition);
            }
            else if (Condition.ButtonPressed == ButtonPressed.Line)
            {
                Condition.Action = Actions.DrawLine;
                if (Shadow.GetShadowLine().Points.Count == 0)
                {
                    Shadow.DrawLineShadow();
                    Shadow.SetLIneFirstPoint(LMB_ClickPosition);
                    Shadow.SetLineSecondPoint(LMB_ClickPosition);
                }
            }
            else if (Condition.ButtonPressed == ButtonPressed.None)
            {
                Condition.Action = WorkplaceProcess.DetermindAction(LMB_ClickPosition);
            }

            if (e.ClickCount == 2)
            {
                Rectangle rect = Process.ExecuteDoubleClick(LMB_ClickPosition);
                WorkplaceProcess.AddToWorkplace(rect);
            }
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            RMB_firstPoint = e.GetPosition(WorkPlace);
            SetCursor(CursorType.Arrow);
            WorkplaceProcess.DeselectFigure();
            if (Condition.Action == Actions.DrawLine)
            {
                Shadow.RemoveLastPoint();
                WorkplaceProcess.CreatePolyline(Shadow.GetShadowLine());
                Shadow.Clear();
            }
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
                    Process.Drag(currentMousePos);
                }
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
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition(WorkPlace);
            Condition.MouseUp = true;

            if (Condition.Action == Actions.DrawRect)
            {
                Condition.Action = Actions.None;
                WorkplaceProcess.CreateRect(endPoint);
                Shadow.Clear();
            }
            else if (Condition.Action == Actions.DrawLine)
            {
                if (Condition.IsDrawLine())
                {
                    if (Shadow.GetShadowLine().Points.Count <= 2)
                    {
                        Condition.Action = Actions.None;
                        WorkplaceProcess.CreateLine(LMB_ClickPosition, endPoint);
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
                Process.ExecuteRelize(endPoint);
            }

        }
        private void InitializeProcessors()
        {
            WorkplaceProcess = new WorkplaceProcess(WorkPlace);
            Process = new FigureProcess(WorkplaceProcess);
            Condition = new WorkplaceCondition();
            Shadow = new WorkplaceShadow(WorkPlace);
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
            WidthPicker.Figure = WorkplaceProcess.GetSelectedFigure();
            WidthPicker.Visibility = Visibility.Visible;
        }
    }
}
