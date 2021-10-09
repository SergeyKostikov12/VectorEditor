using System.Windows.Media;

namespace GraphicEditor
{
    public delegate void ColorPickEventHandler(object sender, ColorPickEventArgs e);
    public class ColorPickEventArgs
    {
        public SolidColorBrush Color { get; }

        public ColorPickEventArgs(SolidColorBrush color)
        {
            Color = color;
        }
    }
}
