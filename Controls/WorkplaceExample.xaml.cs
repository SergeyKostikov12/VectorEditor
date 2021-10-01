using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphicEditor.Functionality;

namespace GraphicEditor.Controls
{
    /// <summary>
    /// Логика взаимодействия для WorkplaceExample.xaml
    /// </summary>
    public partial class WorkplaceExample : UserControl
    {
        Condition_ Condition = new Condition_();
        DetermineClick Determine;
        Figure SelectedFigure;
        public WorkplaceExample()
        {
            InitializeComponent();
            Determine = new DetermineClick(Condition);
        }



        private void WorkPlaceCanvasExample_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Determine.Click(MouseAction.LMB_Down);
            SetAction();
        }


        private void WorkPlaceCanvasExample_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Determine.Click(MouseAction.LMB_Up);
        }

        private void WorkPlaceCanvasExample_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Determine.Click(MouseAction.RMB_Down);
            
        }


        private void WorkPlaceCanvasExample_MouseMove(object sender, MouseEventArgs e)
        {
            Determine.Click(MouseAction.Move);
            Move();
        }

        private void Move()
        {
            
        }

        private void SetAction()
        {
            if (Condition.Mode == Mode.DrawRectMode)
            {
                Condition.Action = Action.DrawRect;
                return;
            }
            else if (Condition.Mode == Mode.DrawLineMode)
            {
                Condition.Action = Action.DrawLine;
                return;
            }
            TrySelectFigure();
        }

        private void TrySelectFigure()
        {
            
        }

        internal void ReadyDrawRectangle()
        {
            Condition.Mode = Mode.DrawRectMode;
        }

        internal void ReadyDrawLine()
        {
            Condition.Mode = Mode.DrawLineMode;
        }
    }
}
