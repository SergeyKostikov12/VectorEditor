using GraphicEditor.Controls;
using GraphicEditor.Functionality;
using System.Windows;
using System.Windows.Media;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private ColorPicker ColorPicker = new ColorPicker();
        private WidthPicker WidthPicker = new WidthPicker();

        public MainWindow()
        {
            InitializeComponent();
            ColorPicker.ColorPick += ColorPicker_ColorPick;
            WidthPicker.WidthPick += WidthPicker_WidthPick;
            Workplace.FigureSelect += Workplace_FigureSelect;
            Workplace.FigureDeselect += Workplace_FigureDeselect;
        }

        private void Workplace_FigureSelect(Figure sender, Events.FigureSelectEventArgs e)
        {
            WidthPicker.SetWidth(e.Width);
        }

        private void Workplace_FigureDeselect(Figure figure)
        {
            ColorPicker.Hide();
            WidthPicker.Hide();
        }

        private void LoadButtonPress(object sender, RoutedEventArgs e)
        {
            Storage serializator = new Storage();
            var figures = serializator.Load();
            Workplace.LoadWorkplace(figures);
        }
        private void SaveButtonPress(object sender, RoutedEventArgs e)
        {
            Storage serializator = new Storage();
            var figures = Workplace.GetAllFigures();
            serializator.Save(figures);
        }
        private void RectangleButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.ReadyDrawFigure(DrawingMode.RectangleMode);
        }
        private void LineButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.ReadyDrawFigure(DrawingMode.LineMode);
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
            Workplace.SetFigureLineColor((SolidColorBrush)ColorField.Background);
        }
        private void FillButtonPress(object sender, RoutedEventArgs e)
        {
            Workplace.SetFigureFillColor((SolidColorBrush)ColorField.Background);
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
            ColorField.Background = color;
            ColorPicker.Hide();
        }
    }
}
