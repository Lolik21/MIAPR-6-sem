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
using static Perseptron.PerseptronAlgorithm;

namespace Perseptron
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PerseptronAlgorithm algorithm;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            PerseptronAlgorithm perseptron = new PerseptronAlgorithm();
            try
            {

                var classesCount = int.Parse(tbxClasses.Text);
                var objectsCount = int.Parse(tbxObjects.Text);
                var attibutesCount = int.Parse(tbxAttributes.Text);

                perseptron.Calculate(classesCount, objectsCount, attibutesCount);
                stackBoxes.Children.Clear();
                FillFunction(perseptron.Weights);
                FillClasses(perseptron.Classes);

                for (int i = 0; i < attibutesCount; i++)
                {
                    stackBoxes.Children.Add(new TextBox { FontSize = 18 });
                }

                algorithm = perseptron;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error!", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FillFunction(List<PerceptronObject> functions)
        {
            lvFunctions.Items.Clear();
            lvFunctions.Items.Add("Решающие функции: ");
            for (int i = 0; i < functions.Count; i++)
            {
                string str = String.Format("d{0}(x) = ", i + 1);

                for (int j = 0; j < functions[i].Attribues.Count; j++)
                {
                    int attribute = functions[i].Attribues[j];

                    if (j < functions[i].Attribues.Count - 1)
                        if (attribute >= 0 && j != 0)
                            str += "+" + attribute + String.Format("*x{0}", j + 1);
                        else
                            str += attribute + String.Format("*x{0}", j + 1);
                    else
                        if (attribute >= 0 && j != 0)
                        str += "+" + attribute;
                    else
                        str += attribute;
                }
                lvFunctions.Items.Add(str);
            }
        }

        private void FillClasses(List<PerceptronClass> classes)
        {
            int indexCurrentClass = 1;
            lvClasses.Items.Clear();
            foreach (PerceptronClass currClass in classes)
            {
                int indexCurrentObject = 1;

                lvClasses.Items.Add(String.Format("Класс {0}:", indexCurrentClass));
                foreach (PerceptronObject currentObject in currClass.Objects)
                {
                    string str = String.Format("    Объект {0}: (", indexCurrentObject);

                    for (int j = 0; j < currentObject.Attribues.Count - 1; j++)
                    {
                        int attribute = currentObject.Attribues[j];
                        str += attribute + ",";
                    }
                    str = str.Remove(str.Length - 1, 1);
                    str += ")";
                    lvClasses.Items.Add(str);
                    indexCurrentObject++;
                }
                lvClasses.Items.Add("");
                indexCurrentClass++;
            }

        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            
            var testObject = new PerceptronObject();
            try
            {
                var numbers = new List<int>();

                for (int i = 0; i < stackBoxes.Children.Count; i++)
                {
                    var box = stackBoxes.Children[i] as TextBox;
                    numbers.Add(int.Parse(box.Text));
                }

                testObject.Attribues.AddRange(numbers);
                
                int classn = algorithm.FindClass(testObject);

                tbxFounded.Content = "Класс: " + Convert.ToString(classn);

            }
            catch(Exception ex)
            {
                MessageBox.Show("Error!", ex.Message, 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
