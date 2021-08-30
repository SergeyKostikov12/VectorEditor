using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    class LineObj : FigureObj
    {
        private List<Rectangle> linePoints;

        public LineObj(Point firstPoint, Point secondPoint)
        {
            linePoints = new List<Rectangle>();
        }

        public LineObj(Point firstPoint)
        {
            linePoints = new List<Rectangle>();
        }

        public override void MoveFigure(Point position)
        {

        }

        protected override int GetStrokeWidth()
        {
            throw new NotImplementedException();
        }

        protected override void SetStrokeWidth(int value)
        {
            throw new NotImplementedException();
        }

        public override void ShowOutline()
        {
            throw new NotImplementedException();
        }

        public override void HideOutline()
        {
            throw new NotImplementedException();
        }

        public override void PlacingInWorkPlace(Canvas canvas)
        {
            throw new NotImplementedException();
        }
    }
}
