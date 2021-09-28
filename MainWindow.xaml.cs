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
        private WorkplaceCondition Condition = new WorkplaceCondition();
        private Serializator serializator = new Serializator();
        private ColorPicker ColoRPicker = new ColorPicker();
        private WidthPicker WidthPicker = new WidthPicker();
        private Workplace workplace;


        public MainWindow()
        {
            InitializeComponent();
            InitializeProcessors();
        }

        private void LoadButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Load;
            var figures = serializator.Load();
            workplace.LoadWorkplace(figures);
        }
        public void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Save;
            var figures = workplace.GetAllFigures();
            serializator.Save(figures);
        }
        public void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Rect;
            workplace.StartDrawRectangle();
        }
        public void LineButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Line;
            workplace.StartDrawLine();
        }
        public void DeleteButtonPress(object sender, RoutedEventArgs e)
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
        public void PaintLineColorButton()
        {
            workplace.SetFigureLineColor((SolidColorBrush)LineColorField.Background);
        }
        public void PaintFillColorButton()
        {
           workplace.SetFigureFillColor((SolidColorBrush)FillColorField.Background);
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
            workplace = new Workplace(Condition);
            WorkplacePanel.Content = workplace;
        }
        


        
        private void SetWidth()
        {
            WidthPicker.Figure = workplace.GetSelectedFigure();
            WidthPicker.Visibility = Visibility.Visible;
        }

    }
}
