// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="no">
//   no
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace K_Means
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// The class radius.
        /// </summary>
        private const int ClassRadius = 10;

        /// <summary>
        /// The object radius.
        /// </summary>
        private const int ObjectRadius = 1;

        /// <summary>
        /// The myCore.
        /// </summary>
        private Core myCore = new Core();

        /// <summary>
        /// The _is valid classes.
        /// </summary>
        private bool isValidClasses;

        /// <summary>
        /// The _is valid objects.
        /// </summary>
        private bool isValidObjects;

        /// <summary>
        /// The colors.
        /// </summary>
        private List<Brush> _brushes = new List<Brush>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.LockButtons();
        }

        /// <summary>
        /// The tbx classes text changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TbxClassesTextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Foreground = Brushes.Black;
            string input = (sender as TextBox)?.Text;
            if (!Regex.IsMatch(input ?? throw new NullReferenceException(), @"^\d{1,2}$"))
            {
                ((TextBox)sender).Foreground = Brushes.Red;
                this.LockButtons();
                return;
            }

            this.isValidClasses = true;
            this.UnlockButtons();
        }

        /// <summary>
        /// The tbx objects text changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TbxObjectsTextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)sender).Foreground = Brushes.Black;
            var input = ((TextBox)sender)?.Text;
            if (!Regex.IsMatch(input ?? throw new NullReferenceException(), @"^\d{1,6}$"))
            {
                ((TextBox)sender).Foreground = Brushes.Red;
                this.LockButtons();
                return;
            }

            this.isValidObjects = true;
            this.UnlockButtons();
        }

        /// <summary>
        /// The lock buttons.
        /// </summary>
        private void LockButtons()
        {
            if (this.BtnCalculate != null && this.BtnGenerate != null)
            {
                this.BtnCalculate.IsEnabled = false;
                this.BtnGenerate.IsEnabled = false;
            }

            if (this.BtnMaxMin != null && !this.isValidObjects)
            {
                this.BtnMaxMin.IsEnabled = false;
            }
        }

        /// <summary>
        /// The unlock buttons.
        /// </summary>
        private void UnlockButtons()
        {
            if (this.isValidClasses && this.isValidObjects)
            {
                this.BtnCalculate.IsEnabled = true;
                this.BtnGenerate.IsEnabled = true;
            }

            if (this.isValidObjects)
            {
                this.BtnMaxMin.IsEnabled = true;
            }
        }

        /// <summary>
        /// The btn generate click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnGenerateClick(object sender, RoutedEventArgs e)
        {
            var classesCount = Convert.ToInt32(this.TbxClasses.Text);
            var objectsCount = Convert.ToInt32(this.TbxObjects.Text);

            if (classesCount > objectsCount)
            {
                MessageBox.Show("Classes is more than objects", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.myCore.DoGenerate(
                classesCount,
                objectsCount,
                (int)this.GrdHelper.ActualHeight,
                (int)this.GrdHelper.ActualWidth);
            this.FillRandomColor(this._brushes, classesCount);
            this.DrawAll(this.myCore);
        }

        /// <summary>
        /// The fill random color.
        /// </summary>
        /// <param name="colors">
        /// The colors.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        private void FillRandomColor(List<Brush> colors, int count)
        {
            var rand = new Random();
            for (int i = 0; i < count; i++)
            {
                colors.Add(
                    new SolidColorBrush(
                        Color.FromRgb((byte)rand.Next(0, 256), (byte)rand.Next(0, 256), (byte)rand.Next(0, 256))));
            }
        }

        /// <summary>
        /// The btn calculate_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private async void BtnCalculateClick(object sender, RoutedEventArgs e)
        {
            this.GrdControls.IsEnabled = false;
            this.myCore.DoMeansValidate();
            do
            {
                this.DrawAll(this.myCore);
                await Task.Delay(1000);
            }
            while (this.myCore.DoKMeanIteration(this.myCore.Classes, this.myCore.Points));
            this.GrdControls.IsEnabled = true;
        }

        /// <summary>
        /// The draw all.
        /// </summary>
        /// <param name="core">
        /// The core.
        /// </param>
        private void DrawAll(Core core)
        {
            DrawingGroup drawingGroup = new DrawingGroup();
            int id = 0;
            foreach (var meansClass in core.Classes)
            {
                this.DrawClass(meansClass, drawingGroup, id);
                id++;
            }

            this.ImgMain.Source = new DrawingImage(drawingGroup);
        }

        /// <summary>
        /// The draw class.
        /// </summary>
        /// <param name="meansClass">
        /// The k class.
        /// </param>
        /// <param name="drawingGroup">
        /// The drawing group.
        /// </param>
        /// <param name="classId">
        /// The class Id.
        /// </param>
        private void DrawClass(KClass meansClass, DrawingGroup drawingGroup, int classId)
        {
            GeometryGroup geometryEllipsesGroup = new GeometryGroup();
            foreach (var point in meansClass.Points)
            {
                geometryEllipsesGroup.Children.Add(point == meansClass.Center
                    ? new EllipseGeometry(point, ClassRadius, ClassRadius)
                    : new EllipseGeometry(point, ObjectRadius, ObjectRadius));
            }    
            
            Brush brush = this._brushes[classId];
            GeometryDrawing geometryDrawing = new GeometryDrawing(brush, new Pen(brush, 1), geometryEllipsesGroup);
            drawingGroup.Children.Add(geometryDrawing);
        }

        /// <summary>
        /// The btn max min click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private async void BtnMaxMinClick(object sender, RoutedEventArgs e)
        {
            this.GrdControls.IsEnabled = false;
            int objCount = Convert.ToInt32(this.TbxObjects.Text);
            this.myCore.MaxMinInit(objCount, (int)this.GrdHelper.ActualHeight, (int)this.GrdHelper.ActualWidth);
            this.FillRandomColor(this._brushes, 100);
            do
            {
                this.DrawAll(this.myCore);
                await Task.Delay(1000);
            }
            while (this.myCore.DoMaxMinIteration(this.myCore.Classes,this.myCore.Points));

            this.GrdControls.IsEnabled = true;
        }
    }
}
