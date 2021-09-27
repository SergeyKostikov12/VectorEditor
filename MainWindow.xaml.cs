using GraphicEditor.Functionality;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private Workplace Workplace;
        private FigureProcess Process;
        //private WorkplaceCondition Condition;
        //private WorkplaceShadow Shadow;
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
            Workplace.LoadWorkplace();
        }


        private void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Save;
            Workplace.DeselectFigure();
            Workplace.SaveWorkplace();
        }
        private void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Rect;
            Workplace.DeselectFigure();
            SetCursor(CursorType.Crosshair);
        }
        private void LineButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Line;
            Workplace.DeselectFigure();
            SetCursor(CursorType.Crosshair);
        }
        private void DeleteButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.DeleteFigure();
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
            LMB_ClickPosition = e.GetPosition(WorkPlaceCanvas);
            Workplace.SetLMB_ClickPosition(LMB_ClickPosition);
            Condition.MouseDown = true;

            if (Condition.ButtonPressed == ButtonPressed.Rect)
            {
                Condition.Action = Actions.DrawRect;
                Shadow.StartDrawRectShadow(LMB_ClickPosition);
                Workplace.SetLMB_ClickPosition(LMB_ClickPosition);
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
                Condition.Action = Workplace.DetermindAction(LMB_ClickPosition);
            }

            if (e.ClickCount == 2)
            {
                Rectangle rect = Process.ExecuteDoubleClick(LMB_ClickPosition);
                Workplace.AddToWorkplace(rect);
            }
        }
        private void WorkPlace_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            RMB_firstPoint = e.GetPosition(WorkPlaceCanvas);
            SetCursor(CursorType.Arrow);
            Workplace.DeselectFigure();
            if (Condition.Action == Actions.DrawLine)
            {
                Shadow.RemoveLastPoint();
                Workplace.CreatePolyline(Shadow.GetShadowLine());
                Shadow.Clear();
            }
            Condition.ResetCondition();
        }
        private void WorkPlace_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentMousePos = e.GetPosition(WorkPlaceCanvas);
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
                Workplace.MovingWorkPlace(Scroll, RMB_firstPoint, currentMousePos);
            }
        }
        private void WorkPlace_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition(WorkPlaceCanvas);
            Condition.MouseUp = true;

            if (Condition.Action == Actions.DrawRect)
            {
                Condition.Action = Actions.None;
                Workplace.CreateRect(endPoint);
                Shadow.Clear();
            }
            else if (Condition.Action == Actions.DrawLine)
            {
                if (Condition.IsDrawLine())
                {
                    if (Shadow.GetShadowLine().Points.Count <= 2)
                    {
                        Condition.Action = Actions.None;
                        Workplace.CreateLine(LMB_ClickPosition, endPoint);
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
            Workplace = new Workplace(WorkPlaceCanvas);
            Process = new FigureProcess(Workplace);
            Condition = new WorkplaceCondition();
            Shadow = new WorkplaceShadow(WorkPlaceCanvas);
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
                    WorkPlaceCanvas.Cursor = Cursors.Hand;
                    break;
                case CursorType.Crosshair:
                    WorkPlaceCanvas.Cursor = Cursors.Cross;
                    break;
                case CursorType.Arrow:
                    WorkPlaceCanvas.Cursor = Cursors.Arrow;
                    break;
                default:
                    WorkPlaceCanvas.Cursor = Cursors.Arrow;
                    break;
            }
        }
        private void SetWidth()
        {
            WidthPicker.Figure = Workplace.GetSelectedFigure();
            WidthPicker.Visibility = Visibility.Visible;
        }
    }
}
