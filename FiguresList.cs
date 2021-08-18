using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GraphicEditor
{
    [Serializable]
    public class FiguresList
    {
        [XmlArray("Figures"), XmlArrayItem(ElementName = "Figure", Type = typeof(SLFigure))] 
        public List<SLFigure> Figures { get; set; }
    }
}
