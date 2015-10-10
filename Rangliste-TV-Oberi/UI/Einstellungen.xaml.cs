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

        //Adds a new discipline to the database
        private void btnDiscSave_Click(object sender, RoutedEventArgs e)
        {
            if (tBDisciplineName.Text == "" || tBMinRes.Text == "" || tBResIncr.Text == "" || tBPtsIncr.Text == "")
                return;

            float minRes = 0;
            float resIncr = 0;
            int minPts = 0;
            int ptsIncr = 0;
            bool resIsDistance;

            if (cBoxResultType.SelectedIndex == 0)
                resIsDistance = true;
            else
                resIsDistance = false;
            tBMinRes.Text = tBMinRes.Text.Replace(".", ",");
            tBResIncr.Text = tBResIncr.Text.Replace(".", ",");
            try
            {
                minRes = (float) Convert.ToDouble(tBMinRes.Text);
                resIncr = (float)Convert.ToDouble(tBResIncr.Text);
                minPts = Convert.ToInt32(tBMinPts.Text);
                ptsIncr = Convert.ToInt32(tBPtsIncr.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }




            RL_Datacontext.Disciplines disc = Businessobjects.SQLFunctions.addDiscipline(tBDisciplineName.Text);
            Businessobjects.SQLFunctions.addPointsToDiscipline(disc, minRes, resIncr, minPts, ptsIncr, resIsDistance);

            tBDisciplineName.Text = "";
            foclost(tBDisciplineName, "Name");
            tBMinRes.Text = "";
            foclost(tBMinRes, "Mindestleistung");
            tBResIncr.Text = "";
            foclost(tBResIncr, "Ergebnisabstufung");
            tBMinPts.Text = "";
            foclost(tBMinPts, "Minimalpunktzahl");
            tBPtsIncr.Text = "";
            foclost(tBPtsIncr, "Punkteabstufung");
            cBoxResultType.SelectedIndex = 0;
        }
        










        #region explainingtext appereance and disappereance
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
            if (tBDisciplineName.Foreground.ToString() == "#FF7E7E7E")
            {
                gotfoc(tBDisciplineName);
            }

        }

        private void tBDisciplineName_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBDisciplineName, "Disziplin");
        }

        private void tBMinPts_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBMinPts);
        }

        private void tBMinPts_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBMinPts, "Minimalpunktzahl");
        }

        private void tBMinRes_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBMinRes);
        }

        private void tBMinRes_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBMinRes, "Schlechtestes Ergebnis");
        }

        private void tBResIncr_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBResIncr);
        }

        private void tBResIncr_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBResIncr, "Ergebnisabstufung");
        }

        private void tBPtsIncr_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBPtsIncr);
        }

        private void tBPtsIncr_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBPtsIncr, "Punkteabstufung");
        }

        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.einstellungenIsOpen = false;
        }







    }
}
