using System;
using System.Collections.Generic;
using System.IO;
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

namespace Images
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Rectangle> Androids { get; set; }
        public List<Rectangle> Apples { get; set; }

        public Dictionary<int, List<Rectangle>> AndroidApps { get; set; }
        public Dictionary<int, List<Rectangle>> IPhoneApps { get; set; }

        public List<Rectangle> nitherAppleApps { get; set; }
        public List<Rectangle> nitherAndroidApps { get; set; }

        public Dictionary<int, Rectangle> AndroidAppRectangles { get; set; }
        public Dictionary<int, Rectangle> IphoneAppRectangles { get; set; }

        public Rectangle AndroidLogo { get; set; }
        public Rectangle AppleLogo { get; set; }

        public Image ImageToDraw { get; set; }

        private void Clear()
        {
            AndroidApps.Clear();
            Androids.Clear();
            IPhoneApps.Clear();
            Apples.Clear();
            AndroidLogo = null;
            AppleLogo = null;
            mainCanvas.Children.Clear();
            nitherAppleApps.Clear();
            nitherAndroidApps.Clear();
            AndroidAppRectangles.Clear();
            IphoneAppRectangles.Clear();
        }

        public MainWindow()
        {
            InitializeComponent();
            string[] images = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.png");

            Androids = new List<Rectangle>();
            Apples = new List<Rectangle>();
            AndroidApps = new Dictionary<int, List<Rectangle>>();
            IPhoneApps = new Dictionary<int, List<Rectangle>>();
            AndroidAppRectangles = new Dictionary<int, Rectangle>();
            IphoneAppRectangles = new Dictionary<int, Rectangle>();
            nitherAndroidApps = new List<Rectangle>();
            nitherAppleApps = new List<Rectangle>();
        }



        private void btnIPhone_Click(object sender, RoutedEventArgs e)
        {
            ImageToDraw = new Image
            {
                Source = new BitmapImage(new Uri("Images/iphone.png", UriKind.Relative)),
                Width = 300,
                Height = 600,
                Stretch = Stretch.Fill,
                Tag = new Action<Point>((point) => AddNewIPhone(point))
            };
        }

        private void AddNewAndroid(Point point)
        {
            var androidRect = new Rectangle
            {
                X = point,
                Y = new Point
                { X = point.X + 300, Y = point.Y + 600 }
            };
            if (!Androids.Any() || CheckIfPhonesOver(androidRect))
            {
                Androids.Add(androidRect);
                AndroidAppRectangles.Add(Androids.Count - 1, new Rectangle
                {
                    X = new Point(androidRect.X.X + 20, androidRect.X.Y + 56),
                    Y = new Point(androidRect.Y.X - 22, androidRect.Y.Y - 91)
                });
                mainCanvas.Children.Add(ImageToDraw);
            }
            else
            {
                MessageBox.Show("Images cannot be over other images");
            }
        }

        private void AddNewIPhone(Point point)
        {
            var appleRect = new Rectangle
            {
                X = point,
                Y = new Point
                { X = point.X + 300, Y = point.Y + 600 }
            };
            if (!Apples.Any() || CheckIfPhonesOver(appleRect))
            {
                Apples.Add(appleRect);
                IphoneAppRectangles.Add(Apples.Count - 1, new Rectangle
                {
                    X = new Point(appleRect.X.X + 25, appleRect.X.Y + 90),
                    Y = new Point(appleRect.Y.X - 22, appleRect.Y.Y - 83)
                });
                mainCanvas.Children.Add(ImageToDraw);
            }
            else
            {
                MessageBox.Show("Images cannot be over other images");
            }
        }

        private bool CheckIfPhonesOver(Rectangle rectangle)
        {
            foreach(var rect in Androids)
            {
                if (rectangle.IsOver(rect))
                {
                    return false;
                }
            }
            foreach (var rect in Apples)
            {
                if (rectangle.IsOver(rect))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnAndroid_Click(object sender, RoutedEventArgs e)
        {
            ImageToDraw = new Image
            {
                Source = new BitmapImage(new Uri("Images/android.png", UriKind.Relative)),
                Width = 300,
                Height = 600,
                Stretch = Stretch.Fill,
                Tag = new Action<Point>((point) => AddNewAndroid(point))
            };
        }

        private void btnIphoneApp_Click(object sender, RoutedEventArgs e)
        {
            ImageToDraw = new Image
            {
                Source = new BitmapImage(new Uri("Images/AppStore.png", UriKind.Relative)),
                Width = 50,
                Height = 50,
                Stretch = Stretch.Fill,
                Tag = new Action<Point>((point) => AddNewIphoneApp(point))
            };
        }

        private void AddNewAndroidApp(Point point)
        {
            var AndroidApp = new Rectangle
            {
                X = point,
                Y = new Point
                { X = point.X + 50, Y = point.Y + 50 }
            };
            foreach (var rect in AndroidAppRectangles)
            {
                if (rect.Value.Contains(AndroidApp))
                {
                    if (!AndroidApps.ContainsKey(rect.Key))
                    {
                        AndroidApps.Add(rect.Key, new List<Rectangle>());
                    }
                    AndroidApps[rect.Key].Add(AndroidApp);
                    mainCanvas.Children.Add(ImageToDraw);
                    return;
                }
            }
            nitherAppleApps.Add(AndroidApp);
            mainCanvas.Children.Add(ImageToDraw);
        }

        private void AddNewIphoneApp(Point point)
        {
            var IphoneApp = new Rectangle
            {
                X = point,
                Y = new Point
                { X = point.X + 50, Y = point.Y + 50 }
            };
            foreach (var rect in IphoneAppRectangles)
            {
                if (rect.Value.Contains(IphoneApp))
                {
                    if (!IPhoneApps.ContainsKey(rect.Key))
                    {
                        IPhoneApps.Add(rect.Key, new List<Rectangle>());
                    }
                    IPhoneApps[rect.Key].Add(IphoneApp);
                    mainCanvas.Children.Add(ImageToDraw);
                    return;
                }
            }
            nitherAppleApps.Add(IphoneApp);
            mainCanvas.Children.Add(ImageToDraw);
        }

        private void btnAndroidApp_Click(object sender, RoutedEventArgs e)
        {
            ImageToDraw = new Image
            {
                Source = new BitmapImage(new Uri("Images/google-play.png", UriKind.Relative)),
                Width = 50,
                Height = 50,
                Stretch = Stretch.Fill,
                Tag = new Action<Point>((point) => AddNewAndroidApp(point))
            };
        }

        private void btnIphoneLogo_Click(object sender, RoutedEventArgs e)
        {
            ImageToDraw = new Image
            {
                Source = new BitmapImage(new Uri("Images/apple-logo.png", UriKind.Relative)),
                Width = 50,
                Height = 50,
                Stretch = Stretch.Fill,
                Tag = new Action<Point>((point) => IPhoneLogoAdd(point))
            };
        }

        private void btnAndroidLogo_Click(object sender, RoutedEventArgs e)
        {
            ImageToDraw = new Image
            {
                Source = new BitmapImage(new Uri("Images/android-logo.png", UriKind.Relative)),
                Width = 50,
                Height = 50,
                Stretch = Stretch.Fill,
                Tag = new Action<Point>((point) => AndroidLogoAdd(point))
            };
        }

        private void AndroidLogoAdd(Point point)
        {
            var AndroidLogo = new Rectangle
            {
                X = point,
                Y = new Point
                { X = point.X + 50, Y = point.Y + 50 }
            };
            foreach (var phone in Androids)
            {
                if (phone.Contains(AndroidLogo) && 
                    !AndroidAppRectangles[Androids.IndexOf(phone)].Contains(AndroidLogo))
                {
                    this.AndroidLogo = AndroidLogo;
                    mainCanvas.Children.Add(ImageToDraw);
                    return;
                }
            }
        }

        private void IPhoneLogoAdd(Point point)
        {
            var IPhoneLogo = new Rectangle
            {
                X = point,
                Y = new Point
                { X = point.X + 50, Y = point.Y + 50 }
            };
            foreach (var phone in Apples)
            {
                if (phone.Contains(IPhoneLogo) &&
                    !IphoneAppRectangles[Apples.IndexOf(phone)].Contains(IPhoneLogo))
                {
                    this.AppleLogo = IPhoneLogo;
                    mainCanvas.Children.Add(ImageToDraw);
                    return;
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.Clear();
        }

        private void btnAnalyze_Click(object sender, RoutedEventArgs e)
        {
            int AndroidN;
            AndroidN = AndroidApps.Sum((value) => value.Value.Count);

            int IPhoneN;
            IPhoneN = IPhoneApps.Sum((value) => value.Value.Count);

            string IphoneLogo;
            if (AppleLogo == null)
            {
                IphoneLogo = "Нет";
            }
            else
            {
                IphoneLogo = "Да";
            }

            string SAndroidLogo;
            if (AndroidLogo == null)
            {
                SAndroidLogo = "Нет";
            }
            else
            {
                SAndroidLogo = "Да";
            }

            MessageBox.Show($"Обнаружено Айфонов: {Apples.Count}\n" +
                $"Обнаружено Андроидов: {Androids.Count}\n" +
                $"Обнаружено всего приложений на айфон: {IPhoneN}\n" +
                $"Обнаружено приложений на андроид: {AndroidN}\n" +
                $"Обнаружено неверных приложений: {nitherAndroidApps.Count + nitherAppleApps.Count}\n" +
                $"Обнаружено лого айфона: {IphoneLogo}\n" +
                $"Обнаружено лого андроид: {SAndroidLogo}");
        }

        private void mainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ImageToDraw != null && !mainCanvas.Children.Contains(ImageToDraw))
            {
                Point mousePoint = e.GetPosition(mainCanvas);
                Canvas.SetLeft(ImageToDraw, mousePoint.X);
                Canvas.SetTop(ImageToDraw, mousePoint.Y);              
                Action<Point> action = ImageToDraw.Tag as Action<Point>;
                action.Invoke(mousePoint);
            }
            else
            {
                MessageBox.Show("Please, select the Image");
            }
        }
    }
}
