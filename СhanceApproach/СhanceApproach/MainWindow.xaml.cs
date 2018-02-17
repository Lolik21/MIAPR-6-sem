// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="BSUIR">
//   BSUIR
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace СhanceApproach
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    using OxyPlot;

    using СhanceApproach.ViewModels;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int pointsCount = 1000;

        private const int minGreen = 0;

        private const int maxGreen = 800;

        private const int minRed = 200;

        private const int maxRed = 1000;


        public MainWindow()
        {
            InitializeComponent();

            double value = Math.Round(this.SldForGreen.Value / 100, 2);
            this.LblForGreen.Content = value.ToString(CultureInfo.InvariantCulture);
            value = Math.Round(this.SldForRed.Value / 100, 2);
            this.LblForRed.Content = value.ToString(CultureInfo.InvariantCulture);
        }

        private void DoChanceApproach()
        {
            int actualPlotWidth = (int)this.GrdMainPlot.ActualWidth;

            int[] pointsForGreenPlot = new int[pointsCount];
            int[] pointsForRedPlot = new int[pointsCount];

            double valueForGreen = this.SldForGreen.Value / 100;
            double valueForRed = this.SldForRed.Value / 100;

            Random random = new Random();

            pointsForGreenPlot = pointsForGreenPlot.Select(i => random.Next(minGreen, maxGreen)).ToArray();
            pointsForRedPlot = pointsForRedPlot.Select(i => random.Next(minRed, maxRed)).ToArray();

            double averageForGreenPlot = pointsForGreenPlot.Average();
            double averageForRedPlot = pointsForRedPlot.Average();
            double meanDeviationForRed = this.GetMeanDeviation(pointsForRedPlot, averageForRedPlot);
            double meanDeviationForGreen = this.GetMeanDeviation(pointsForGreenPlot, averageForGreenPlot);


            double[] resultForGreen = new double[actualPlotWidth];
            double[] resultForRed = new double[actualPlotWidth];

            int match = 0;

            for (int i = 0; i < actualPlotWidth; i++)
            {
                resultForGreen[i] = Math.Exp(-0.5 * Math.Pow((i - averageForGreenPlot) / meanDeviationForGreen, 2))
                                    / (meanDeviationForGreen * Math.Sqrt(2 * Math.PI)) * valueForGreen;
                resultForRed[i] = Math.Exp(-0.5 * Math.Pow((i - averageForRedPlot) / meanDeviationForRed, 2))
                                    / (meanDeviationForRed * Math.Sqrt(2 * Math.PI)) * valueForRed;

                if (Math.Abs(resultForGreen[i] - resultForRed[i]) < 0.000002)
                {
                   match = i;
                }
            }

            this.ToDataContext(resultForGreen, resultForRed, match);


            double error1 = resultForRed.Take(match).Sum();
            double error2;
            if (valueForGreen > valueForRed)
            {
                error2 = resultForRed.Skip(match).Sum();
            }
            else
            {
                error2 = resultForGreen.Skip(match).Sum();
            }

            this.TbxError1.Content = Math.Round(error1,5).ToString(CultureInfo.InvariantCulture);
            this.TbxError2.Content = Math.Round(error2, 5).ToString(CultureInfo.InvariantCulture);
            this.TbxErrorRez.Content = Math.Round(error1 + error2, 5).ToString(CultureInfo.InvariantCulture);
        }

        private double GetMeanDeviation(int[] array, double average)
        {
            double dispersion = 0;
            for (int i = 0; i < array.Length; i++)
            {
                dispersion += Math.Pow(array[i] - average, 2);
            }
            return Math.Sqrt(dispersion/ array.Length);
        }

        private void ToDataContext(double[] resultForGreen, double[] resultForRed, int line)
        {
            this.DataContext = new MainViewModel(resultForGreen, resultForRed, line);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DoChanceApproach();
        }

        private void SldForRed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SldForGreen.Value = 100 - this.SldForRed.Value;
            if (this.LblForRed != null)
            {
                double value = Math.Round(this.SldForRed.Value / 100, 2);
                this.LblForRed.Content = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void SldForGreen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SldForRed.Value = 100 - this.SldForGreen.Value;

            if (this.LblForGreen != null)
            {
                double value = Math.Round(this.SldForGreen.Value / 100, 2);
                this.LblForGreen.Content = value.ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
