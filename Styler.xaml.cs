using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicEditor
{
    public partial class ColorPicker : Page
    {
        private Button colorBtn;
        private SolidColorBrush Brush { get ; set ; }

        public ColorPicker()
        {
            InitializeComponent();
            FillTable();
            this.Visibility = Visibility.Hidden;
        }

        private void FillTable()
        {
            var values = typeof(Brushes).GetProperties().
                  Select(p => new { p.Name, Brush = p.GetValue(null) as Brush }).
                  ToArray();

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
                    Button btn = CreateButton(values[x].Brush);
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

        public void Hide()
        {
            this.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Brush = (SolidColorBrush)(sender as Button).Background;
            colorBtn.Background = Brush;
            this.Visibility = Visibility.Hidden;
        }

        internal void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        internal void GetButtonColor(object sender)
        {
            colorBtn = (Button)sender;
        }
    }
}
