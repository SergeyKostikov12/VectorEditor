using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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
        public void StartDraw()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void EndDraw()
        {
            throw new NotImplementedException();
        }

        public void AddPoint()
        {
            throw new NotImplementedException();
        }
    }
    public class ShadowLine : IDraw
    {
        public void AddPoint()
        {
            throw new NotImplementedException();
        }

        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void EndDraw()
        {
            throw new NotImplementedException();
        }

        public void StartDraw()
        {
            throw new NotImplementedException();
        }
    }

    interface IDraw
    {
        void StartDraw();
        void Draw();
        void EndDraw();
        void AddPoint();
    }
}
