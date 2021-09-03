using GraphicEditor.Functionality;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Serialization;

[Serializable]
public class SLFigure
{
    [XmlElement] public string Name;
    [XmlElement] public int ShapeTypeNumber;
    [XmlElement] public string PivotPoint;
    [XmlElement] public string CenterPoint;
    [XmlElement] public string RotatePoint;
    [XmlElement] public string OutlinePivotPoint;
    [XmlElement] public string Size;
    [XmlElement] public string FillColor;
    [XmlElement] public string LineColor;
    [XmlElement] public string LineStrokeThinkness;
    [XmlArray] public string[] CenterGizmo;
    [XmlArray] public string[] Outline;
    [XmlArray] public string[][] GizmosRectagles;
    [XmlArray] public string[] Polyline;
    [XmlIgnore] public Rectangle CenterGizmoRectangle;
    [XmlIgnore] public Rectangle OutlineRectangle;
    [XmlIgnore] public SolidColorBrush FillBrush;
    [XmlIgnore] public SolidColorBrush LineColorBrush;
    [XmlIgnore] public FigureType _ShapeType;


    public SLFigure()
    {

    }
    private string[] CreateStringFromRect(Rectangle rectangle)
    {
        if (rectangle != null)
        {
            string[] str = new string[8];
            str[0] = rectangle.Name;
            str[1] = rectangle.Width.ToString();
            str[2] = rectangle.Height.ToString();
            str[3] = rectangle.StrokeThickness.ToString();
            str[4] = rectangle.Visibility.ToString();
            str[5] = Canvas.GetLeft(rectangle).ToString();
            str[6] = Canvas.GetTop(rectangle).ToString();
            str[7] = rectangle.StrokeDashArray.ToString();
            return str;
        }
        else return null;
    }
    private string[] CreatePoliline(Polyline polyline)
    {
        string[] array = new string[polyline.Points.Count];
        for (int i = 0; i < polyline.Points.Count; i++)
        {
            array[i] = polyline.Points[i].ToString();
        }
        return array;
    }
    public SLFigure CreateSLFigureFromFigureObject(FigureObj figureObject)
    {
        //Name = figureObject.Name;
        //ShapeTypeNumber = ((int)figureObject.ShapeType);
        //PivotPoint = figureObject.AnchorPoint.ToString();
        //CenterPoint = figureObject.CenterPoint.ToString();
        //RotatePoint = figureObject.RotatePoint.ToString();
        //OutlinePivotPoint = figureObject.OutlinePivotPoint.ToString();
        //Size = figureObject.Size.ToString();
        //FillColor = figureObject.Fill.Color.ToString();
        //LineColor = figureObject.LineColor.Color.ToString();
        //LineStrokeThinkness = figureObject.StrokeWidth.ToString();
        //CenterGizmoRectangle = figureObject.CenterGizmo;
        //CenterGizmo = CreateStringFromRect(CenterGizmoRectangle);
        //OutlineRectangle = figureObject.Outline;
        //Outline = CreateStringFromRect(OutlineRectangle);
        //Polyline = CreatePoliline(figureObject.Polyline);

        //GizmosRectagles = new string[figureObject.Gizmos.Count][];
        //for (int i = 0; i < figureObject.Gizmos.Count; i++)
        //{
        //    GizmosRectagles[i] = CreateStringFromRect(figureObject.Gizmos[i]);
        //}

        return this;
    }
}
