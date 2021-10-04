using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraphicEditor.Functionality
{
    public abstract class Drawer
    {
        private Canvas Workplace;

        public Drawer(Canvas workplace)
        {
           Workplace = workplace;
        }

        public abstract void StartDrawFigure();
        public abstract void EndDrawFigure();
        public abstract void 
    }
}
