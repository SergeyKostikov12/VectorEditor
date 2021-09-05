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
    public class FigureProcess
    {
        private Canvas workPlace;
        private WorkplaceProcess workplaceProcess;

        public FigureObj selectedFigure;
        private Point lMB_ClickPosition;
        private Point rMB_ClickPosition;

        public FigureObj SelectedFigure
        {
            get
            {
                return selectedFigure = workplaceProcess.SelectedFigure;
            }
        }
        public Point LMB_ClickPosition { get => lMB_ClickPosition; set => lMB_ClickPosition = value; }
        public Point RMB_ClickPosition { get => rMB_ClickPosition; set => rMB_ClickPosition = value; }

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

        internal void DeleteFigure() // TODO: Сделать удаление из Workplace
        {
            if (SelectedFigure != null)
            {
                selectedFigure.DeleteFigureFromWorkplace(workPlace);
                workplaceProcess.AllFigures.Remove(SelectedFigure);
                workplaceProcess.DeselectFigure();
            }
            else MessageBox.Show("Сначала выделите объект!");
        }

        public void Drag(Point position)
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.MoveMarker(position);
            }
        }

        internal void MoveRect(Point currentMousePos) // TODO: Перемещение от "Точки перемещения"
        {
        }

        internal void RotateRect(Point currentMousePos) // TODO: Вращение от "Точки вращения"
        {
        }

        internal void ScaleRect(Point currentMousePos) // TODO: Увеличение от "Точки увеличения"
        {
        }
        internal void MovePoint(Point currentMousePos)
        {
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




        internal void AddPoint(Point clickPosition)
        {
            throw new NotImplementedException();
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
