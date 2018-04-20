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

namespace GrammarGenerator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Grammar grammar = new Grammar();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void tbxWordsCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DisallowNotNumericEnter(e);
        }

        private void tbxMinDeepLevel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DisallowNotNumericEnter(e);
        }

        private void tbxMaxDeepLevel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DisallowNotNumericEnter(e);
        }

        private void DisallowNotNumericEnter(TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void tbxChain_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsLetter(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void btnAddChain_Click(object sender, RoutedEventArgs e)
        {
            string chain = tbxChain.Text;
            if (!string.IsNullOrWhiteSpace(chain))
            {
                if (!lvChains.Items.Contains(chain))
                {
                    lvChains.Items.Add(chain);
                }               
            }
        }

        private void btnGenerateGrammar_Click(object sender, RoutedEventArgs e)
        {
            List<string> chains = lvChains.Items.Cast<string>().ToList();

            grammar = Generator.GenerateGrammar(chains);

            tblkResultGrammar.Text = grammar.ToString();
        }

        private void btnGenerateChains_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbxWordsCount.Text)) 
            {
                tblkResultChains.Text = Generator.GenerateChains(grammar, 
                    Convert.ToInt32(tbxWordsCount.Text));
            }    
        }

        private void btnDeleteChain_Click(object sender, RoutedEventArgs e)
        {
            lvChains.Items.Remove(lvChains.SelectedItem);
        }
    }
}
