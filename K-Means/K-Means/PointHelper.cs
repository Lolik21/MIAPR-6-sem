using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

namespace K_Means
{
    public static class PointHelper
    {
        public static IEnumerable<Point> GetRandomPoints(int count, int canvasHeight, int canvasWidth)
        {
            var random = new Random();
            List<Point> pointList = new List<Point>(count);
            for (int i = 0; i < count; i++)
            {
                pointList.Add(new Point(random.Next(canvasWidth),random.Next(canvasHeight)));
            }
            return pointList;
        }

        public static double GetPointsDistance(Point first, Point second)
        {
            var dx = first.X - second.X;
            var dy = first.Y - second.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static Point GetNearestPoint(Point point, List<Point> points)
        {
            var min = Double.MaxValue;
            var nearestPoint = new Point();
            foreach (var p in points)
            {
                var dist = GetPointsDistance(point, p);
                if (dist < min)
                {
                    min = dist;
                    nearestPoint = p;
                }
            }

            return nearestPoint;
        }

    }
}