using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor
{
    public class WorkplaceProcess
    {
        private Canvas workPlace;
        
        private FigureObj selectedFigure;
        private Point lMB_ClickPosition;
        private Point rMB_ClickPosition;

        public Canvas WorkPlace { get => workPlace; set => workPlace = value; }

        public FigureObj SelectedFigure { get => selectedFigure; set => selectedFigure = value; }
        public Point LMB_ClickPosition { get => lMB_ClickPosition; set => lMB_ClickPosition = value; }
        public Point RMB_ClickPosition { get => rMB_ClickPosition; set => rMB_ClickPosition = value; }

        public WorkplaceProcess(Canvas workPlace)
        {
            WorkPlace = workPlace;
        }




        internal void DeselectFigure()
        {
            throw new NotImplementedException();
        }

        internal void LoadWorkplace()
        {
            throw new NotImplementedException();
        }

        internal void SaveWorkplace()
        {
            throw new NotImplementedException();
        }

        internal void ExecuteOneClick(Point point)
        {
            throw new NotImplementedException();
        }

        internal void ExecuteDoubleClick(Point point)
        {
            throw new NotImplementedException();
        }

        internal void Drag(Point point)
        {
            throw new NotImplementedException();
        }

        internal void MovingWorkPlace(Point currentMousePos)
        {
            throw new NotImplementedException();
        }

        internal void DeleteFigure()
        {
            throw new NotImplementedException();
        }

        internal void MoveRect(Point currentMousePos)
        {
            throw new NotImplementedException();
        }

        internal void RotateRect(Point currentMousePos)
        {
            throw new NotImplementedException();
        }

        internal void ScaleRect(Point currentMousePos)
        {
            throw new NotImplementedException();
        }
    }
}
