namespace СhanceApproach.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;

    using OxyPlot;

    public class MainViewModel
    {
        public MainViewModel(double[] resultForGreen, double[] resultForRed, int line)
        {
            this.Title = "Example 2";
            this.PointsForRed = new List<DataPoint>();
            this.PointsForGreen = new List<DataPoint>();
            this.Line = new List<DataPoint>();

            for (int i = 0; i < resultForRed.Length; i++)
            {
                this.PointsForRed.Add(new DataPoint(i, resultForRed[i]));
                this.PointsForGreen.Add(new DataPoint(i, resultForGreen[i]));              
            }

            if (resultForGreen.Max() > resultForRed.Max())
            {
                this.Line.Add(new DataPoint(line, resultForGreen.Max()));
                this.Line.Add(new DataPoint(line, 0));
            }
            else
            {
                this.Line.Add(new DataPoint(line, resultForRed.Max()));
                this.Line.Add(new DataPoint(line, 0));
            }
        }

        public string Title { get; private set; }

        public IList<DataPoint> PointsForRed { get;  }

        public IList<DataPoint> PointsForGreen { get; }

        public IList<DataPoint> Line { get; }

    }
}