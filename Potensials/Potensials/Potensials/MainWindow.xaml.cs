using OxyPlot;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Potensials
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<Point>[] points = new List<Point>[2];

        public Function separetFunction = null;
        private List<Series> Series { get; set; }
        public List<DataPoint> TestPoints { get; private set; } = new List<DataPoint>
                              {
                                  new DataPoint(0, 4),
                                  new DataPoint(10, 13),
                                  new DataPoint(20, 15),
                                  new DataPoint(30, 16),
                                  new DataPoint(40, 12),
                                  new DataPoint(50, 12)
                              };

        public MainWindow()
        {
            InitializeComponent();
            class1point1x1.Text = "-1";
            class1point1y1.Text = "0";
            class1point2x1.Text = "1";
            class1point2y1.Text = "1";
            class2point1x1.Text = "2";
            class2point1y1.Text = "0";
            class2point2x1.Text = "1";
            class2point2y1.Text = "-2";
            points[0] = new List<Point>();
            points[1] = new List<Point>();

            

           
        }

        private void DrawSeries(List<DataPoint> points, Color color)
        {
            LineSeries lineSeries = new LineSeries { ItemsSource = points, Color = color };
            mainPlot.Series.Add(lineSeries);
            mainPlot.InvalidatePlot();
        }

        private void DrawMarker(int x, int y, Color color)
        {
            LineSeries lineSeries1 = new LineSeries
            {
                ItemsSource = new List<DataPoint>
                { new DataPoint(x,y) },
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = color
            };
            mainPlot.Series.Add(lineSeries1);
            mainPlot.InvalidatePlot();
        }

        private void btnTeach_Click(object sender, RoutedEventArgs e)
        {
            mainPlot.Series.Clear();
            mainPlot.InvalidatePlot();

            var potintials = new Potintials();
            var teaching = new Point[2][];

            teaching[0] = new Point[2];
            teaching[0][0] = new Point(Convert.ToInt32(class1point1x1.Text), 
                Convert.ToInt32(class1point1y1.Text));
            teaching[0][1] = new Point(Convert.ToInt32(class1point2x1.Text),
                Convert.ToInt32(class1point2y1.Text));

            points[0].Add(teaching[0][0]);
            points[0].Add(teaching[0][1]);

            teaching[1] = new Point[2];
            teaching[1][0] = new Point(Convert.ToInt32(class2point1x1.Text),
                Convert.ToInt32(class2point1y1.Text));
            teaching[1][1] = new Point(Convert.ToInt32(class2point2x1.Text),
                Convert.ToInt32(class2point2y1.Text));
            points[1].Add(teaching[1][0]);
            points[1].Add(teaching[1][1]);

            separetFunction = potintials.GetFunction(teaching);
            lblFunction.Content = separetFunction.ToString();

            DrawAllMarkers(this.points);
            DrawFunction();
        }

        private void DrawAllMarkers(List<Point>[] points)
        {
            foreach (Point point in points[0])
            {
                DrawMarker((int)point.X, (int)point.Y, Colors.Green);
            }

            foreach (Point point in points[1])
            {
                DrawMarker((int)point.X, (int)point.Y, Colors.Yellow);
            }
        }

        private void DrawFunction()
        {
            List<DataPoint> points1 = new List<DataPoint>();
            List<DataPoint> points2 = new List<DataPoint>();
            int sign = 0;
            bool ToOtherCollection = false;
            for (double x = -5; x < 5; x += 0.1)
            {
                x = Math.Round(x, 4);

                double y = separetFunction.GetY(x);

                if (sign == 0)
                {
                    if (y > 0) sign = 1;
                    else sign = -1;
                }
                
                if (!ToOtherCollection)
                {
                    if (!((y > 0 && sign == 1) || (y < 0 && sign == -1)))
                    {
                        ToOtherCollection = true;
                    }
                }
                
                if (ToOtherCollection)
                {
                    points2.Add(new DataPoint(x, y));
                }
                else
                {
                    points1.Add(new DataPoint(x, y));
                }

                
            }

            DrawSeries(points1, Colors.Red);
            DrawSeries(points2, Colors.Red);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x = int.Parse(tbX.Text);
                int y = int.Parse(tbY.Text);
                Point point = new Point(x, y);
                int classNumber = separetFunction.GetValue(point) >= 0 ? 0 : 1;
                points[classNumber].Add(point);
                if (classNumber == 1)
                {
                    DrawMarker(x, y, Colors.Yellow);
                }
                else
                {
                    DrawMarker(x, y, Colors.Green);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error", ex.Message);
            }
            
        }
    }
}
