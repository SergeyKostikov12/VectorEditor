using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace GraphicEditor.Functionality
{
    public class Workplace
    {
        //public FiguresList FiguresList = new FiguresList();
        //private Serializator serializator = new Serializator();
        private Canvas workplace;
        private WorkplaceCondition Condition = new WorkplaceCondition();
        public List<Figure> AllFigures = new List<Figure>();
        private Point lMB_ClickPosition;
        private Figure selectedFigure;
        private Point scrollPoint = new Point(0, 0);

        public Workplace(Canvas _worklace)
        {
            workplace = _worklace;
        }

        internal void LoadWorkplace()
        {
            Condition.ButtonPressed = ButtonPressed.Load;
            Serializator serializator = new Serializator();
            serializator.Deserealize();
            var list = serializator.GetFiguresList();
            if (list.Count == 0) return;
            DeselectFigure();
            ClearCanvas();

        }


        internal void SaveWorkplace()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".vec";
            dlg.Filter = "Vector documents (.vec)|*.vec";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                if (dlg.FileName.Length != 0)
                {
                    File.Delete(dlg.FileName);
                }
                string filename = dlg.FileName;
                CreateSaveList(AllFigures);
                XmlSerializer formatter = new XmlSerializer(FiguresList.GetType());
                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, FiguresList);
                }
                MessageBox.Show("Файл успешно сохранен!");
            }
        }
        internal void SetLMB_ClickPosition(Point LMB_ClickPositionPoint)
        {
            lMB_ClickPosition = LMB_ClickPositionPoint;
        }
        internal void MovingWorkPlace(ScrollViewer Scroll, Point rMB_firstPoint, Point currentMousePos)
        {
            scrollPoint.X = Scroll.HorizontalOffset - rMB_firstPoint.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rMB_firstPoint.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
        }
        internal void DeselectFigure()
        {
            if (selectedFigure != null)
            {
                selectedFigure.HideOutline();
                selectedFigure.DeselectFigure();
                selectedFigure = null;
            }
        }
        internal void AddToWorkplace(Rectangle rect)
        {
            if (rect != null)
            {
                workplace.Children.Add(rect);
            }
        }
        
        private void PlacingInWorkPlace(Figure figure)
        {
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
        }
        private void CreateSaveList(List<Figure> list)
        {
            FiguresList = new FiguresList();
            List<SerializableFigure> tmp = new List<SerializableFigure>();
            for (int i = 0; i < list.Count; i++)
            {
                SerializableFigure sLFigure = new SerializableFigure();
                sLFigure = sLFigure.CreateSLFigureFromFigureObject(AllFigures[i]);
                tmp.Add(sLFigure);
            }
            FiguresList.Figures = tmp;
        }
        private void SetSelectedFigure(Figure figure)
        {
            selectedFigure = figure;
        }
        internal void DeleteFigure()
        {
            if (selectedFigure != null)
            {
                workplace.Children.Remove(selectedFigure.GetShape());
                var markers = selectedFigure.GetMarkers();
                foreach (var marker in markers)
                {
                    workplace.Children.Remove(marker);
                }
                AllFigures.Remove(selectedFigure);
                DeselectFigure();
            }
            else MessageBox.Show("Сначала выделите объект!");
        }
        internal void CreateRect(Point endPoint)
        {
            Figure figure = new RectangleFigure(lMB_ClickPosition, endPoint);
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }
        internal void CreateLine(Point firstPoint, Point endPoint)
        {
            Figure figure = new LineFigure(firstPoint, endPoint);
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
            //AllFigures.Add(figure);
        }
        internal void CreatePolyline(Polyline shadowLine)
        {
            shadowLine.Points.RemoveAt(0);
            Figure figure = new LineFigure(shadowLine);
            workplace.Children.Add(figure.GetShape());
            var markers = figure.GetMarkers();
            foreach (var marker in markers)
            {
                workplace.Children.Add(marker);
            }
            AllFigures.Add(figure);
        }
        internal Actions DetermindAction(Point clickPosition)
        {
            if (selectedFigure == null)
            {
                foreach (var figure in AllFigures)
                {
                    if (figure.SelectMarker(clickPosition) == true)
                    {
                        SetSelectedFigure(figure);
                        figure.ShowOutline();
                        return Actions.Ready;
                    }
                    else if (figure.SelectLine(clickPosition) == true)
                    {
                        SetSelectedFigure(figure);
                        figure.ShowOutline();
                        return Actions.Ready;
                    }
                    else figure.HideOutline();
                }
            }
            else
            {
                selectedFigure.SelectMarker(clickPosition);
                return Actions.MovePoint;
            }

            return Actions.None;
        }
        public Figure GetSelectedFigure()
        {
            return selectedFigure;
        }
        public void ClearCanvas()
        {
            workplace.Children.Clear();
            AllFigures.Clear();
        }
    }
}
