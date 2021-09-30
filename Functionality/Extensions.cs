using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

public static class Extensions
{
    public static Point DeltaTo(this Point point, Point newPosition)
    {
        Point delta = new Point(newPosition.X - point.X, newPosition.Y - point.Y);
        return delta;
    }
    public static Point AbsDeltaTo(this Point point, Point newPosition)
    {
        Point absDelta = new Point(Math.Abs(newPosition.X - point.X), Math.Abs(newPosition.Y - point.Y));
        return absDelta;
    }
    public static bool ItInsideCircle(this Point centre, Point point, int lineThinkness)
    {
        double radius = lineThinkness + 10;

        double d = Math.Sqrt(Math.Pow(centre.X - point.X, 2) + Math.Pow(centre.Y - point.Y, 2));
        if (d <= radius)
        {
            return true;
        }
        else return false;
    }
    public static bool ItIntersect(this Point centre, Point firstPoint, Point secondPoint, int lineThinkness)
    {
        Point a = firstPoint;
        Point b = secondPoint;
        Point c = centre;

        if (c.AbsAngleBetweenPoints(a, b) < 60) return false;
        double A = c.Length(b);
        double B = c.Length(a);
        double C = a.Length(b);
        double radius = lineThinkness + 10;
        double p = (A + B + C) / 2;
        double S = Math.Sqrt(p * (p - A) * (p - B) * (p - C));
        double h = 2 * S / C;
        if (h <= radius)
        {
            return true;
        }
        else return false;
    }
    public static double Length(this Point point, Point secondPoint)
    {
        return Math.Sqrt(Math.Pow(secondPoint.X - point.X, 2) + Math.Pow(secondPoint.Y - point.Y, 2));
    }
    public static double AbsAngleBetweenPoints(this Point centre, Point first, Point second)
    {
        Point A = first;
        Point B = second;
        Point C = centre;

        Vector CA = new Vector(A.X - C.X, A.Y - C.Y);
        Vector CB = new Vector(B.X - C.X, B.Y - C.Y);

        double angle = Math.Abs(Vector.AngleBetween(CA, CB));
        return angle;
    }
    public static double AngleBetweenPoints(this Point centre, Point first, Point second)
    {
        Point A = first;
        Point B = second;
        Point C = centre;

        Vector CA = new Vector(A.X - C.X, A.Y - C.Y);
        Vector CB = new Vector(B.X - C.X, B.Y - C.Y);

        double angle = Vector.AngleBetween(CA, CB);
        return angle;
    }
    public static Point ParsePoint(this string deserealizedString)
    {
        return Point.Parse(deserealizedString.Replace(',', '.').Replace(';', ','));
    }
    public static Polyline ParsePolylineFromArray(this string[] polyline)
    {
        Polyline tmpLine = new Polyline();
        for (int i = 0; i < polyline.Length; i++)
        {
            tmpLine.Points.Add(ParsePoint(polyline[i]));
        }
        return tmpLine;
    }
    public static void Show(this Control control)
    {
        control.Visibility = Visibility.Visible;
    }
    public static void Hide(this Control control)
    {
        control.Visibility = Visibility.Hidden;
    }
}