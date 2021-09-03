using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace GraphicEditor.Functionality
{
    public class WorkplaceProcess
    {
        public List<FigureObj> allFigures = new List<FigureObj>();
        public FiguresList FiguresList;

        private Canvas workplace;
        private Point scrollPoint = new Point(0, 0);

        public WorkplaceProcess(Canvas _worklace)
        {
            workplace = _worklace;
        }

        internal void LoadWorkplace()
        {
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
                    ClearCanvas();
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
            dlg.FileName = ""; // Default file name
            dlg.DefaultExt = ".vec"; // Default file extension
            dlg.Filter = "Vector documents (.vec)|*.vec"; // Filter files by extension
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                if (dlg.FileName.Length != 0)
                {
                    File.Delete(dlg.FileName);
                }
                string filename = dlg.FileName;
                CreateSaveList(allFigures);
                XmlSerializer formatter = new XmlSerializer(FiguresList.GetType());
                using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, FiguresList);
                }
                MessageBox.Show("Файл успешно сохранен!");
            }
        }
        internal void MovingWorkPlace(ScrollViewer Scroll,Point rMB_firstPoint, Point currentMousePos)
        {
                scrollPoint.X = Scroll.HorizontalOffset - rMB_firstPoint.DeltaTo(currentMousePos).X / 2;
                scrollPoint.Y = Scroll.VerticalOffset - rMB_firstPoint.DeltaTo(currentMousePos).Y / 2;
                Scroll.ScrollToHorizontalOffset(scrollPoint.X);
                Scroll.ScrollToVerticalOffset(scrollPoint.Y);
        }

        private void CreateFiguresFromList()
        {
            for (int i = 0; i < FiguresList.Figures.Count; i++)
            {
                //FigureObj figureObject = new FigureObj(FiguresList.Figures[i], workplace);
                //allFigures.Add(figureObject);
            }
        }
        private void ClearCanvas()
        {
            workplace.Children.Clear();
            //InitializeShadows();
        }
        private void CreateSaveList(List<FigureObj> list)
        {
            FiguresList = new FiguresList();
            List<SLFigure> tmp = new List<SLFigure>();
            for (int i = 0; i < list.Count; i++)
            {
                SLFigure sLFigure = new SLFigure();
                sLFigure = sLFigure.CreateSLFigureFromFigureObject(allFigures[i]);
                tmp.Add(sLFigure);
            }
            FiguresList.Figures = tmp;
        }
    }
}
