using GraphicEditor.Functionality;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace GraphicEditor
{
    public class FigureProcess
    {
        public FigureObj selectedFigure;

        private Canvas workPlace;
        private WorkplaceProcess workplaceProcess;
        private Point lMB_ClickPosition;
        private Point rMB_ClickPosition;
        public Point LMB_ClickPosition { get => lMB_ClickPosition; set => lMB_ClickPosition = value; }
        public Point RMB_ClickPosition { get => rMB_ClickPosition; set => rMB_ClickPosition = value; }
        public FigureObj SelectedFigure
        {
            get
            {
                return selectedFigure = workplaceProcess.SelectedFigure;
            }
        }

        public FigureProcess(Canvas _workPlace, WorkplaceCondition _condition, WorkplaceProcess _workplaceProcess)
        {
            workPlace = _workPlace;
            workplaceProcess = _workplaceProcess;
        }
        internal void ExecuteDoubleClick(Point point)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.ExecuteDoubleClick(point);
            }
        }

        public void Drag(Point position)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.MoveMarker(position);
            }
        }
        internal void DeleteFigure() 
        {
            if (SelectedFigure != null)
            {
                selectedFigure.DeleteFigureFromWorkplace(workPlace);
                workplaceProcess.AllFigures.Remove(SelectedFigure);
                workplaceProcess.DeselectFigure();
            }
            else MessageBox.Show("Сначала выделите объект!");
        }
        internal void CreateRect(Point endPoint)
        {
            FigureObj figure = new RectangleObj(LMB_ClickPosition, endPoint);
            figure.PlacingInWorkPlace(workPlace);
            workplaceProcess.AllFigures.Add(figure);
        }
        internal void CreateLine(Point firstPoint, Point endPoint)
        {
            FigureObj figure = new LineObj(firstPoint, endPoint);
            figure.PlacingInWorkPlace(workPlace);
            workplaceProcess.AllFigures.Add(figure);
        }
        internal void CreatePolyline(Polyline shadowLine)
        {
            shadowLine.Points.RemoveAt(0);
            FigureObj figure = new LineObj(shadowLine);
            figure.PlacingInWorkPlace(workPlace);
            workplaceProcess.AllFigures.Add(figure);
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
