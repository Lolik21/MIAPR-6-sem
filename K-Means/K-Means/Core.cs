// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Core.cs" company="no">
//   no
// </copyright>
// <summary>
//   The core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace K_Means
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Documents;

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
        /// The do validate.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool DoMeansValidate()
        {
            return this.Classes.Count != 0 && this.Points.Count != 0;
        }

        /// <summary>
        /// The max min init.
        /// </summary>
        /// <param name="objectsCount">
        /// The objects count.
        /// </param>
        /// <param name="canvasHeight">
        /// The canvas height.
        /// </param>
        /// <param name="canvasWidth">
        /// The canvas width.
        /// </param>
        public void MaxMinInit(int objectsCount, int canvasHeight, int canvasWidth)
        {
            this.Points = PointHelper.GetRandomPoints(objectsCount, canvasHeight, canvasWidth).ToList();
            this.Classes.Clear();
            var rnd = new Random();
            var meansClass = new KClass() { Center = this.Points[rnd.Next(this.Points.Count)] };
            meansClass.Points = new List<Point> { meansClass.Center };
            this.Classes.Add(meansClass);

            double tmp = 0;
            Point farPoint = this.FindFarPoint(this.Points, meansClass,ref tmp);
            meansClass = new KClass() { Center = farPoint };
            meansClass.Points = new List<Point> { farPoint };
            this.Classes.Add(meansClass);
            this.FillClasses(this.Classes, this.Points);
        }

        /// <summary>
        /// The do max min iteration.
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
        public bool DoMaxMinIteration(List<KClass> classes, List<Point> points)
        {
            
            List<KClass> tempClasses = new List<KClass>();
            Point tmpPoint = new Point();
            double tmp2 = double.MinValue;
            foreach (var meansClass in classes)
            {
                double tmp = 0;
                Point far = this.FindFarPoint(meansClass.Points, meansClass, ref tmp);
                meansClass.Max = tmp;
                meansClass.Far = far;
                if (tmp > tmp2)
                {
                    tmpPoint = meansClass.Far;
                    tmp2 = tmp;
                }
            }

            List<double> list = new List<double>();

            for (int i = 0; i < classes.Count-1; i++)
            {
                list.Add(PointHelper.GetPointsDistance(classes[i].Center, classes[i+1].Center));
            }

            double avr = list.Average() / 2;

            if (tmp2 > avr)
            {
                var tmpMeansClass = new KClass() { Center = tmpPoint };
                tmpMeansClass.Points = new List<Point> { tmpPoint };
                classes.Add(tmpMeansClass);
                this.FillClasses(classes, points);
                return true;
            }
            this.FillClasses(classes, points);
            return false;
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
            this.FillClasses(classes, points);
            return this.ChangeCenters(classes);
        }

        /// <summary>
        /// The clear classes.
        /// </summary>
        /// <param name="classes">
        /// The classes.
        /// </param>
        private void ClearClasses(List<KClass> classes)
        {
            foreach (var meansClass in classes)
            {
                meansClass.Points.Clear();
                meansClass.Points.Add(meansClass.Center);
            }
        }

        /// <summary>
        /// The max min validate.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool IsMaxMinValid()
        {
            return this.Classes.Count != 0;
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
        private void FillClasses(List<KClass> classes, List<Point> points)
        {
            this.ClearClasses(classes);
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
        private void AddNearestPointToClass(Point source, List<KClass> classes)
        {
            var min = double.MaxValue;
            var nearestClass = new KClass();
            foreach (var meansClass in classes)
            {
                var dist = PointHelper.GetPointsDistance(meansClass.Center, source);
                if (dist < min)
                {
                    nearestClass = meansClass;
                    min = dist;
                }
            }

            if (nearestClass.Center != source)
            {
                nearestClass.Points.Add(source);
            }
        }

        /// <summary>
        /// The find far point.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <param name="meansClass">
        /// The means class.
        /// </param>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        private Point FindFarPoint(List<Point> points, KClass meansClass, ref double dist)
        {
            double maxDistance = double.MinValue;
            Point farPoint = new Point();
            foreach (var point in points)
            {
                double distance = PointHelper.GetPointsDistance(meansClass.Center, point);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    farPoint = point;
                }
            }

            dist = maxDistance;
            return farPoint;
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
        private bool ChangeCenters(List<KClass> classes)
        {
            bool isNotDiffer = false;
            foreach (var meansClass in classes)
            {
                Point point = this.GetNewClassCenterPoint(meansClass);
                if (point != meansClass.Center)
                {
                    meansClass.Center = point;
                    isNotDiffer = true;
                }
            }

            return isNotDiffer;
        }

        /// <summary>
        /// The get new class center point.
        /// </summary>
        /// <param name="meansClass">
        /// The k class.
        /// </param>
        /// <returns>
        /// The <see cref="Point"/>.
        /// </returns>
        private Point GetNewClassCenterPoint(KClass meansClass)
        {
            var center = new Point(
                meansClass.Points.Average(point => point.X),
                meansClass.Points.Average(point => point.Y));
            return PointHelper.GetNearestPoint(center, meansClass.Points);
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
        private void SetRandomClasses(List<KClass> classes, List<Point> points, int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var meansClass = new KClass() { Center = points[random.Next(points.Count)] };
                meansClass.Points = new List<Point> { meansClass.Center };
                classes.Add(meansClass);
            }
        }
    }
}