using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Rangliste_TV_Oberi.Businessobjects
{
    static class SQLUpdateFuntions
    {
        private static RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

        public static void updateDiscipline(string oldDicsName, string newDiscName, bool resIsDistance, float minRes, float resIncr, int minPts, int ptsIncr, float minResF, float resIncrF, int minPtsF, int ptsIncrF, string gender)
        {
            RL_Datacontext.Disciplines disc = (from d in dc.Disciplines
                                               where d.DisciplineName == oldDicsName
                                               select d).First();

            if (disc == null)
            {
                MessageBox.Show("Die Disziplin wurde während dem Bearbeiten gelöscht");
                return;
            }

            disc.DisciplineName = newDiscName;
            disc.IsDistance = resIsDistance;

            switch (gender)
            {
                case "male":
                    disc.MinResult =  Math.Round(minRes, 1);
                    disc.ResultIncr = Math.Round(resIncr, 1);
                    disc.MinPoints = minPts;
                    disc.PointsIncr = ptsIncr;

                    disc.MaleDisciplinePoints.Clear();
                    Businessobjects.SQLAddAndReturnFunctions.addPointsAndResultsMale(disc, resIsDistance, minRes, resIncr, minPts, ptsIncr);
                    break;

                case "female":
                    disc.MinResultF =  Math.Round(minResF, 1);
                    disc.ResultIncrF = Math.Round(resIncrF, 1);
                    disc.MinPointsF = minPtsF;
                    disc.PointsIncrF = ptsIncrF;

                    disc.FemaleDisciplinePoints.Clear();
                    Businessobjects.SQLAddAndReturnFunctions.addPointsAndResultsFemale(disc, resIsDistance, minResF, resIncrF, minPtsF, ptsIncrF);
                    break;

                case "both":
                    disc.MinResult =  Math.Round(minRes, 1);
                    disc.ResultIncr = Math.Round(resIncr, 1);
                    disc.MinPoints = minPts;
                    disc.PointsIncr = ptsIncr;

                    disc.MinResultF =  Math.Round(minResF, 1);
                    disc.ResultIncrF = Math.Round(resIncrF, 1);
                    disc.MinPointsF = minPtsF;
                    disc.PointsIncrF = ptsIncrF;

                    disc.MaleDisciplinePoints.Clear();
                    disc.FemaleDisciplinePoints.Clear();
                    Businessobjects.SQLAddAndReturnFunctions.addPointsAndResultsMale(disc, resIsDistance, minRes, resIncr, minPts, ptsIncr);
                    Businessobjects.SQLAddAndReturnFunctions.addPointsAndResultsFemale(disc, resIsDistance, minResF, resIncrF, minPtsF, ptsIncrF);
                    break;
            }

            disc.DisciplineName = newDiscName;
            dc.SubmitChanges();


        }

        public static void fillwPAddDisc(string currentName, TextBox name, ComboBox resType, TextBox minres, TextBox resIncr, TextBox minPts, TextBox ptsIncr, TextBox minresF, TextBox resIncrF, TextBox minPtsF, TextBox ptsIncrF)
        {
            RL_Datacontext.Disciplines disc = (from d in dc.Disciplines
                                               where d.DisciplineName == currentName
                                               select d).First();
            if (disc.IsDistance)
                resType.SelectedIndex = 0;
            else
                resType.SelectedIndex = 1;

            name.Text = currentName;
            minres.Text = disc.MinResult.ToString();
            resIncr.Text = disc.ResultIncr.ToString();
            minPts.Text = disc.MinPoints.ToString();
            ptsIncr.Text = disc.PointsIncr.ToString();

            minresF.Text = disc.MinResultF.ToString();
            resIncrF.Text = disc.ResultIncrF.ToString();
            minPtsF.Text = disc.MinPointsF.ToString();
            ptsIncrF.Text = disc.PointsIncrF.ToString();
        }
    }
}
