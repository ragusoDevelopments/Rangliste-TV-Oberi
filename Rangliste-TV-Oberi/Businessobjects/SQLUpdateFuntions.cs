﻿using System;
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
                    disc.MinResult = Math.Round(minRes, 1);
                    disc.ResultIncr = Math.Round(resIncr, 1);
                    disc.MinPoints = minPts;
                    disc.PointsIncr = ptsIncr;

                    disc.MaleDisciplinePoints.Clear();
                    Businessobjects.SQLAddAndReturnFunctions.addPointsAndResultsMale(disc, resIsDistance, minRes, resIncr, minPts, ptsIncr);
                    break;

                case "female":
                    disc.MinResultF = Math.Round(minResF, 1);
                    disc.ResultIncrF = Math.Round(resIncrF, 1);
                    disc.MinPointsF = minPtsF;
                    disc.PointsIncrF = ptsIncrF;

                    disc.FemaleDisciplinePoints.Clear();
                    Businessobjects.SQLAddAndReturnFunctions.addPointsAndResultsFemale(disc, resIsDistance, minResF, resIncrF, minPtsF, ptsIncrF);
                    break;

                case "both":
                    disc.MinResult = Math.Round(minRes, 1);
                    disc.ResultIncr = Math.Round(resIncr, 1);
                    disc.MinPoints = minPts;
                    disc.PointsIncr = ptsIncr;

                    disc.MinResultF = Math.Round(minResF, 1);
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

        public static void filllBDiscSet(string discSetName, TextBox tBName, ListBox lBDiscSet)
        {
            RL_Datacontext.DisciplineSet discSet = (from d in dc.DisciplineSet
                                                    where d.Name == discSetName
                                                    select d).First();
            tBName.Text = discSet.Name;

            IEnumerable<RL_Datacontext.DisciplinesFromSet> discs = discSet.DisciplinesFromSet;

            foreach (var v in discs)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = v.Discipline;
                lBDiscSet.Items.Add(newItem);
            }
        }

        public static void updateDiscSet(string oldName, string newName, string[] disciplines)
        {
            RL_Datacontext.DisciplineSet set = (from d in dc.DisciplineSet
                                                where d.Name == oldName
                                                select d).First();

            if (set == null)
            {
                MessageBox.Show("Der Disziplin-Satz wurde während dem Bearbeiten gelöscht");
                return;
            }

            set.Name = newName;
            dc.SubmitChanges();
            set.DisciplinesFromSet.Clear();
            foreach (var v in disciplines)
            {
                RL_Datacontext.DisciplinesFromSet newDiscFromSet = new RL_Datacontext.DisciplinesFromSet()
                {
                    Discipline = v
                };
                set.DisciplinesFromSet.Add(newDiscFromSet);
            }

            dc.SubmitChanges();
        }

        public static void filllBDiscSets(ListBox discSets)
        {
            discSets.Items.Clear();

            IEnumerable<RL_Datacontext.DisciplineSet> sets = from s in dc.DisciplineSet
                                                             select s;

            foreach(RL_Datacontext.DisciplineSet discset in sets)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = discset.Name;
                discSets.Items.Add(item);
            }
        }
    }
}
