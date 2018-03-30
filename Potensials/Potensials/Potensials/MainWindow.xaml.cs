using OxyPlot;
using OxyPlot.Series;
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

        private void btnTeach_Click(object sender, RoutedEventArgs e)
        {
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

        }
    }
}
