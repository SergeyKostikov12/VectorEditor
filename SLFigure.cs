using GraphicEditor.Functionality;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Xml.Serialization;

[Serializable]
public class SLFigure
{
    [XmlElement] public int FigureTypeNumber;
    [XmlElement] public string AnchorPoint;
    [XmlElement] public string MovePoint;
    [XmlElement] public string FillColor;
    [XmlElement] public string LineColor;
    [XmlElement] public string LineStrokeThinkness;

    [XmlArray] public string[] Polyline;
    [XmlArray] public string[] Markers;

    public SLFigure()
    {

    }
    public SLFigure CreateSLFigureFromFigureObject(Figure figureObject)
    {
        if (figureObject.FigureType == FigureType.Rectangle)
        {
            CreateRectangle(figureObject);
        }
        else
        {
            CreatePolyline(figureObject);
        }
        return this;
    }

    private void CreatePolyline(Figure figureObject)
    {
        LineFigure line = (LineFigure)figureObject;
        FigureTypeNumber = ((int)line.FigureType);
        LineStrokeThinkness = line.StrokeWidth.ToString();
        LineColor = line.LineColor.Color.ToString();
        Polyline = CreatePolilineString(line.GetPolyline());
        Markers = CreateMarkersString(line.GetMarkerPoints());
        //Polyline = CreatePolilineString(line.polyline);
        //Markers = CreateMarkersString(line.markers);
    }
    private void CreateRectangle(Figure figureObject)
    {
        RectangleFigure rectangle = (RectangleFigure)figureObject;
        FigureTypeNumber = ((int)rectangle.FigureType);
        MovePoint = rectangle.GetMoveMarker().ToString();
       // MovePoint = rectangle.MovePoint.Point.ToString();
        LineStrokeThinkness = rectangle.StrokeWidth.ToString();
        LineColor = rectangle.LineColor.Color.ToString();
        FillColor = rectangle.Fill.Color.ToString();
        Polyline = CreatePolilineString(rectangle.GetRectangle());
        //Polyline = CreatePolilineString(rectangle.Rectangle);
    }
    private string[] CreatePolilineString(Polyline polyline)
    {
        string[] array = new string[polyline.Points.Count];
        for (int i = 0; i < polyline.Points.Count; i++)
        {
            array[i] = polyline.Points[i].ToString();
        }
        return array;
    }
    private string[] CreateMarkersString(List<MarkerPoint> markers)
    {
        string[] array = new string[markers.Count];
        for (int i = 0; i < markers.Count; i++)
        {
            array[i] = markers[i].Point.ToString();
        }
        return array;
    }
}
