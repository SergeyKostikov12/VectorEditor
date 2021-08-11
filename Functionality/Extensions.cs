using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

public static class Extensions
{
    public static Point AbsDeltaTo(this Point point, Point newPosition)
    {
        Point absDelta = new Point(Math.Abs(newPosition.X - point.X), Math.Abs(newPosition.Y - point.Y));
        return absDelta;
    }
    public static Point DeltaTo(this Point point, Point newPosition)
    {
        Point absDelta = new Point(newPosition.X - point.X, newPosition.Y - point.Y);
        return absDelta;
    }
    public static bool Near(this Point pt, Point point)
    {
        Point delta = pt.AbsDeltaTo(point);
        if (delta.X <= 5 && delta.Y <= 5) return true;
        else return false;
    }
}