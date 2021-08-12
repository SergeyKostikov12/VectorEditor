using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphicEditor
{
    /// <summary>
    /// Логика взаимодействия для WidthPicker.xaml
    /// </summary>
    public partial class WidthPicker : Page
    {
        private int width;
        public FigureObject Figure;
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
            Figure.Shape.StrokeThickness = width;
            this.Visibility = Visibility.Hidden;
        }
    }
}
