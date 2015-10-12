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
        private string SaveButtonMode;
        Businessobjects.GenearalHelper helper = new Businessobjects.GenearalHelper();

        public Einstellungen()
        {
            InitializeComponent();
            SaveButtonMode = "insert";
            helper.filllBDiscipline(lBDisciplines);
            this.Topmost = true; //testpurpose
        }

        private void btnDiscSave_Click(object sender, RoutedEventArgs e)
        {
            if (SaveButtonMode == "insert")
            {
                helper.addDisciplineAndResultsToDiscipline(tBDisciplineName, tBMinRes, tBResIncr, tBPtsIncr, cBoxResultType, tBMinPts, true, wPMale);
                helper.addDisciplineAndResultsToDiscipline(tBDisciplineName, tBMinResF, tBResIncrF, tBPtsIncrF, cBoxResultTypeF, tBMinPtsF, false, wPFemale);
            }

            if (SaveButtonMode == "update")
            {
                #region cleanup
                wPFemale.Visibility = Visibility.Hidden;
                wPMale.Visibility = Visibility.Hidden;
                #endregion
            }

            helper.filllBDiscipline(lBDisciplines);

        }

        private void btnDeletDisc_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in lBDisciplines.Items)
            {
                if (item.IsSelected)
                {
                    Businessobjects.SQLDeleteFunctions.deleteDiscipline(item.Content.ToString());
                }
            }
            helper.filllBDiscipline(lBDisciplines);
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

        private void btnDiscSetSave_Click(object sender, RoutedEventArgs e)
        {
            if (tBDiscSetName.Foreground.ToString() == "#FF7E7E7E" || lBDiscSet.Items.Count == 0)
                return;

            string[] disciplines = new string[lBDiscSet.Items.Count];

            int itemCount = lBDiscSet.Items.Count;

            for (int i = 0; i < itemCount; i++)
            {
                ListBoxItem item = (ListBoxItem)lBDiscSet.Items[i];
                disciplines[i] = item.Content.ToString();
            }

            Businessobjects.SQLAddAndReturnFunctions.addDiscSet(tBDiscSetName.Text, disciplines);

            #region cleanup
            lBDiscSet.Items.Clear();
            tBDiscSetName.Text = "Name";
            tBDiscSetName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
            #endregion
        }

        private void btnEditDisc_Click(object sender, RoutedEventArgs e)
        {
            SaveButtonMode = "update";
            wPFemale.Visibility = Visibility.Visible;
            wPMale.Visibility = Visibility.Visible;
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.einstellungenIsOpen = false;
        }

        private void tVINewDisc_GotFocus(object sender, RoutedEventArgs e)
        {
            wPMale.Visibility = Visibility.Visible;
            wPFemale.Visibility = Visibility.Visible;
            SaveButtonMode = "insert";

                #region cleanup
                string[] texts = new string[] { "Disziplin", "Mindestleistung", "Ergebnisabstufung", "Minimalpunktzahl", "Punkteabstufung" };
                int count = 0;

                foreach (TextBox tB in wPMale.Children.OfType<TextBox>())
                {
                    tB.Text = texts[count];
                    tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                    count++;
                }
                cBoxResultType.SelectedIndex = 0;

                count = 0;
                foreach (TextBox tB in wPFemale.Children.OfType<TextBox>())
                {
                    tB.Text = texts[count];
                    tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                    count++;
                }
                cBoxResultTypeF.SelectedIndex = 0;
                #endregion
            
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
            SaveButtonMode = "insert";
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





















    }
}
