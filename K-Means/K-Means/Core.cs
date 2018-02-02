// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="my">
//   free
// </copyright>
// <summary>
//   Defines the Core type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace K_Means
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// The core.
    /// </summary>
    public class Core
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Core"/> class.
        /// </summary>
        public Core()
        {
            this.Classes = new List<KClass>();
        }

        /// <summary>
        /// Gets or sets the classes.
        /// </summary>
        public List<KClass> Classes { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        public List<Point> Points { get; set; }

        /// <summary>
        /// The do generate.
        /// </summary>
        /// <param name="classesCount">
        /// The classes count.
        /// </param>
        /// <param name="objectsCount">
        /// The objects count.
        /// </param>
        /// <param name="canvasHeight">
        /// The canvas height.
        /// </param>
        /// <param name="canvasWidth">
        /// The canvas width.
        /// </param>
        public void DoGenerate(int classesCount, int objectsCount, int canvasHeight, int canvasWidth)
        {
            this.Classes.Clear();
            this.Points = PointHelper.GetRandomPoints(objectsCount, canvasHeight, canvasWidth).ToList();
            this.SetRandomClasses(this.Classes, this.Points, classesCount);
            this.FillClasses(this.Classes, this.Points);
        }

        /// <summary>
        /// The do calculate.
        /// </summary>
        /// <param name="classesCount">
        /// The classes count.
        /// </param>
        /// <param name="objectsCount">
        /// The objects count.
        /// </param>
        /// <param name="canvasHeight">
        /// The canvas height.
        /// </param>
        /// <param name="canvasWidth">
        /// The canvas width.
        /// </param>
        public void DoCalculate(int classesCount, int objectsCount, int canvasHeight, int canvasWidth)
        {
            if (this.Classes.Count == 0)
            {
                this.DoGenerate(classesCount, objectsCount, canvasHeight, canvasWidth);
            }

            this.DoKMeanCalculation(this.Classes, this.Points);
        }

        /// <summary>
        /// The do k mean calculation.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        public void DoKMeanCalculation(List<KClass> classes, List<Point> points)
        {
            while (this.DoKMeanIteration(classes, points))
            {
            }
        }

        /// <summary>
        /// The do k mean iteration.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DoKMeanIteration(List<KClass> classes, List<Point> points)
        {
            this.ClearClasses(classes);
            this.FillClasses(classes, points);
            return this.ChangeCenters(classes);
        }

        /// <summary>
        /// The clear classes.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        public void ClearClasses(List<KClass> classes)
        {
            foreach (var kclass in classes)
            {
                kclass.Points.Clear();
                kclass.Points.Add(kclass.Center);
            }
        }

        /// <summary>
        /// The fill classes.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        public void FillClasses(List<KClass> classes, List<Point> points)
        {
            foreach (var point in points)
            {
                this.AddNearestPointToClass(point, classes);
            }
        }

        /// <summary>
        /// The add nearest point to class.
        /// </summary>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="classes">
        /// The classes.
        /// </param>
        public void AddNearestPointToClass(Point source, List<KClass> classes)
        {
            var min = double.MaxValue;
            var nearestClass = new KClass();
            foreach (var kclass in classes)
            {
                var dist = PointHelper.GetPointsDistance(kclass.Center, source);
                if (dist < min)
                {
                    nearestClass = kclass;
                    min = dist;
                }
            }

            if (nearestClass.Center != source)
            {
                nearestClass.Points.Add(source);
            }
        }

        /// <summary>
        /// The change centers.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool ChangeCenters(List<KClass> classes)
        {
            bool isNotDiffer = false;
            foreach (var kclass in classes)
            {
                Point point = this.GetNewClassCenterPoint(kclass);
                if (point != kclass.Center)
                {
                    kclass.Center = point;
                    isNotDiffer = true;
                }
            }

            return isNotDiffer;
        }

        /// <summary>
        /// The get new class center point.
        /// </summary>
        /// <param name="kclass">
        /// The k class.
        /// </param>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        public Point GetNewClassCenterPoint(KClass kclass)
        {
            var center = new Point(kclass.Points.Average(point => point.X), kclass.Points.Average(point => point.Y));
            return PointHelper.GetNearestPoint(center, kclass.Points);
        }

        /// <summary>
        /// The set random classes.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        public void SetRandomClasses(List<KClass> classes, List<Point> points, int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var kclass = new KClass() { Center = points[random.Next(points.Count)] };
                kclass.Points = new List<Point> { kclass.Center };
                classes.Add(kclass);
            }
        }
    }
}