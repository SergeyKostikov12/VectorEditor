using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class MarkerPoint
{
    private double x;
    private double y;
    private Point point;
    private Rectangle marker;
    private int markerSize;
    private Point anchorPoint;

    public double X { get => x; set => SetX(value); }
    public double Y { get => y; set => SetY(value); }
    public Point Point { get => point; set => SetPoint(value); }

    public Rectangle Marker { get => marker; }

    public MarkerPoint(Point pt)
    {
        x = pt.X;
        y = pt.Y;
        point = pt;
        markerSize = 1;
        marker = new Rectangle
        {
            Height = 10 + markerSize,
            Width = 10 + markerSize,
            Fill = Brushes.Blue,
            Stroke = Brushes.Black,
            StrokeThickness = 1,
            Visibility = Visibility.Hidden
        };
        anchorPoint = new Point();
        RefreshAnchorPoint();
    }
    public void Show()
    {
        marker.Visibility = Visibility.Visible;
    }
    public void Hide()
    {
        marker.Visibility = Visibility.Hidden;
    }
    public void Move(Point newPosition)
    {
        SetPoint(newPosition);
        RefreshAnchorPoint();
    }

    private void SetX(double xValue)
    {
        point.X = xValue;
        x = xValue;
    }
    private void SetY(double yValue)
    {
        point.Y = yValue;
        y = yValue;
    }
    private void SetPoint(Point newPoint)
    {
        point = newPoint;
        x = point.X;
        y = point.Y;
    }
    public void SetMarkerSize(int size)
    {
        marker.Height -= markerSize;
        marker.Width -= markerSize;
        markerSize = size;
        marker.Height += markerSize;
        marker.Width += markerSize;
    }
    private void RefreshAnchorPoint()
    {
        Point pt = new Point(X - markerSize / 2 - marker.Width/2, Y - markerSize / 2 - marker.Height/2);
        Canvas.SetLeft(marker, pt.X);
        Canvas.SetTop(marker, pt.Y);
        anchorPoint = pt;
    }
}
