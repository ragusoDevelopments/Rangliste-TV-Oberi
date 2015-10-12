﻿using System;
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
            this.Topmost = true; //testpurpose
        }

        //Adds a new discipline to the database
        private void btnDiscSave_Click(object sender, RoutedEventArgs e)
        {
            

            addResultsToDiscipline(tBDisciplineName, tBMinRes, tBResIncr, tBPtsIncr, cBoxResultType, tBMinPts, true);
            addResultsToDiscipline(tBDisciplineName, tBMinResF, tBResIncrF, tBPtsIncrF, cBoxResultTypeF, tBMinPtsF, false);

            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.listTable();

        }


        private void filllBDiscipline()
        {
            lBDisciplines.Items.Clear();

            IEnumerable<RL_Datacontext.Disciplines> discs = Businessobjects.SQLFunctions.returnDisciplines();


            foreach (var v in discs)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = v.Discipline;

                lBDisciplines.Items.Add(newItem);
            }
        }

        private void addResultsToDiscipline(TextBox tBDiscName, TextBox tBMinResult, TextBox tBResIncrement, TextBox tBPtsIncrement, ComboBox cBResType, TextBox tBMinPoints, bool male)
        {
            if (tBDiscName.Foreground.ToString() == "#FF7E7E7E" || tBMinResult.Foreground.ToString() == "#FF7E7E7E" || tBResIncrement.Foreground.ToString() == "#FF7E7E7E" || tBPtsIncrement.Foreground.ToString() == "#FF7E7E7E")
            {
                cBoxResultType.SelectedIndex = 0;
                cBoxResultTypeF.SelectedIndex = 0;
                return;
            }

            float minRes = 0;
            float resIncr = 0;
            int minPts = 0;
            int ptsIncr = 0;
            bool resIsDistance;

            if (cBResType.SelectedIndex == 0)
                resIsDistance = true;
            else
                resIsDistance = false;

            tBMinResult.Text = tBMinResult.Text.Replace(".", ",");
            tBResIncrement.Text = tBResIncrement.Text.Replace(".", ",");

            try
            {
                minRes = (float)Convert.ToDouble(tBMinResult.Text);
                resIncr = (float)Convert.ToDouble(tBResIncrement.Text);
                minPts = Convert.ToInt32(tBMinPoints.Text);
                ptsIncr = Convert.ToInt32(tBPtsIncrement.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());//testpurpose
                return;
            }

            RL_Datacontext.Disciplines disc = Businessobjects.SQLFunctions.addDiscipline(tBDiscName.Text);
            Businessobjects.SQLFunctions.addPointsToDiscipline(disc, minRes, resIncr, minPts, ptsIncr, resIsDistance, male);

            filllBDiscipline();

            tBDiscName.Text = "";
            foclost(tBDiscName, "Name");
            tBMinResult.Text = "";
            foclost(tBMinResult, "Mindestleistung");
            tBResIncrement.Text = "";
            foclost(tBResIncrement, "Ergebnisabstufung");
            tBMinPoints.Text = "";
            foclost(tBMinPoints, "Minimalpunktzahl");
            tBPtsIncrement.Text = "";
            foclost(tBPtsIncrement, "Punkteabstufung");
            cBResType.SelectedIndex = 0;
        }

        private void btnAddDiscsToSet_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in lBDisciplines.Items)
            {
                if (item.IsSelected)
                {
                    ListBoxItem newItem = new ListBoxItem();
                    newItem.Content = item.Content;
                    lBDiscSet.Items.Add(newItem);
                }
            }
        }





        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.einstellungenIsOpen = false;
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

        private void tBMinResF_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBMinResF);
        }

        private void tBMinResF_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBMinResF, "Schlechtestes Ergebnis");
        }

        private void tBResIncrF_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBResIncrF);
        }

        private void tBResIncrF_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBResIncrF, "Ergebnisabstufung");
        }

        private void tBMinPtsF_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBMinPtsF);

        }

        private void tBMinPtsF_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBMinPtsF, "Minimalpunktzahl");
        }

        private void tBPtsIncrF_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBPtsIncrF);
        }

        private void tBPtsIncrF_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBPtsIncrF, "Punkteabstufung");
        }

        private void tBDiscSetName_GotFocus(object sender, RoutedEventArgs e)
        {
            gotfoc(tBDiscSetName);
        }

        private void tBDiscSetName_LostFocus(object sender, RoutedEventArgs e)
        {
            foclost(tBDiscSetName, "Name");
        }
        #endregion

        private void TreeViewItem_GotFocus(object sender, RoutedEventArgs e)
        {
            wPMale.Visibility = Visibility.Visible;
            wPFemale.Visibility = Visibility.Visible;
        }

        private void tVINewDiscSet_GotFocus(object sender, RoutedEventArgs e)
        {
            wPDiscSet.Visibility = Visibility.Visible;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            wPMale.Visibility = Visibility.Hidden;
            wPFemale.Visibility = Visibility.Hidden;
            wPDiscSet.Visibility = Visibility.Hidden;
        }

        private void btnDiscSetSave_Click(object sender, RoutedEventArgs e)
        {
            if (tBDiscSetName.Foreground.ToString() == "#FF7E7E7E" || lBDiscSet.Items.Count == 0)
                return;

            string[] disciplines = null;
            int count = 0;

            foreach(ListBoxItem item in lBDiscSet.Items)
            {
                disciplines[count] = item.Content.ToString();
                count++;
            }

            Businessobjects.SQLFunctions.addDiscSet(tBDiscSetName.Text, disciplines);

        }

        

       












    }
}
