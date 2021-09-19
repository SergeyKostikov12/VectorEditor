using GraphicEditor.Functionality;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GraphicEditor
{
    public partial class ColorPicker : Page
    {
        public ButtonPressed BtnPressed;
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
            grid.Margin = new Thickness(0, 0, 0, 0);

            int x = 0;
            for (int c = 0; c < 8; c++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                for (int r = 0; r < 4; r++)
                {
                    RowDefinition rd = new RowDefinition();
                    rd.Height = GridLength.Auto;
                    grid.RowDefinitions.Add(rd);
                    Button btn = CreateButton(values[x].Brush, c, r);
                    Grid.SetColumn(btn, c);
                    Grid.SetRow(btn, r);
                    grid.Children.Add(btn);
                    x++;
                }
            }
            this.Content = grid;
        }
        private Button CreateButton(Brush color, int Col, int Row)
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
            Brush = (SolidColorBrush)(sender as Button).Background;
            colorBtn.Background = Brush;
            this.Visibility = Visibility.Hidden;
        }
        internal SolidColorBrush GetColor(Button lineColorField)
        {
            throw new NotImplementedException();
        }
        internal void GetButtonColor(object sender)
        {
            colorBtn = (Button)sender;
        }
        public Brush GetColor()
        {
            this.Visibility = Visibility.Hidden;
            return Brush;
        }

    }
}
