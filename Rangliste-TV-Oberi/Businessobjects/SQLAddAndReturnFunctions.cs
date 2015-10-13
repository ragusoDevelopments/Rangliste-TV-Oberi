using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Rangliste_TV_Oberi.Businessobjects
{
    static class SQLAddAndReturnFunctions
    {
        private static RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

        /// <summary>
        /// Adds a new participant to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="yearOfBirth"></param>
        public static void addParticipant(string name, string gender, int yearOfBirth, int cBSelInd)
        {
            DateTime dt = DateTime.Now;
            int startnumber = checkStartnumbers() + 1;

            IEnumerable<RL_Datacontext.Status> state = from c in dc.Status
                                                       where c.SelectedIndex == cBSelInd
                                                       select c;

            RL_Datacontext.Participants newPart = new RL_Datacontext.Participants()
            {
                Startnumber = startnumber,
                Name = name,
                Category = getCategory(yearOfBirth, dt.Year),
                Gender = gender,
                YearOfBirth = yearOfBirth,
                Status = state.ElementAtOrDefault(0).Status1
            };

            dc.Participants.InsertOnSubmit(newPart);
            dc.SubmitChanges();

            MainWindow main = (MainWindow)App.Current.MainWindow;
            main.listTable();
        }

        public static void addDiscipline(string discName, bool resIsDist, float minRes, float resIncr, int minPts, int ptsIncr, float minResF, float resIncrF, int minPtsF, int ptsIncrF, string gender)
        {
            RL_Datacontext.Disciplines newDisc = new RL_Datacontext.Disciplines();
            newDisc.DisciplineName = discName;
            newDisc.IsDistance = resIsDist;

            switch (gender)
            {
                case "male":
                    {
                        newDisc.MinResult = minRes;
                        newDisc.ResultIncr = resIncr;
                        newDisc.MinPoints = minPts;
                        newDisc.ResultIncr = resIncr;
                        break;
                    }
                case "female":
                    {
                        newDisc.MinResultF = minResF;
                        newDisc.ResultIncrF = resIncrF;
                        newDisc.MinPointsF = minPtsF;
                        newDisc.ResultIncrF = resIncrF;
                        break;
                    }
                case "both":
                    {
                        newDisc.MinResult = minRes;
                        newDisc.ResultIncr = resIncr;
                        newDisc.MinPoints = minPts;
                        newDisc.ResultIncr = resIncr;

                        newDisc.MinResultF = minResF;
                        newDisc.ResultIncrF = resIncrF;
                        newDisc.MinPointsF = minPtsF;
                        newDisc.ResultIncrF = resIncrF;
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

        private static void addPointsAndResultsMale(RL_Datacontext.Disciplines disc, bool resIsDist, float minRes, float resIncr, int minPts, int ptsIncr)
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

        private static void addPointsAndResultsFemale(RL_Datacontext.Disciplines disc, bool resIsDist, float minResF, float resIncrF, int minPtsF, int ptsIncrF)
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


        /// <summary>
        /// fills the Category Table with the standard ages
        /// </summary>
        public static void fillCategoriesTable()
        {
            string category = null;

            for (int i = 5; i <= 16; i++)
            {
                if (i >= 5 && i <= 9)
                    category = "US";
                if (i >= 10 && i <= 12)
                    category = "MS";
                if (i >= 13 && i <= 16)
                    category = "OS";


                RL_Datacontext.Categories newCat = new RL_Datacontext.Categories()
                {
                    Age = i,
                    Category = category
                };

                dc.Categories.InsertOnSubmit(newCat);
            }

            dc.SubmitChanges();
        }

        /// <summary>
        /// finds out the participants category by his age
        /// </summary>
        /// <param name="yearOfBirth"></param>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        public static string getCategory(int yearOfBirth, int currentYear)
        {
            IEnumerable<RL_Datacontext.Categories> cats = from c in dc.Categories
                                                          where c.Age == currentYear - yearOfBirth
                                                          select c;

            return cats.ElementAt(0).Category;
        }

        /// <summary>
        /// gets the biggest startnumber in the database
        /// </summary>
        /// <returns></returns>
        public static int checkStartnumbers()
        {
            #region testpurpose, not enabled
            /*
            RL_Datacontext.Participants newPart = new RL_Datacontext.Participants()
            {
                Startnumber = 1,
                YearOfBirth = 2000,
                Name = "test",
                Gender = "male",
                Category="OS"
            };

            dc.Participants.InsertOnSubmit(newPart);

            RL_Datacontext.Participants newPart2 = new RL_Datacontext.Participants()
            {
                Startnumber = 2,
                YearOfBirth = 2000,
                Name = "test",
                Gender = "male",
                Category = "OS"
            };

            dc.Participants.InsertOnSubmit(newPart2);
            dc.SubmitChanges();*/
            #endregion

            IEnumerable<RL_Datacontext.Participants> parts = from p in dc.Participants
                                                             orderby p.Startnumber descending
                                                             select p;

            if (parts.Count() == 0)
                return 0;
            else
                return parts.ElementAtOrDefault(0).Startnumber;

        }

        /// <summary>
        /// fills the status-table 
        /// </summary>
        public static void fillStatusTable()
        {
            string[] status = new string[] { "Aktiv", "Ausser Konkurrenz", "Disqualifiziert", "Fremder Tu/Ti", "Keine Anmeldung", "Nicht gestartet", "Unfall" };

            for (int i = 0; i <= 6; i++)
            {
                RL_Datacontext.Status newState = new RL_Datacontext.Status()
                {
                    Status1 = status[i],
                    SelectedIndex = i
                };

                dc.Status.InsertOnSubmit(newState);
            }

            dc.SubmitChanges();
        }

        /// <summary>
        /// Adds a set of disciplines to the database
        /// </summary>
        /// <param name="discSetName"></param>
        /// <param name="disciplines"></param>
        public static void addDiscSet(string discSetName, string[] disciplines)
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

        /// <summary>
        /// returns all Disciplines
        /// </summary>
        /// <returns>all Disciplines</returns>
        public static IEnumerable<RL_Datacontext.Disciplines> returnDisciplines(string discName)
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
        public static bool checkDisciplines(string discName)
        {
            IEnumerable<RL_Datacontext.Disciplines> discs = returnDisciplines(discName);

            if (discs.Count() == 0)
                return true;
            else
                return false;

        }
    }
}
