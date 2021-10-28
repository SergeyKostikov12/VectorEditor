using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicEditor.Controls
{
    public partial class ColorPicker : UserControl
    {
        private string[] colors = new string[] 
        { "#FF1493", "#C71585", "#FF0000", "#B22222", "#8B0000", "#FF6347", "#FF4500", 
            "#FFD700", "#FFFF00", "#00FF00", "#00FA9A", "#006400", "#20B2AA", "#0000FF", 
            "#191970", "#FFFFFF", "#808080", "#696969", "#778899", "#2F4F4F", "#000000" };

        public event ColorPickEventHandler ColorPick;
        public ColorPicker()
        {
            InitializeComponent();
            FillTable();
            Visibility = Visibility.Hidden;
        }

        private void FillTable()
        {
            Grid grid = new Grid();
            grid.Margin = new Thickness(0, 0, 10, 5);

            int x = 0;
            for (int c = 0; c < 7; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int r = 0; r < 3; r++)
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = GridLength.Auto;
                    grid.RowDefinitions.Add(rd);

                    var color = (Color)ColorConverter.ConvertFromString(colors[x]);
                    SolidColorBrush solidColor = new SolidColorBrush(color);
                    Button btn = CreateButton(solidColor);
                    Grid.SetColumn(btn, c);
                    Grid.SetRow(btn, r);
                    grid.Children.Add(btn);
                    x++;
                }
            }
            this.Content = grid;
        }
        private Button CreateButton(Brush color)
        {
            Button button = new Button();
            button.Background = color;
            button.Margin = new Thickness(0, 0, 0, 0);
            button.Click += Button_Click;
            button.Height = 20;
            return button;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)(sender as Button).Background;
            ColorPick.Invoke(this, new ColorPickEventArgs(color));
        }
    }
}
