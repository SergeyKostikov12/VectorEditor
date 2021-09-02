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
        
        private FigureObj selectedFigure;
        private Point lMB_ClickPosition;
        private Point rMB_ClickPosition;

        public FigureObj SelectedFigure { get => selectedFigure; set => selectedFigure = value; }
        public Point LMB_ClickPosition { get => lMB_ClickPosition; set => lMB_ClickPosition = value; }
        public Point RMB_ClickPosition { get => rMB_ClickPosition; set => rMB_ClickPosition = value; }

        public FigureProcess(Canvas _workPlace, WorkplaceCondition condition)
        {
            workPlace = _workPlace;
        }




        internal void DeselectFigure()
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


        internal void DeleteFigure() // TODO: Сделать удаление из Workplace
        {
            if (selectedFigure != null) selectedFigure.DeletePolyline();
            DeselectFigure();
        }

        internal void MoveRect(Point currentMousePos) // TODO: Перемещение от "Точки перемещения"
        {


            //if (isMoving)
            //{
            //    if (selectedFigure != null)
            //    {
            //        tempPosition = selectedFigure.MoveFigure(tempPosition, currentMousePos);
            //    }
            //}
        }

        internal void RotateRect(Point currentMousePos) // TODO: Вращение от "Точки вращения"
        {


            //if (isRotating)
            //{
            //    if (selectedFigure != null)
            //    {
            //        tempPosition = selectedFigure.RotateFigure(tempPosition, currentMousePos);
            //    }
            //}
        }

        internal void ScaleRect(Point currentMousePos) // TODO: Увеличение от "Точки увеличения"
        {
            if (isScaling)
            {
                if (selectedFigure != null)
                {
                    tempPosition = selectedFigure.ScaleFigure(tempPosition, currentMousePos);
                }
            }
        }

        internal void CreateRect(Point endPoint)
        {
            throw new NotImplementedException();
        }

        internal void CreateLine(Point endPoint)
        {
            throw new NotImplementedException();
        }

        internal void MovePoint(Point currentMousePos)
        {
            if (isLineSelect)
            {
                if (pointNumber != 0)
                {
                    tempPosition = selectedFigure.MovePointToNewPosition(pointNumber, tempPosition, currentMousePos);
                }
            }
        }

        internal Actions DetermindAction(Point clickPosition)
        {
            throw new NotImplementedException();
        }

        internal void AddPoint(Point clickPosition)
        {
            throw new NotImplementedException();
        }
        private string FindCollinearPoint()
        {
            for (int i = 0; i < allFigures.Count; i++)
            {
                FigureObject figure = allFigures[i];
                if (figure.AreCollinear(LMB_ClickPosition))
                {
                    return figure.Name;
                }
            }
            return null;
        }
    }
}
