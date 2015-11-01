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
        #region Variables
        private string SaveButtonMode;
        private string DiscSetSaveButtonMode;
        Businessobjects.GenearalHelper helper = new Businessobjects.GenearalHelper();
        Businessobjects.Discipline discipine = new Businessobjects.Discipline();

        string disciplineName;
        bool resIsDistance;
        float minimalResult = 0;
        float resultIncrement = 0;
        int minimalPoints = 0;
        int pointsIncrement = 0;
        float minimalResultF = 0;
        float resultIncrementF = 0;
        int minimalPointsF = 0;
        int pointsIncrementF = 0;

        string[] texts = new string[] { "Schlechtestes Ergebnis", "Ergebnisabstufung", "Minimalpunktzahl", "Punkteabstufung" };
        string oldDisciplineName;
        string oldDiscSetName;
        #endregion

        public Einstellungen()
        {
            InitializeComponent();
            SaveButtonMode = "insert";
            DiscSetSaveButtonMode = "insert";
            discipine.filllBDiscipline(lBDisciplines);
        }

        private void btnDiscSave_Click(object sender, RoutedEventArgs e)
        {
            #region SaveButtonMode = "insert"
            if (SaveButtonMode == "insert")
            {
                if (discipine.checkDisciplines(tBDisciplineName.Text))
                {
                    switch (checkTextBoxes())
                    {
                        case "male":
                            if (!PrepareVars(true))
                                return;

                            discipine.addDiscipline(tBDisciplineName.Text, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                            break;

                        case "female":
                            if (!PrepareVars(false))
                                return;

                            discipine.addDiscipline(tBDisciplineName.Text, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "female");
                            break;

                        case "both":
                            if (!PrepareVars(true) || !PrepareVars(false))
                                return;

                            discipine.addDiscipline(tBDisciplineName.Text, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "both");
                            break;

                        case "nothing":
                            return;
                    }
                }
                else
                {
                    MessageBox.Show("Eine Disziplin mit dem Namen '" + tBDisciplineName.Text + "' existiert bereits.\n Bitten Namen ändern.");
                    return;
                }

                cleanupNewDisc();
            }
            #endregion
            else
            #region SavebuttonMode = "update"
            {
                checkTextBoxes();

                switch(checkTextBoxes())
                {
                    case "male":
                        if (!PrepareVars(true))
                            return;

                        discipine.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                        break;

                    case "female":
                        if (!PrepareVars(false))
                            return;

                        discipine.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                        break;

                    case "both":
                        if (!PrepareVars(true) || !PrepareVars(false))
                            return;
                        discipine.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                        discipine.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "female");
                        break;
                }
                wPAddDisc.Visibility = Visibility.Hidden;
                cleanupNewDisc();

            }
            #endregion



            discipine.filllBDiscipline(lBDisciplines);
        }

        private void btnEditDisc_Click(object sender, RoutedEventArgs e)
        {
            SaveButtonMode = "update";
            wPAddDisc.Visibility = Visibility.Visible;

            if (lBDisciplines.SelectedItems.Count != 1)
                return;

            helper.prepareTextBoxes(wPAddDisc);

            string currentName = ((ListBoxItem)lBDisciplines.SelectedValue).Content.ToString();

            discipine.fillwPAddDisc(currentName.Trim(), tBDisciplineName, cBoxResultType, tBMinRes, tBResIncr, tBMinPts, tBPtsIncr, tBMinResF, tBResIncrF, tBMinPtsF, tBPtsIncrF);

            oldDisciplineName = tBDisciplineName.Text;

            doFoclostFunctionByEdit();
        }

        private void btnDeletDisc_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in lBDisciplines.Items)
            {
                if (item.IsSelected)
                {
                    discipine.deleteDiscipline(item.Content.ToString());
                }
            }
            discipine.filllBDiscipline(lBDisciplines);
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
            lBDisciplines.SelectedItems.Clear();
        }

        private void btnDelDiscsFromSet_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem[] items = new ListBoxItem[lBDiscSet.SelectedItems.Count];
            int count = 0;

            foreach (ListBoxItem item in lBDiscSet.SelectedItems)
            {
                items[count] = item;
                count++;
            }

            foreach (var v in items)
            {
                lBDiscSet.Items.Remove(v);
            }
        }

        private void btnDiscSetSave_Click(object sender, RoutedEventArgs e)
        {
            if (DiscSetSaveButtonMode == "insert")
            {
                if (tBDiscSetName.Foreground.ToString() == "#FF7E7E7E" || lBDiscSet.Items.Count == 0 || !discipine.checkDisciplineSets(tBDiscSetName.Text))
                    return;

                string[] disciplines = new string[lBDiscSet.Items.Count];

                int itemCount = lBDiscSet.Items.Count;

                for (int i = 0; i < itemCount; i++)
                {
                    ListBoxItem item = (ListBoxItem)lBDiscSet.Items[i];
                    disciplines[i] = item.Content.ToString();
                }

                discipine.addDiscSet(tBDiscSetName.Text, disciplines);
            }
            else
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

                discipine.updateDiscSet(oldDiscSetName, tBDiscSetName.Text, disciplines);
                discipine.filllBDiscSets(lBEditDiscSets);
            }

            #region cleanup
            lBDiscSet.Items.Clear();
            tBDiscSetName.Text = "Name";
            tBDiscSetName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
            #endregion
        }

        private void btnEditDiscSet_Click(object sender, RoutedEventArgs e)
        {
            if (lBEditDiscSets.SelectedItems.Count > 1)
            {
                MessageBox.Show("Bitte nur einen Disziplin-Satz zum bearbeiten auswählen");
                return;
            }

            if (lBEditDiscSets.SelectedItems.Count == 0)
                return;
                
            DiscSetSaveButtonMode = "update";

            string discSetName = ((ListBoxItem)lBEditDiscSets.SelectedValue).Content.ToString();
            tBDiscSetName.Foreground = Brushes.Black;

            discipine.filllBDiscSet(discSetName, tBDiscSetName, lBDiscSet);
            oldDiscSetName = discSetName;
            lBEditDiscSets.SelectedItems.Clear();
        }

        private void btnDeletDiscSet_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in lBEditDiscSets.Items)
            {
                if (item.IsSelected)
                {
                    discipine.deleteDiscSet(item.Content.ToString());
                }
            }
            discipine.filllBDiscSets(lBEditDiscSets);
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
             bool open = main.einstellungenIsOpen;
             open = false;
        }

        private void tVINewDisc_GotFocus(object sender, RoutedEventArgs e)
        {
            SaveButtonMode = "insert";
            wPAddDisc.Visibility = Visibility.Visible;

            cleanupNewDisc();

        }

        private void tVINewDiscSet_GotFocus(object sender, RoutedEventArgs e)
        {
            DiscSetSaveButtonMode = "insert";
            wPDiscSet.Visibility = Visibility.Visible;
        }

        private void tVIDelDiscSet_GotFocus(object sender, RoutedEventArgs e)
        {
            wPEditDiscSets.Visibility = Visibility.Visible;
            wPDiscSet.Visibility = Visibility.Visible;
            discipine.filllBDiscSets(lBEditDiscSets);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            wPAddDisc.Visibility = Visibility.Hidden;
            wPDiscSet.Visibility = Visibility.Hidden;
            wPEditDiscSets.Visibility = Visibility.Hidden;
            wPEditDiscSets.Visibility = Visibility.Hidden;
        }

        private string checkTextBoxes()
        {
            bool maleIsFull = true;
            bool femaleIsFull = true;

            foreach (TextBox tB in wPMale.Children.OfType<TextBox>())
            {
                if (tB.Foreground.ToString() == "#FF7E7E7E")
                    maleIsFull = false;
            }

            foreach (TextBox tB in wPFemale.Children.OfType<TextBox>())
            {
                if (tB.Foreground.ToString() == "#FF7E7E7E")
                    femaleIsFull = false;
            }

            if (maleIsFull && !femaleIsFull)
                return "male";
            if (!maleIsFull && femaleIsFull)
                return "female";
            if (maleIsFull && femaleIsFull)
                return "both";
            else
                return "nothing";

        }

        private void doFoclostFunctionByEdit()
        {
            int count = 0;

            foreach(TextBox tB in wPMale.Children.OfType<TextBox>())
            {
                foclost(tB, texts[count]);
                count++;
            }

            count = 0;
            foreach (TextBox tB in wPFemale.Children.OfType<TextBox>())
            {
                foclost(tB, texts[count]);
                count++;
            }
        }

        private bool PrepareVars(bool male)
        {
            if (cBoxResultType.SelectedIndex == 0)
                resIsDistance = true;
            else
                resIsDistance = false;

            disciplineName = tBDisciplineName.Text;

            if (male)
            {
                tBMinRes.Text = tBMinRes.Text.Replace(".", ",");
                tBResIncr.Text = tBResIncr.Text.Replace(".", ",");
                try
                {
                    minimalResult = (float)Convert.ToDouble(tBMinRes.Text);
                    resultIncrement = (float)Convert.ToDouble(tBResIncr.Text);
                    minimalPoints = Convert.ToInt32(tBMinPts.Text);
                    pointsIncrement = Convert.ToInt32(tBPtsIncr.Text);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                tBMinResF.Text = tBMinResF.Text.Replace(".", ",");
                tBResIncrF.Text = tBResIncrF.Text.Replace(".", ",");
                try
                {
                    minimalResultF = (float)Convert.ToDouble(tBMinResF.Text);
                    resultIncrementF = (float)Convert.ToDouble(tBResIncrF.Text);
                    minimalPointsF = Convert.ToInt32(tBMinPtsF.Text);
                    pointsIncrementF = Convert.ToInt32(tBPtsIncrF.Text);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;

        }

        private void cleanupNewDisc()
        {
            int count = 0;

            tBDisciplineName.Text = "Disziplin";
            tBDisciplineName.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
            cBoxResultType.SelectedIndex = 0;

            foreach (TextBox tB in wPMale.Children.OfType<TextBox>())
            {
                tB.Text = texts[count];
                tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                count++;
            }

            count = 0;

            foreach (TextBox tB in wPFemale.Children.OfType<TextBox>())
            {
                tB.Text = texts[count];
                tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                count++;
            }
        }

        #region explainingtext appereance and disappereance
        //gotfoc + foclost handle the appereance and disappereance of the grey explaining text in Textboxes
        private void gotfoc(TextBox tB)
        {
            //OMFG, that fucking works!!
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
            gotfoc(tBDisciplineName);
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