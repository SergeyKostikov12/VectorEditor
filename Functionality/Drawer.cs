using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GraphicEditor.Functionality
{
    public abstract class Drawer
    {
        private Canvas Workplace;
        public Drawer(Canvas workplace)
        {
            Workplace = workplace;
        }
    }

   

    public class ShadowRect : IDraw
    {
        private Point firstPoint;
        public void StartDraw(Point point)
        {
            firstPoint = point;
        }

        public void Draw(Point currentPosition)
        {
            throw new NotImplementedException();
        }

        public void EndDraw(Point endPoint)
        {
            throw new NotImplementedException();
        }

        public void AddPoint(Point position)
        {
            throw new NotImplementedException();
        }
    }
    public class ShadowLine : IDraw
    {
        public void StartDraw(Point point)
        {
            throw new NotImplementedException();
        }

        public void Draw(Point currentPosition)
        {
            throw new NotImplementedException();
        }

        public void EndDraw()
        {
            throw new NotImplementedException();
        }

        public void EndDraw(Point endPoint)
        {
            throw new NotImplementedException();
        }

        public void AddPoint(Point position)
        {
            throw new NotImplementedException();
        }
    }

    interface IDraw
    {
        void StartDraw(Point point);
        void Draw(Point currentPosition);
        void EndDraw(Point endPoint);
        void AddPoint(Point position);
        Shape GetShape()
    }
}
