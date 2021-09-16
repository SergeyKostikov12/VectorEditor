using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class MarkerPoint
{
    private double x;
    private double y;
    private Point point;
    private int markerSize;

    public double X { get => x; set => SetX(value); }
    public double Y { get => y; set => SetY(value); }
    public Point Point { get => point; set => SetPoint(value); }
    public Rectangle Marker { get; }

    public MarkerPoint(Point pt)
    {
        x = pt.X;
        y = pt.Y;
        point = pt;
        markerSize = 1;
        Marker = new Rectangle
        {
            Height = 10 + markerSize,
            Width = 10 + markerSize,
            Fill = Brushes.Blue,
            Stroke = Brushes.Black,
            StrokeThickness = 1,
            Visibility = Visibility.Hidden
        };
        RefreshAnchorPoint();
    }
    public void Show()
    {
        Marker.Visibility = Visibility.Visible;
    }
    public void Hide()
    {
        Marker.Visibility = Visibility.Hidden;
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
        Marker.Height -= markerSize;
        Marker.Width -= markerSize;
        markerSize = size;
        Marker.Height += markerSize;
        Marker.Width += markerSize;
        RefreshAnchorPoint();
    }
    private void RefreshAnchorPoint()
    {
        Point pt = new Point(X - Marker.Width/2, Y - Marker.Height/2);
        Canvas.SetLeft(Marker, pt.X);
        Canvas.SetTop(Marker, pt.Y);
    }
}
