// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointHelper.cs" company="no">
//   no
// </copyright>
// <summary>
//   The point helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace K_Means
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// The point helper.
    /// </summary>
    public static class PointHelper
    {
        /// <summary>
        /// The get random points.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="canvasHeight">
        /// The canvas height.
        /// </param>
        /// <param name="canvasWidth">
        /// The canvas width.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<Point> GetRandomPoints(int count, int canvasHeight, int canvasWidth)
        {
            var random = new Random();
            List<Point> pointList = new List<Point>(count);
            for (int i = 0; i < count; i++)
            {
                pointList.Add(new Point(random.Next(canvasWidth), random.Next(canvasHeight)));
            }

            return pointList;
        }

        /// <summary>
        /// The get points distance.
        /// </summary>
        /// <param name="first">
        /// The first.
        /// </param>
        /// <param name="second">
        /// The second.
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        public static double GetPointsDistance(Point first, Point second)
        {
            var dx = first.X - second.X;
            var dy = first.Y - second.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// The get nearest point.
        /// </summary>
        /// <param name="point">
        /// The point.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        public static Point GetNearestPoint(Point point, List<Point> points)
        {
            var min = double.MaxValue;
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