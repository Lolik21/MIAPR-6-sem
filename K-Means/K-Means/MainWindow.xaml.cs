using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

namespace K_Means
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int ClassRadius = 10;
        private const int ObjectRadius = 1;

        private Core core = new Core();
        private bool IsValidClasses = false;
        private bool IsValidObjects = false;

        public MainWindow()
        {
            InitializeComponent();
            LockButtons();
        }

        private void TbxClasses_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).Foreground = Brushes.Black;
            string input = (sender as TextBox).Text;
            if (!Regex.IsMatch(input, @"^\d{1,2}$"))
            {
                (sender as TextBox).Foreground = Brushes.Red;
                LockButtons();
                return;
            }

            IsValidClasses = true;
            UnlockButtons();
        }

        private void TbxObjects_TextChanged(object sender, TextChangedEventArgs e)
        {
            (sender as TextBox).Foreground = Brushes.Black;
            string input = (sender as TextBox).Text;
            if (!Regex.IsMatch(input, @"^\d{1,6}$"))
            {
                (sender as TextBox).Foreground = Brushes.Red;
                LockButtons();
                return;
            }
            IsValidObjects = true;
            UnlockButtons();
        }

        private void LockButtons()
        {
            if (BtnCalculate != null && BtnGenerate != null)
            {
                BtnCalculate.IsEnabled = false;
                BtnGenerate.IsEnabled = false;
            }       
        }

        private void UnlockButtons()
        {
            if (IsValidClasses && IsValidObjects)
            {
                BtnCalculate.IsEnabled = true;
                BtnGenerate.IsEnabled = true;
            }
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            core.DoGenerate(Convert.ToInt32(TbxClasses.Text), Convert.ToInt32(TbxObjects.Text), 
                (int)GrdHelper.ActualHeight, (int)GrdHelper.ActualWidth);
            DrawAll(core);
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            core.DoCalculate(Convert.ToInt32(TbxClasses.Text), Convert.ToInt32(TbxObjects.Text),
                (int)GrdHelper.ActualHeight, (int)GrdHelper.ActualWidth);
            DrawAll(core);
        }

        private void DrawAll(Core core)
        {
            DrawingGroup drawingGroup = new DrawingGroup();
            Random random = new Random();
            foreach (var kClass in core.Classes)
            {
                DrawClass(kClass, drawingGroup, random);
            }
            ImgMain.Source = new DrawingImage(drawingGroup);


        }

        private void DrawClass(KClass kClass, DrawingGroup drawingGroup, Random random)
        {
            GeometryGroup geometryEllipsesGroup = new GeometryGroup();
            foreach (var point in kClass.Points)
            {
                geometryEllipsesGroup.Children.Add(point == kClass.Center
                    ? new EllipseGeometry(point, ClassRadius, ClassRadius)
                    : new EllipseGeometry(point, ObjectRadius, ObjectRadius));
            }        
            Brush brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(0, 256), 
                (byte)random.Next(0, 256), (byte)random.Next(0, 256)));
            GeometryDrawing geometryDrawing = new GeometryDrawing(brush, new Pen(brush, 1), geometryEllipsesGroup);
            drawingGroup.Children.Add(geometryDrawing);
        }
    }
}
