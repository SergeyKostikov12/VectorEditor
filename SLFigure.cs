using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

public class SLFigure
{
    [XmlElement] public string Name;
    [XmlElement] public string PivotPoint;
    [XmlElement] public string CenterPoint;
    [XmlElement] public string RotatePoint;
    [XmlElement] public string OutlinePivotPoint;
    [XmlElement] public string Size;
    [XmlArray] public string[] CenterGizmo;
    [XmlArray] public string[] Outline;
    [XmlArray] public string[][] GizmosRectagles;
    [XmlArray] public string[] Polyline;
    [XmlAttribute] public string FillColor;
    [XmlElement] public string LineColor;
    [XmlElement] public int ShapeTypeNumber;
    [NonSerialized] public Rectangle CenterGizmoRectangle;
    [NonSerialized] public Rectangle OutlineRectangle;
    [NonSerialized] public SolidColorBrush FillBrush;
    [NonSerialized] public SolidColorBrush LineColorBrush;
    [XmlEnum] public ShapeType _ShapeType;


    public SLFigure()
    {

    }

    public SLFigure CreateSLFigureFromFigureObject(FigureObject figureObject)
    {
        Name = figureObject.Name;
        PivotPoint = figureObject.PivotPoint.ToString();
        CenterPoint = figureObject.CenterPoint.ToString();
        RotatePoint = figureObject.RotatePoint.ToString();
        OutlinePivotPoint = figureObject.OutlinePivotPoint.ToString();
        Size = figureObject.Size.ToString();
        CenterGizmoRectangle = figureObject.CenterGizmo;
        CenterGizmo = CreateStringFromRect(CenterGizmoRectangle);
        OutlineRectangle = figureObject.Outline;
        Outline = CreateStringFromRect(OutlineRectangle);

        GizmosRectagles = new string[figureObject.Gizmos.Count][];
        for (int i = 0; i <figureObject.Gizmos.Count; i++)
        {
            GizmosRectagles[i] = CreateStringFromRect(figureObject.Gizmos[i]);
        }

        Polyline = CreatePoliline(figureObject.Polyline);
        FillColor = figureObject.Fill.Color.ToString();
        LineColor = figureObject.LineColor.Color.ToString();
        ShapeTypeNumber = ((int)figureObject.ShapeType);
        return this;
    }

    private string[] CreateStringFromRect(Rectangle rectangle)
    {
        string[] str = new string[9];
        str[0] = rectangle.Name;
        str[1] = rectangle.Width.ToString();
        str[2] = rectangle.Height.ToString();
        str[3] = rectangle.StrokeThickness.ToString();
        str[4] = rectangle.Visibility.ToString();
        str[5] = Canvas.GetLeft(rectangle).ToString();
        str[6] = Canvas.GetTop(rectangle).ToString();
        str[7] = rectangle.Visibility.ToString();
        str[8] = rectangle.StrokeDashArray.ToString();
        return str;
    }
    private string[] CreatePoliline(Polyline polyline)
    {
        string[] array = new string[polyline.Points.Count];
        for (int i = 0; i<polyline.Points.Count; i++)
        {
            array[i] = polyline.Points[i].ToString();
        }
        return array;
    }
}
