using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Rangliste_TV_Oberi.Businessobjects
{
    class GenearalHelper
    {
        public void addDisciplineAndResultsToDiscipline(TextBox tBDiscName, TextBox tBMinResult, TextBox tBResIncrement, TextBox tBPtsIncrement, ComboBox cBResType, TextBox tBMinPoints, bool IsMale, WrapPanel wP)
        {
            if (tBDiscName.Foreground.ToString() == "#FF7E7E7E" || tBMinResult.Foreground.ToString() == "#FF7E7E7E" || tBResIncrement.Foreground.ToString() == "#FF7E7E7E" || tBPtsIncrement.Foreground.ToString() == "#FF7E7E7E")
                return;

            tBMinResult.Text = tBMinResult.Text.Replace(".", ",");
            tBResIncrement.Text = tBResIncrement.Text.Replace(".", ",");

            bool resIsDistance;

            if (cBResType.SelectedIndex == 0)
                resIsDistance = true;
            else
                resIsDistance = false;

            float minRes = checkAndConvertVar(tBMinResult.Text);
            float resIncr = checkAndConvertVar(tBResIncrement.Text);
            int minPts = (int)checkAndConvertVar(tBMinPoints.Text);
            int ptsIncr = (int)checkAndConvertVar(tBPtsIncrement.Text);

            if (minRes == -1 || resIncr == -1 || minPts == -1 || ptsIncr == -1)
                return;

            RL_Datacontext.Disciplines disc = null;

            if (Businessobjects.SQLAddAndReturnFunctions.checkDisciplines(tBDiscName.Text))
            {
                disc = Businessobjects.SQLAddAndReturnFunctions.addDiscipline(tBDiscName.Text);
                Businessobjects.SQLUpdateFuntions.handlePointsToDiscipline(disc, minRes, resIncr, minPts, ptsIncr, resIsDistance, IsMale, null);
            }
            else
            {
                MessageBox.Show("Es existiert bereits eine Kategorie mit dem Namen '" + tBDiscName.Text + "'.\nBitte Namen ändern.", "Kategorie besteht bereits");
                return;
            }

            #region cleanup
            string[] texts = new string[] { "Disziplin", "Mindestleistung", "Ergebnisabstufung", "Minimalpunktzahl", "Punkteabstufung" };
            int count = 0;

            foreach (TextBox tB in wP.Children.OfType<TextBox>())
            {
                tB.Text = texts[count];
                tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                count++;
            }
            cBResType.SelectedIndex = 0;
            #endregion
        }

        public void filllBDiscipline(ListBox lBDisciplines)
        {
            RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();
            lBDisciplines.Items.Clear();

            IEnumerable<RL_Datacontext.Disciplines> discs = Businessobjects.SQLAddAndReturnFunctions.returnDisciplines(null);

            foreach (var v in discs)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = v.Discipline;

                lBDisciplines.Items.Add(newItem);
            }
        }

        public float checkAndConvertVar(string var)
        {
            float returnVar = 0;
            try
            {
                returnVar = (float)Convert.ToDouble(var);
            }
            catch (Exception ex)
            {
                return -1;
            }
            return returnVar;
        }

        public void EditDiscipline(TextBox tBDiscName, TextBox tBMinResult, TextBox tBResIncrement, TextBox tBPtsIncrement, ComboBox cBResType, TextBox tBMinPoints, bool IsMale, WrapPanel wP, string oldDiscName)
        {
            if (tBDiscName.Foreground.ToString() == "#FF7E7E7E" || tBMinResult.Foreground.ToString() == "#FF7E7E7E" || tBResIncrement.Foreground.ToString() == "#FF7E7E7E" || tBPtsIncrement.Foreground.ToString() == "#FF7E7E7E")
                return;

            tBMinResult.Text = tBMinResult.Text.Replace(".", ",");
            tBResIncrement.Text = tBResIncrement.Text.Replace(".", ",");

            bool resIsDistance;

            if (cBResType.SelectedIndex == 0)
                resIsDistance = true;
            else
                resIsDistance = false;

            float minRes = checkAndConvertVar(tBMinResult.Text);
            float resIncr = checkAndConvertVar(tBResIncrement.Text);
            int minPts = (int)checkAndConvertVar(tBMinPoints.Text);
            int ptsIncr = (int)checkAndConvertVar(tBPtsIncrement.Text);

            if (minRes == -1 || resIncr == -1 || minPts == -1 || ptsIncr == -1)
                return;

            RL_Datacontext.Disciplines disc = Businessobjects.SQLUpdateFuntions.updateDiscipline(tBDiscName.Text, oldDiscName);
            Businessobjects.SQLUpdateFuntions.handlePointsToDiscipline(disc, minRes, resIncr, minPts, ptsIncr, resIsDistance, true, "update");
            

            #region cleanup
            string[] texts = new string[] { "Disziplin", "Mindestleistung", "Ergebnisabstufung", "Minimalpunktzahl", "Punkteabstufung" };
            int count = 0;

            foreach (TextBox tB in wP.Children.OfType<TextBox>())
            {
                tB.Text = texts[count];
                tB.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF7E7E7E");
                count++;
            }
            cBResType.SelectedIndex = 0;
            #endregion
        }
    }
}
