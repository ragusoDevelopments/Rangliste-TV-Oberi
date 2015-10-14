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
        Businessobjects.GenearalHelper helper = new Businessobjects.GenearalHelper();

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
        #endregion

        public Einstellungen()
        {
            InitializeComponent();
            SaveButtonMode = "insert";
            helper.filllBDiscipline(lBDisciplines);
            this.Topmost = true; //testpurpose
        }

        private void btnDiscSave_Click(object sender, RoutedEventArgs e)
        {
            #region SaveButtonMode = "insert"
            if (SaveButtonMode == "insert")
            {
                if (Businessobjects.SQLAddAndReturnFunctions.checkDisciplines(tBDisciplineName.Text))
                {
                    switch (checkTextBoxes())
                    {
                        case "male":
                            if (!PrepareVars(true))
                                return;

                            Businessobjects.SQLAddAndReturnFunctions.addDiscipline(tBDisciplineName.Text, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                            break;

                        case "female":
                            if (!PrepareVars(false))
                                return;

                            Businessobjects.SQLAddAndReturnFunctions.addDiscipline(tBDisciplineName.Text, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "female");
                            break;

                        case "both":
                            if (!PrepareVars(true) || !PrepareVars(false))
                                return;

                            Businessobjects.SQLAddAndReturnFunctions.addDiscipline(tBDisciplineName.Text, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "both");
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

                        Businessobjects.SQLUpdateFuntions.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                        break;

                    case "female":
                        if (!PrepareVars(false))
                            return;

                        Businessobjects.SQLUpdateFuntions.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                        break;

                    case "both":
                        if (!PrepareVars(true) || !PrepareVars(false))
                            return;
                        Businessobjects.SQLUpdateFuntions.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "male");
                        Businessobjects.SQLUpdateFuntions.updateDiscipline(oldDisciplineName, disciplineName, resIsDistance, minimalResult, resultIncrement, minimalPoints, pointsIncrement, minimalResultF, resultIncrementF, minimalPointsF, pointsIncrementF, "female");
                        break;
                }
                cleanupNewDisc();

            }
            #endregion



            helper.filllBDiscipline(lBDisciplines);
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.listTable();
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
            if (tBDiscSetName.Foreground.ToString() == "#FF7E7E7E" || lBDiscSet.Items.Count == 0 || !Businessobjects.SQLAddAndReturnFunctions.checkDisciplineSets(tBDiscSetName.Text))
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
            wPAddDisc.Visibility = Visibility.Visible;

            if (lBDisciplines.SelectedItems.Count != 1)
                return;
            helper.prepareTextBoxes(wPAddDisc);


            char[] split = new char[] { Convert.ToChar(":") };
            string currentName = lBDisciplines.SelectedItem.ToString().Split(split)[1];

            Businessobjects.SQLUpdateFuntions.fillwPAddDisc(currentName.Trim(), tBDisciplineName, cBoxResultType, tBMinRes, tBResIncr, tBMinPts, tBPtsIncr, tBMinResF, tBResIncrF, tBMinPtsF, tBPtsIncrF);

            oldDisciplineName = tBDisciplineName.Text;

            doFoclostFunctionByEdit();
        }

        private void btnDeletDiscSet_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem item in lBEditDiscSets.Items)
            {
                if (item.IsSelected)
                {
                    Businessobjects.SQLDeleteFunctions.deleteDiscSet(item.Content.ToString());
                }
            }
            Businessobjects.SQLDeleteFunctions.filllBDiscSets(lBEditDiscSets);
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.einstellungenIsOpen = false;
        }

        private void tVINewDisc_GotFocus(object sender, RoutedEventArgs e)
        {
            SaveButtonMode = "insert";
            wPAddDisc.Visibility = Visibility.Visible;

            cleanupNewDisc();

        }

        private void tVINewDiscSet_GotFocus(object sender, RoutedEventArgs e)
        {
            wPDiscSet.Visibility = Visibility.Visible;
        }

        private void tVIDelDiscSet_GotFocus(object sender, RoutedEventArgs e)
        {
            wPEditDiscSets.Visibility = Visibility.Visible;
            Businessobjects.SQLDeleteFunctions.filllBDiscSets(lBEditDiscSets);
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            wPAddDisc.Visibility = Visibility.Hidden;
            wPDiscSet.Visibility = Visibility.Hidden;
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
