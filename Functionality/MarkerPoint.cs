using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class MarkerPoint
{
    private Point point;
    private int markerSize;

    public Point Point { get => point; set => SetPoint(value); }
    public Rectangle Marker { get; }

    public MarkerPoint(Point pt)
    {
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
    public void SetMarkerSize(int size)
    {
        Marker.Height -= markerSize;
        Marker.Width -= markerSize;
        markerSize = size;
        Marker.Height += markerSize;
        Marker.Width += markerSize;
        RefreshAnchorPoint();
    }

    private void SetPoint(Point newPoint)
    {
        point = newPoint;
    }
    private void RefreshAnchorPoint()
    {
        Point pt = new Point(Point.X - Marker.Width/2, Point.Y - Marker.Height/2);
        Canvas.SetLeft(Marker, pt.X);
        Canvas.SetTop(Marker, pt.Y);
    }
}
