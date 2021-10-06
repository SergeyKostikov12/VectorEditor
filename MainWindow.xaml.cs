using GraphicEditor.Controls;
using GraphicEditor.Functionality;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private Serializator Serializator = new Serializator();
        private ColorPicker ColorPicker = new ColorPicker();
        private WidthPicker WidthPicker = new WidthPicker();

        public MainWindow()
        {
            InitializeComponent();
            ColorPicker.ColorPick += ColorPicker_ColorPick;
            WidthPicker.WidthPick += WidthPicker_WidthPick;
        }


        private void LoadButtonPress(object sender, RoutedEventArgs e)
        {
            var figures = Serializator.Load();
            Workplace.LoadWorkplace(figures);

        }
        private void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            var figures = Workplace.GetAllFigures();
            Serializator.Save(figures);
        }
        private void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.ReadyDrawRectangle();
        }
        private void LineButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.ReadyDrawLine();
        }
        private void DeleteButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.DeleteFigure();
        }
        private void ThinknessButtonPress(object sender, RoutedEventArgs e)
        {
            ToolPanel.Content = WidthPicker;
            ColorPicker.Hide();
            WidthPicker.Show();
        }
        private void LineColorButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.SetFigureLineColor((SolidColorBrush)LineColorField.Background);
        }
        private void FillButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.SetFigureFillColor((SolidColorBrush)FillColorField.Background);
        }
        private void ColorButtonPress(object sender, RoutedEventArgs e)
        {
            ToolPanel.Content = ColorPicker;
            WidthPicker.Hide();
            ColorPicker.Show();
        }
        private void WidthPicker_WidthPick(object sender, WidthPickEventArgs e)
        {
            int width = e.Width;
            Workplace.SetFigureWidth(width);
        }

        private void ColorPicker_ColorPick(object sender, ColorPickEventArgs e)
        {
            var color = e.Color;
            LineColorField.Background = color;
            FillColorField.Background = color;
            ColorPicker.Hide();
        }
    }
}
