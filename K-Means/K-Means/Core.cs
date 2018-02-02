using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace K_Means
{
    public class Core
    {
        public List<KClass> Classes { get; set; }
        public List<Point> Points { get; set; }

        public Core()
        {
            Classes = new List<KClass>();
        }

        public void DoGenerate(int classesCount, int objectsCount, int canvasHeight, int canvasWidth)
        {
            Classes.Clear();
            Points = PointHelper.GetRandomPoints(objectsCount, canvasHeight, canvasWidth).ToList();
            SetRandomClasses(this.Classes, this.Points, classesCount);
            FillClasses(this.Classes,this.Points);
        }

        public void DoCalculate(int classesCount, int objectsCount, int canvasHeight, int canvasWidth)
        {
            if (Classes.Count == 0) DoGenerate(classesCount,objectsCount,canvasHeight,canvasWidth);
            DoKMeanCalculation(Classes, Points);
        }

        public void DoKMeanCalculation(List<KClass> classes, List<Point> points)
        {
            while (DoKMeanIteration(classes, points));
        }

        public bool DoKMeanIteration(List<KClass> classes, List<Point> points)
        {
            ClearClasses(classes);
            FillClasses(classes, points);
            return ChangeCenters(classes);
        }

        public void ClearClasses(List<KClass> classes)
        {
            foreach (var kClass in classes)
            {
                kClass.Points.Clear();
                kClass.Points.Add(kClass.Center);
            }
        }

        public void FillClasses(List<KClass> classes, List<Point> points)
        {
            foreach (var point in points)
            {
                AddNearestPointToClass(point, classes);
            }
        }

        public void AddNearestPointToClass(Point source, List<KClass> classes)
        {
            var min = Double.MaxValue;
            var nearestClass = new KClass();
            foreach (var kClass in classes)
            {
                var dist = PointHelper.GetPointsDistance(kClass.Center, source);
                if (dist < min)
                {
                    nearestClass = kClass;
                    min = dist;
                }
            }

            if (nearestClass.Center != source)
            {
                nearestClass.Points.Add(source);
            }
        }

        public bool ChangeCenters(List<KClass> classes)
        {
            bool isNotDiffer = false;
            foreach (var kClass in classes)
            {
                Point point = GetNewClassCenterPoint(kClass);
                if (point != kClass.Center)
                {
                    kClass.Center = point;
                    isNotDiffer = true;
                }
            }
            return isNotDiffer;
        }

        public Point GetNewClassCenterPoint(KClass kClass)
        {
            var center = new Point(kClass.Points.Average(point => point.X), 
                kClass.Points.Average(point => point.Y));
            return PointHelper.GetNearestPoint(center, kClass.Points);

        }

        public void SetRandomClasses(List<KClass> classes, List<Point> points, int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                var kClass = new KClass() {Center = points[random.Next(points.Count)]};
                kClass.Points = new List<Point> {kClass.Center};
                classes.Add(kClass);
            }
        }
    }
}