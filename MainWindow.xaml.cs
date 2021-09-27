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
        private Workplace workplace;
        private FigureProcess FigureProcess;
        //private WorkplaceCondition Condition;
        //private WorkplaceShadow Shadow;
        private Serializator serializator;
        private ColorPicker ColoRPicker = new ColorPicker();
        private WidthPicker WidthPicker = new WidthPicker();

        //private Point LMB_ClickPosition;
        //private Point RMB_firstPoint;

        public MainWindow()
        {
            InitializeComponent();
            InitializeProcessors();
        }

        private void LoadButtonPress(object sender, RoutedEventArgs e)
        {


            serializator.Save(workplace.GetFigures());
            workplace.LoadWorkplace();
        }
        private void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            workplace.SaveWorkplace();
        }
        private void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            workplace.StartDrawRectangle();
        }
        private void LineButtonPress(object sender, RoutedEventArgs e)
        {
            workplace.StartDrawLine();
        }
        private void DeleteButtonPress(object sender, RoutedEventArgs e)
        {
            workplace.DeleteFigure();
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
            FigureProcess.SetFigureLineColor((SolidColorBrush)LineColorField.Background);
        }
        private void PaintFillColorButton()
        {
           FigureProcess.SetFigureFillColor((SolidColorBrush)FillColorField.Background);
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

        private void InitializeProcessors()
        {
            Workplace workplace = new Workplace();
            WorkplacePanel.Content = workplace;


            //остальное ненужно!!!
            //workplace = new Workplace(WorkPlaceCanvas, Scroll);
            FigureProcess = new FigureProcess(workplace);
            //Condition = new WorkplaceCondition();
            //Shadow = new WorkplaceShadow(WorkPlaceCanvas);
        }
        


        //private void SetCursor(CursorType cursorType)
        //{
        //    switch (cursorType)
        //    {
        //        case CursorType.Hand:
        //            WorkPlaceCanvas.Cursor = Cursors.Hand;
        //            break;
        //        case CursorType.Crosshair:
                    
        //            break;
        //        case CursorType.Arrow:
        //            WorkPlaceCanvas.Cursor = Cursors.Arrow;
        //            break;
        //        default:
        //            WorkPlaceCanvas.Cursor = Cursors.Arrow;
        //            break;
        //    }
        //}
        private void SetWidth()
        {
            //WidthPicker.Figure = workplace.GetSelectedFigure();
            WidthPicker.Visibility = Visibility.Visible;
        }

    }
}
