using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Rangliste_TV_Oberi.Businessobjects
{
    class Discipline
    {
        RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

        public void addDiscipline(string discName, bool resIsDist, float minRes, float resIncr, int minPts, int ptsIncr, float minResF, float resIncrF, int minPtsF, int ptsIncrF, string gender)
        {
            RL_Datacontext.Disciplines newDisc = new RL_Datacontext.Disciplines();
            newDisc.DisciplineName = discName;
            newDisc.IsDistance = resIsDist;

            switch (gender)
            {
                case "male":
                    {
                        newDisc.MinResult = Math.Round(minRes, 1);
                        newDisc.ResultIncr = Math.Round(resIncr, 1);
                        newDisc.MinPoints = minPts;
                        newDisc.PointsIncr = ptsIncr;
                        break;
                    }
                case "female":
                    {
                        newDisc.MinResultF = Math.Round(minResF, 1);
                        newDisc.ResultIncrF = Math.Round(resIncrF, 1);
                        newDisc.MinPointsF = minPtsF;
                        newDisc.PointsIncrF = ptsIncrF;
                        break;
                    }
                case "both":
                    {
                        newDisc.MinResult = Math.Round(minRes, 1);
                        newDisc.ResultIncr = Math.Round(resIncr, 1);
                        newDisc.MinPoints = minPts;
                        newDisc.PointsIncr = ptsIncr;

                        newDisc.MinResultF = Math.Round(minResF, 1);
                        newDisc.ResultIncrF = Math.Round(resIncrF, 1);
                        newDisc.MinPointsF = minPtsF;
                        newDisc.PointsIncrF = ptsIncrF;
                        break;
                    }
            }
            dc.Disciplines.InsertOnSubmit(newDisc);
            dc.SubmitChanges();

            switch (gender)
            {
                case "male":
                    {
                        addPointsAndResultsMale(newDisc, resIsDist, minRes, resIncr, minPts, ptsIncr);
                        break;
                    }
                case "female":
                    {
                        addPointsAndResultsFemale(newDisc, resIsDist, minResF, resIncrF, minPtsF, ptsIncrF);
                        break;
                    }
                case "both":
                    {
                        addPointsAndResultsMale(newDisc, resIsDist, minRes, resIncr, minPts, ptsIncr);
                        addPointsAndResultsFemale(newDisc, resIsDist, minResF, resIncrF, minPtsF, ptsIncrF);
                        break;
                    }
            }
        }

        public void addPointsAndResultsMale(RL_Datacontext.Disciplines disc, bool resIsDist, float minRes, float resIncr, int minPts, int ptsIncr)
        {
            float currentResult = minRes;
            int currentPoints = minPts;

            if (resIsDist)
            {
                for (int i = minPts; i <= 100; i += ptsIncr)
                {
                    RL_Datacontext.MaleDisciplinePoints newDiscPts = new RL_Datacontext.MaleDisciplinePoints()
                    {
                        Result = Math.Round(currentResult, 1),
                        Points = currentPoints
                    };
                    disc.MaleDisciplinePoints.Add(newDiscPts);
                    currentResult += resIncr;
                    currentPoints += ptsIncr;
                }
            }
            else
            {
                for (int i = minPts; i <= 100; i += ptsIncr)
                {
                    RL_Datacontext.MaleDisciplinePoints newDiscPts = new RL_Datacontext.MaleDisciplinePoints()
                    {
                        Result = Math.Round(currentResult, 1),
                        Points = currentPoints
                    };
                    disc.MaleDisciplinePoints.Add(newDiscPts);
                    currentResult -= resIncr;
                    currentPoints += ptsIncr;
                }
            }
            dc.SubmitChanges();
        }

        public void addPointsAndResultsFemale(RL_Datacontext.Disciplines disc, bool resIsDist, float minResF, float resIncrF, int minPtsF, int ptsIncrF)
        {
            float currentResult = minResF;
            int currentPoints = minPtsF;

            if (resIsDist)
            {
                for (int i = minPtsF; i <= 100; i += ptsIncrF)
                {
                    RL_Datacontext.FemaleDisciplinePoints newDiscPts = new RL_Datacontext.FemaleDisciplinePoints()
                    {
                        Result = Math.Round(currentResult, 1),
                        Points = currentPoints
                    };
                    disc.FemaleDisciplinePoints.Add(newDiscPts);
                    currentResult += resIncrF;
                    currentPoints += ptsIncrF;
                }
            }
            else
            {
                for (int i = minPtsF; i <= 100; i += ptsIncrF)
                {
                    RL_Datacontext.FemaleDisciplinePoints newDiscPts = new RL_Datacontext.FemaleDisciplinePoints()
                    {
                        Result = Math.Round(currentResult, 1),
                        Points = currentPoints
                    };
                    disc.FemaleDisciplinePoints.Add(newDiscPts);
                    currentResult -= resIncrF;
                    currentPoints += ptsIncrF;
                }
            }
            dc.SubmitChanges();
        }

        public void updateDiscipline(string oldDicsName, string newDiscName, bool resIsDistance, float minRes, float resIncr, int minPts, int ptsIncr, float minResF, float resIncrF, int minPtsF, int ptsIncrF, string gender)
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
                    addPointsAndResultsMale(disc, resIsDistance, minRes, resIncr, minPts, ptsIncr);
                    break;

                case "female":
                    disc.MinResultF = Math.Round(minResF, 1);
                    disc.ResultIncrF = Math.Round(resIncrF, 1);
                    disc.MinPointsF = minPtsF;
                    disc.PointsIncrF = ptsIncrF;

                    disc.FemaleDisciplinePoints.Clear();
                    addPointsAndResultsFemale(disc, resIsDistance, minResF, resIncrF, minPtsF, ptsIncrF);
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
                    addPointsAndResultsMale(disc, resIsDistance, minRes, resIncr, minPts, ptsIncr);
                    addPointsAndResultsFemale(disc, resIsDistance, minResF, resIncrF, minPtsF, ptsIncrF);
                    break;
            }

            disc.DisciplineName = newDiscName;
            dc.SubmitChanges();


        }

        /// <summary>
        /// Deletes a discipline from the database
        /// </summary>
        /// <param name="disciplineName"></param>
        public void deleteDiscipline(string disciplineName)
        {
            RL_Datacontext.Disciplines delDisc = (from d in dc.Disciplines
                                                  where d.DisciplineName == disciplineName
                                                  select d).FirstOrDefault();
            delDisc.MaleDisciplinePoints.Clear();
            delDisc.FemaleDisciplinePoints.Clear();


            dc.Disciplines.DeleteOnSubmit(delDisc);
            dc.SubmitChanges();
        }

        /// <summary>
        /// Adds a set of disciplines to the database
        /// </summary>
        /// <param name="discSetName"></param>
        /// <param name="disciplines"></param>
        public void addDiscSet(string discSetName, string[] disciplines)
        {
            RL_Datacontext.DisciplineSet newDiscSet = new RL_Datacontext.DisciplineSet()
            {
                Name = discSetName
            };

            dc.DisciplineSet.InsertOnSubmit(newDiscSet);

            foreach (string discipline in disciplines)
            {
                RL_Datacontext.DisciplinesFromSet discsFromSet = new RL_Datacontext.DisciplinesFromSet()
                {
                    Discipline = discipline
                };
                newDiscSet.DisciplinesFromSet.Add(discsFromSet);
            }


            dc.SubmitChanges();


        }

        public void updateDiscSet(string oldName, string newName, string[] disciplines)
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

        public void deleteDiscSet(string discSetname)
        {
            RL_Datacontext.DisciplineSet delDiscSet = (from d in dc.DisciplineSet
                                                       where d.Name == discSetname
                                                       select d).First();

            delDiscSet.DisciplinesFromSet.Clear();


            dc.DisciplineSet.DeleteOnSubmit(delDiscSet);
            dc.SubmitChanges();
        }




        //Helperfunctions

        /// <summary>
        /// returns all Disciplines
        /// </summary>
        /// <returns>all Disciplines</returns>
        public IEnumerable<RL_Datacontext.Disciplines> returnDisciplines(string discName)
        {
            if (discName == null)
            {
                IEnumerable<RL_Datacontext.Disciplines> discs = from d in dc.Disciplines
                                                                select d;
                return discs;
            }
            else
            {
                IEnumerable<RL_Datacontext.Disciplines> discs = from d in dc.Disciplines
                                                                where d.DisciplineName == discName
                                                                select d;
                return discs;
            }
        }

        /// <summary>
        /// checks if a discipline with the name discName alreasy exits
        /// </summary>
        /// <param name="discName">New disciplines name</param>
        /// <returns>false if it already exits</returns>
        public bool checkDisciplines(string discName)
        {
            IEnumerable<RL_Datacontext.Disciplines> discs = returnDisciplines(discName);

            if (discs.Count() == 0)
                return true;
            else
                return false;

        }

        public void fillwPAddDisc(string currentName, TextBox name, ComboBox resType, TextBox minres, TextBox resIncr, TextBox minPts, TextBox ptsIncr, TextBox minresF, TextBox resIncrF, TextBox minPtsF, TextBox ptsIncrF)
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

        public void filllBDiscipline(ListBox lBDisciplines)
        {
            RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();
            lBDisciplines.Items.Clear();

            IEnumerable<RL_Datacontext.Disciplines> discs = returnDisciplines(null);

            foreach (var v in discs)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = v.DisciplineName;

                lBDisciplines.Items.Add(newItem);
            }
        }

        /// <summary>
        /// checks if a discipline set with the name discSetName alreasy exits
        /// </summary>
        /// <param name="discName">New disciplines name</param>
        /// <returns>false if it already exits</returns>
        public bool checkDisciplineSets(string discSetName)
        {
            IEnumerable<RL_Datacontext.DisciplineSet> discSets = from d in dc.DisciplineSet
                                                                 where d.Name == discSetName
                                                                 select d;

            if (discSets.Count() == 0)
                return true;
            else
                return false;

        }

        public void filllBDiscSet(string discSetName, TextBox tBName, ListBox lBDiscSet)
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

        public void filllBDiscSets(ListBox discSets)
        {
            discSets.Items.Clear();

            IEnumerable<RL_Datacontext.DisciplineSet> sets = from s in dc.DisciplineSet
                                                             select s;

            foreach (RL_Datacontext.DisciplineSet discset in sets)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = discset.Name;
                discSets.Items.Add(item);
            }
        }

        public IEnumerable<RL_Datacontext.DisciplineSet> returnDisciplineSets(string discSetName)
        {
            IEnumerable<RL_Datacontext.DisciplineSet> sets = null;

            if (discSetName == null)
                sets = from s in dc.DisciplineSet
                       select s;

            else
                sets = from s in dc.DisciplineSet
                       where s.Name == discSetName
                       select s;



            return sets;
        }



    }
}