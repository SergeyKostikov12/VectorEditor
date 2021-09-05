using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace GraphicEditor.Functionality
{
    public class WorkplaceProcess
    {
        public List<FigureObj> AllFigures = new List<FigureObj>();
        public FiguresList FiguresList = new FiguresList();
        public FigureObj SelectedFigure;

        private Canvas workplace;
        private Point scrollPoint = new Point(0, 0);

        public WorkplaceProcess(Canvas _worklace)
        {
            workplace = _worklace;
        }
        public void ClearCanvas()
        {
            workplace.Children.Clear();
            AllFigures.Clear();
        }

        internal void LoadWorkplace()
        {
            FiguresList.Figures.Clear();
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vector Files(*.vec)|*.vec|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            var xmlSerializer = new XmlSerializer(typeof(FiguresList));
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            FiguresList sL = (FiguresList)xmlSerializer.Deserialize(myStream);
                            FiguresList = sL;
                        }
                    }
                    CreateFiguresFromList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                workplace.UpdateLayout();
            }
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
        internal void MovingWorkPlace(ScrollViewer Scroll, Point rMB_firstPoint, Point currentMousePos)
        {
            scrollPoint.X = Scroll.HorizontalOffset - rMB_firstPoint.DeltaTo(currentMousePos).X / 2;
            scrollPoint.Y = Scroll.VerticalOffset - rMB_firstPoint.DeltaTo(currentMousePos).Y / 2;
            Scroll.ScrollToHorizontalOffset(scrollPoint.X);
            Scroll.ScrollToVerticalOffset(scrollPoint.Y);
        }
        internal void DeselectFigure()
        {
            if (SelectedFigure != null)
            {
                SelectedFigure.HideOutline();
                SelectedFigure.DeselectFigure();
                SelectedFigure = null;
            }
        }
        internal Actions DetermindAction(Point clickPosition)
        {
            if (SelectedFigure == null)
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
                SelectedFigure.SelectMarker(clickPosition);
                return Actions.MovePoint;
            }

            return Actions.None;
        }

        private void CreateFiguresFromList()
        {
            for (int i = 0; i < FiguresList.Figures.Count; i++)
            {
                if ((FigureType)FiguresList.Figures[i].FigureTypeNumber == FigureType.Rectangle)
                {
                    FigureObj figure = new RectangleObj(FiguresList.Figures[i]);
                    AllFigures.Add(figure);
                    figure.PlacingInWorkPlace(workplace);
                }
                else if ((FigureType)FiguresList.Figures[i].FigureTypeNumber == FigureType.Line)
                {
                    FigureObj figure = new LineObj(FiguresList.Figures[i]);
                    AllFigures.Add(figure);
                    figure.PlacingInWorkPlace(workplace);
                }
            }
        }
        private void CreateSaveList(List<FigureObj> list)
        {
            FiguresList = new FiguresList();
            List<SLFigure> tmp = new List<SLFigure>();
            for (int i = 0; i < list.Count; i++)
            {
                SLFigure sLFigure = new SLFigure();
                sLFigure = sLFigure.CreateSLFigureFromFigureObject(AllFigures[i]);
                tmp.Add(sLFigure);
            }
            FiguresList.Figures = tmp;
        }
        private void SetSelectedFigure(FigureObj figure)
        {
            SelectedFigure = figure;
        }
    }
}
