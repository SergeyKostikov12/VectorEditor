using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace GraphicEditor.Functionality
{
    public class Serializator
    {
        private string fileName;
        private Stream stream;
        private FiguresList figuresList = new FiguresList();
        private List<Figure> figures = new List<Figure>();

        public List<Figure> Load()
        {
            if(!GetStream()) 
                return figures;

            CreateSerializableFiguresList();
            CreateFiguresFromSerializableList();
            return figures;
        }
        public void Save(List<Figure> allFigures)
        {
            figures = allFigures;
            if(!OpenFileDialog()) 
                return;

            CreateSaveList();
            Serialize();
        }
        public List<Figure> GetFiguresList()
        {
            return figures;
        }

        private bool GetStream()
        {
            Stream myStream;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vector Files(*.vec)|*.vec|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == false) 
                return false;

            if ((myStream = openFileDialog.OpenFile()) == null) 
                return false;

            stream = myStream;
            return true;
        }
        private void Serialize()
        {

            XmlSerializer formatter = new XmlSerializer(figuresList.GetType());
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, figuresList);
            }
            MessageBox.Show("Файл успешно сохранен!");
        }
        private bool OpenFileDialog()
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "";
            saveFileDialog.DefaultExt = ".vec";
            saveFileDialog.Filter = "Vector documents (.vec)|*.vec";
            Nullable<bool> result = saveFileDialog.ShowDialog();
            if (result == false) 
                return false;

            if (saveFileDialog.FileName.Length != 0)
            {
                File.Delete(saveFileDialog.FileName);
            }

            fileName = saveFileDialog.FileName;
            return true;
        }
        private void CreateSaveList()
        {
            figuresList = new FiguresList();
            List<SerializableFigure> tmp = new List<SerializableFigure>();
            for (int i = 0; i < figures.Count; i++)
            {
                SerializableFigure sLFigure = new SerializableFigure();
                sLFigure = sLFigure.CreateSLFigureFromFigureObject(figures[i]);
                tmp.Add(sLFigure);
            }
            figuresList.Figures = tmp;
        }
        private void CreateSerializableFiguresList()
        {
            if (stream == null) 
                return;

            try
            {
                using (stream)
                {
                    var xmlSerializer = new XmlSerializer(typeof(FiguresList));
                    FiguresList sL = (FiguresList)xmlSerializer.Deserialize(stream);
                    figuresList = sL;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CreateFiguresFromSerializableList()
        {
            for (int i = 0; i < figuresList.Figures.Count; i++)
            {
                if ((FigureType)figuresList.Figures[i].FigureTypeNumber == FigureType.Rectangle)
                {
                    Figure figure = new RectangleFigure(figuresList.Figures[i]);
                    figures.Add(figure);
                }
                else if ((FigureType)figuresList.Figures[i].FigureTypeNumber == FigureType.Line)
                {
                    Figure figure = new LineFigure(figuresList.Figures[i]);
                    figures.Add(figure);
                }
            }
        }
    }
}
