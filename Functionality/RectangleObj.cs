using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public class RectangleObj : FigureObj
    {
        private MarkerPoint moveMarker;
        private MarkerPoint rotateMarker;
        private MarkerPoint scaleMarker;

        public MarkerPoint MovePoint { get => moveMarker; }
        public MarkerPoint RotatePoint { get => rotateMarker; }
        public MarkerPoint ScalePoint { get => scaleMarker; }

        public Size Size { get; }
        public Rectangle OutlineRectangle { get; }
        public Rectangle Rectangle { get; }
        public SolidColorBrush Fill { get; }


        public RectangleObj(Point firstPoint, Point secondPoint)
        {

        }

        public override void MoveFigure(Point newPosition)
        {

        }

        private void SetAnchorRectangle()
        {
        }
    }
}
