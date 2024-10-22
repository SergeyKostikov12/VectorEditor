﻿using Microsoft.Win32;
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
    public class Storage
    {
        private string fileName;
        private Stream stream;
        private FiguresList figuresList = new FiguresList();
        private List<Figure> figures = new List<Figure>();

        public List<Figure> Load()
        {
            GetStream();
            CreateSerializableFiguresList();
            CreateFiguresFromSerializableList();
            return figures;
        }
        public void Save(List<Figure> allFigures)
        {
            if (!OpenFileDialog())
                return;
            figures = allFigures;
            CreateSaveList();
            Serialize();
        }

        private void GetStream()
        {
            Stream myStream;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Vector Files(*.vec)|*.vec|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == false)
                return;

            if ((myStream = openFileDialog.OpenFile()) == null)
                return;

            stream = myStream;
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
            try
            {
                for (int i = 0; i < figuresList.Figures.Count; i++)
                {
                    figures.Add(Figure.Create(figuresList.Figures[i]));
                }
            }
            catch
            {
                MessageBox.Show("Невозможно загрузить файл!!");
            }

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
    }
}
