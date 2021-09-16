using GraphicEditor.Functionality;
using System.Windows;
using System.Windows.Shapes;

namespace GraphicEditor
{
    public class FigureProcess
    {
        private Figure selectedFigure;

        private WorkplaceProcess workplaceProcess;
        private Figure SelectedFigure
        {
            get
            {
                return selectedFigure = workplaceProcess.GetSelectedFigure();
            }
        }

        public FigureProcess(WorkplaceProcess _workplaceProcess)
        {
            workplaceProcess = _workplaceProcess;
        }
        internal Rectangle ExecuteDoubleClick(Point point)
        {
            if (SelectedFigure != null)
            {
                Rectangle rect = SelectedFigure.ExecuteDoubleClick(point);
                return rect;
            }
            return null;
        }

        public void Drag(Point position)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.MoveMarker(position);
            }
        }
       
        internal void ExecuteRelize(Point endPoint)
        {
            if (selectedFigure != null)
            {
                selectedFigure.ExecuteRelize(endPoint);
            }
        }
    }
}
