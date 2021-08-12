using GraphicEditor.Functionality;
using System.Windows;
using System.Windows.Controls;

namespace GraphicEditor
{
    public partial class WidthPicker : Page
    {
        private int width;
        public FigureObject Figure;

        public WidthPicker()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Figure.Shape.StrokeThickness = width;
            this.Visibility = Visibility.Hidden;
        }

        private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            width = (int)WidthSlider.Value;
            WidthText.Text = width.ToString();
        }
    }
}
