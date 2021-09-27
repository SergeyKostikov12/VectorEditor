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
        private Stream stream;
        private FiguresList figuresList = new FiguresList();
        private List<Figure> figures = new List<Figure>();

        public void Deserealize()
        {
            GetStream();
            CreateSerializableFiguresList();
            CreateFiguresFromSerializableList();
        }



        private void GetStream()
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vector Files(*.vec)|*.vec|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == false) return;
            if ((myStream = openFileDialog.OpenFile()) == null) return;
            stream = myStream;
        }

        private List<Figure> GetFiguresList()
        {
            return figures;
        }
        private void CreateSerializableFiguresList()
        {
            if (stream == null) return;
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
