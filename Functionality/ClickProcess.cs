using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraphicEditor
{
    public class ClickProcess
    {
        public ButtonPressed ButtonPressed { get; set; }
        public FigureObj SelectedFigure { get; set; }
        public Canvas WorkPlace { get; set; }

    }
}
