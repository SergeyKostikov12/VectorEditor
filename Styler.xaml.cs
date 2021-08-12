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
    public partial class ColorPicker : Page
    {
        private Brush brush;
        FigureObject figureObject;
        public BtnPressed BtnPressed;

        public Brush Brush { get => brush; set => brush = value; }
        public FigureObject FigureObject { get => figureObject; set => figureObject = value; }

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
            if (BtnPressed == BtnPressed.Color)
            {
                FigureObject.LineColor = (sender as Button).Background;
                BtnPressed = BtnPressed.None;
            }
            else if(BtnPressed == BtnPressed.Fill)
            {
                FigureObject.Fill = (sender as Button).Background;
                BtnPressed = BtnPressed.None;
            }

            this.Visibility = Visibility.Hidden;
        }

        public Brush GetColor()
        {
            this.Visibility = Visibility.Hidden;
            return Brush;
        }
    }
}
