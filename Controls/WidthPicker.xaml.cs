using System.Windows;
using System.Windows.Controls;
using GraphicEditor.Events;

namespace GraphicEditor.Controls
{
    public partial class WidthPicker : UserControl
    {
        public event WidthPickEventHandler WidthPick;

        public WidthPicker()
        {
            InitializeComponent();
            
        }

        public void WidthPicker_Select(object sender, FigureSelectEventArgs e)
        {
            Text.Content = e.Width;
            WidthSlider.Value = e.Width;
        }

        private void WidthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            int width = (int)WidthSlider.Value;
            
            Text.Content = width.ToString();

            WidthPick?.Invoke(this, new WidthPickEventArgs(width));
        }

    }
}
