using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GraphicEditor
{
    [Serializable]
    public class FiguresList
    {
        [XmlArray("Figures"), XmlArrayItem(ElementName = "Figure", Type = typeof(SLFigure))]
        public List<SLFigure> Figures = new List<SLFigure>();
    }
}
