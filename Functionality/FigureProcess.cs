using GraphicEditor.Functionality;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphicEditor
{
    public class FigureProcess
    {

        private Workplace workplaceProcess;
        private Figure SelectedFigure
        {
            get
            {
                return workplaceProcess.GetSelectedFigure();
            }
        }

        public FigureProcess(Workplace _workplaceProcess)
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
        internal void ExecuteRelize(Point endPoint)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.ExecuteRelize(endPoint);
            }
        }
        internal void SetFigureLineColor(SolidColorBrush lineColor)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.LineColor = lineColor;
            }
            else
            {
                MessageBox.Show("Сначала выберите объект");
            }
        }
        internal void SetFigureFillColor(SolidColorBrush fillColor)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.Fill = fillColor;
            }
            else
            {
                MessageBox.Show("Сначала выберите объект");
            }

        }
        public void Drag(Point position)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.MoveMarker(position);
            }
        }
    }
}
