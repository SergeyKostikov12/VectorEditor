using GraphicEditor.Functionality;
using System.Windows;
using System.Windows.Media;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private Serializator Serializator = new Serializator();
        private ColorPicker ColoRPicker = new ColorPicker();
        private WidthPicker WidthPicker = new WidthPicker();
        private Workplace Workplace = new Workplace();


        public MainWindow()
        {
            InitializeComponent();
            WorkplacePanel.Content = Workplace;
        }

        private void LoadButtonPress(object sender, RoutedEventArgs e)
        {
            var figures = Serializator.Load();
            Workplace.LoadWorkplace(figures);
        }
        public void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            var figures = Workplace.GetAllFigures();
            Serializator.Save(figures);
        }
        public void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.ReadyDrawRectangle();
            
        }
        public void LineButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.ReadyDrawLine();
        }
        public void DeleteButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.DeleteFigure();
        }
        private void ThinknessButtonPress(object sender, RoutedEventArgs e)
        {
            ToolPanel.Content = WidthPicker;
            ColoRPicker.Hide();
            WidthPicker.Show();
            WidthPicker.Figure = Workplace.GetSelectedFigure();
        }
        private void LineColorButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.SetFigureLineColor((SolidColorBrush)LineColorField.Background);
        }
        private void FillButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.SetFigureFillColor((SolidColorBrush)FillColorField.Background);
        }
        private void LineColorFieldButtonPress(object sender, RoutedEventArgs e)
        {
            ToolPanel.Content = ColoRPicker;
            WidthPicker.Hide();
            ColoRPicker.Show();
            ColoRPicker.GetButtonColor(sender);
        }
        private void FillColorFieldButtonPress(object sender, RoutedEventArgs e)
        {
            ToolPanel.Content = ColoRPicker;
            ColoRPicker.Show();
            ColoRPicker.GetButtonColor(sender);
        }
    }
}
