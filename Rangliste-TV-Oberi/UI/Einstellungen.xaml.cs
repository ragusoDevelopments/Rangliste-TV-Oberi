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
using System.Windows.Shapes;

namespace Rangliste_TV_Oberi
{
    /// <summary>
    /// Interaktionslogik für Einstellungen.xaml
    /// </summary>
    public partial class Einstellungen : Window
    {
        
        public Einstellungen()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.einstellungenIsOpen = false;
        }









        #region explainingtexts appereance and disappereance
        //gotfoc + foclost handle the appereance and disappereance of the grey explaining text in Textboxes
        private void gotfoc(TextBox tB)
        {
            if (tB.Foreground.ToString() == "#FF7E7E7E")
            {
                tB.Text = "";
                tB.Foreground = Brushes.Black;
            }
        }
        private void foclost(TextBox tB, string Text)
        {
            if (tB.Text == "")
            {
                tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                tB.Text = Text;
            }
        }



        private void tBDisciplineName_GotFocus(object sender, RoutedEventArgs e)
        {
            //OMFG, That fucking works!!!
            if(tBDisciplineName.Foreground.ToString() == "#FF7E7E7E")
            {
                gotfoc(tBDisciplineName);
            }
            
        }

        private void tBDisciplineName_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBDisciplineName, "Disziplin");
        }

        private void tBMax_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBMax);
        }

        private void tBMax_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBMax, "Höchstleistung");
        }

        private void tBMin_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBMin);
        }

        private void tBMin_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBMin, "Mindestleistung");
        }

        #endregion

    }
}
