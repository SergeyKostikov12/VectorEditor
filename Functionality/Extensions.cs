using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

public static class Extensions
{
    public static Point AbsDeltaTo(this Point point, Point newPosition)
    {
        Point absDelta = new Point(Math.Abs(newPosition.X - point.X), Math.Abs(newPosition.Y - point.Y));
        return absDelta;
    }
    public static Point DeltaTo(this Point point, Point newPosition)
    {
        Point delta = new Point(newPosition.X - point.X, newPosition.Y - point.Y);
        return delta;
    }
    public static bool Near(this Point pt, Point point)
    {
        Point delta = pt.AbsDeltaTo(point);
        if (delta.X <= 5 && delta.Y <= 5) return true;
        else return false;
    }
    public static Point ParseEx(string deserealizingString)
    {
        return Point.Parse(deserealizingString.Replace(',', '.').Replace(';', ','));
    }
    public static Point OffsetEx(this Point thisP, Point firstPoint, Point endPoint)
    {
        Point delta = firstPoint.DeltaTo(endPoint);
        thisP.X += delta.X;
        thisP.Y += delta.Y;

        return thisP;
    }
    public static Point Move(this Point thisPoint, Point newPosition)
    {
        Point delta = thisPoint.DeltaTo(newPosition);
        return new Point(thisPoint.X + delta.X, thisPoint.Y + delta.Y);
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

        if (c.AngleBetweenPoints(a, b) > 60)
        {
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
        return false;
    }
    public static double Length(this Point point, Point secondPoint)
    {
        return Math.Sqrt(Math.Pow(secondPoint.X - point.X, 2) + Math.Pow(secondPoint.Y - point.Y, 2));
    }
    public static double AngleBetweenPoints(this Point centre, Point first, Point second)
    {
        Point A = first;
        Point B = second;
        Point C = centre;

        Vector CA = new Vector(A.X - C.X, A.Y - C.Y);
        Vector CB = new Vector(B.X - C.X, B.Y - C.Y);

        double angle = Math.Abs(Vector.AngleBetween(CA, CB));
        return angle;
    }
    public static Point Add(this Point point, Point addedPoint)
    {
        Point tmp = new Point(point.X + addedPoint.X, point.Y + addedPoint.Y);
        return tmp;
    }

    public static Point ParsePoint( this string deserealizedString)
    {
        return Point.Parse(deserealizedString.Replace(',', '.').Replace(';', ','));
    }
    public static Polyline ParsePolylineFromArray( this string[] polyline)
    {
        Polyline tmpLine = new Polyline();
        for (int i = 0; i < polyline.Length; i++)
        {
            tmpLine.Points.Add(ParsePoint(polyline[i]));
        }
        return tmpLine;
    }
}