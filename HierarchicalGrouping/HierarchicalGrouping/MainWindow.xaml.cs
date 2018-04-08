using HierarchyAlgorithm;
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

namespace HierarchicalGrouping
{   
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int N = 4;
        private MainViewModel mainViewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mainViewModel;
            tbCount.Text = Convert.ToString(N);
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int N = int.Parse(tbCount.Text);
                if (N >= 100)
                {
                    throw new Exception("Количество колонок не может превышать 99");
                }
                mainViewModel.SetDistanses(N);
                mainDataGrid.Items.Refresh();
                this.N = N;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private bool IsDistancesArrayValid(List<List<int>> distances)
        {
            var result = true;
            for (var i = 0; i < this.N; i++)
            {
                for (var j = 0; j < this.N; j++)
                {
                    if (distances[i][j] != distances[j][i]) result = false;
                    if (i == j && distances[i][j] != 0) result = false;
                }
            }
            return result;
        }

        private void btnMinimum_Click(object sender, RoutedEventArgs e)
        {
            if (!IsDistancesArrayValid(mainViewModel.Distanses))
            {
                MessageBox.Show(
                    "Данные в талице некорректны");
                return;
            }
            var tableElements = new List<TableElement>();
            for (var i = 0; i < this.N; i++)
            {
                tableElements.Add(new TableElement(i));
            }
            var hierarchy = new MinHierarchy(this.N,355,400);


            ResultImage.Source = new DrawingImage(hierarchy.GetDrawingGroup(mainViewModel.Distanses, tableElements));
        }

        private void btnMaximum_Click(object sender, RoutedEventArgs e)
        {
            if(!IsDistancesArrayValid(mainViewModel.Distanses))
            {
                MessageBox.Show(
                    "Данные в талице некорректны");
                return;
            }
            var tableElements = new List<TableElement>();
            for (var i = 0; i < this.N; i++)
            {
                tableElements.Add(new TableElement(i));
            }
            var hierarchy = new InvertMaxHierarchy(this.N, 355, 400);


            ResultImage.Source = new DrawingImage(hierarchy.GetDrawingGroup(mainViewModel.Distanses, tableElements));
        }
    }
}
