using GraphicEditor.Functionality;
using System.Windows;
using System.Windows.Controls;

namespace GraphicEditor
{
    public partial class WidthPicker : Page
    {
        private int width;
        public FigureObj Figure;
        TextBlock WidthText;
        public WidthPicker()
        {
            AddBlock();
            InitializeComponent();
            Container.Children.Add(WidthText);
        }

        private void AddBlock()
        {
            TextBlock textBlock = new TextBlock();
            WidthText = textBlock;
            Grid.SetColumn(WidthText, 2);
            WidthText.FontSize = 20;
        }

        private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            width = (int)WidthSlider.Value;
            WidthText.Text = width.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Figure.StrokeWidth = width;
            this.Visibility = Visibility.Hidden;
        }
    }
}
