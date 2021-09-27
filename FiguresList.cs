using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GraphicEditor
{
    [Serializable]
    public class FiguresList
    {
        [XmlArray("Figures"), XmlArrayItem(ElementName = "Figure", Type = typeof(SerializableFigure))]
        public List<SerializableFigure> Figures = new List<SerializableFigure>();
    }
}
