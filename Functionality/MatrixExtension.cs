using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace GraphicEditor.Functionality
{
    public static class MatrixExtension
    {
        public static Point TransformPoint(this Matrix @this, Point _point)
        {
            var point = new Point(_point.X, _point.Y);

            @this.TransformPoint(point);

            return point;
        }
    }
}
