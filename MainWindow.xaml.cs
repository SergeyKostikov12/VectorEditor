using GraphicEditor.Functionality;
using System.Windows;
using System.Windows.Media;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private WorkplaceCondition Condition = new WorkplaceCondition();
        private Serializator Serializator = new Serializator();
        private ColorPicker ColoRPicker = new ColorPicker();
        private WidthPicker WidthPicker = new WidthPicker();
        private Workplace Workplace;


        public MainWindow()
        {
            InitializeComponent();
            InitializeProcessors();
        }

        private void LoadButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Load;
            var figures = Serializator.Load();
            Workplace.LoadWorkplace(figures);
        }
        public void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Save;
            var figures = Workplace.GetAllFigures();
            Serializator.Save(figures);
        }
        public void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Rect;
            Workplace.StartDrawRectangle();
        }
        public void LineButtonPress(object sender, RoutedEventArgs e)
        {
            Condition.ButtonPressed = ButtonPressed.Line;
            Workplace.StartDrawLine();
        }
        public void DeleteButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.DeleteFigure();
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
            Workplace.SetFigureLineColor((SolidColorBrush)LineColorField.Background);
        }
        public void PaintFillColorButton()
        {
           Workplace.SetFigureFillColor((SolidColorBrush)FillColorField.Background);
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
            Workplace = new Workplace(Condition);
            WorkplacePanel.Content = Workplace;
        }
        private void SetWidth()
        {
            WidthPicker.Figure = Workplace.GetSelectedFigure();
            WidthPicker.Visibility = Visibility.Visible;
        }

    }
}
