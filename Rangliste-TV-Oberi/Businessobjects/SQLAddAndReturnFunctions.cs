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

        /// <summary>
        /// adds a new discipline to the databse
        /// </summary>
        /// <param name="discName"></param>
        /// <returns></returns>
        public static RL_Datacontext.Disciplines addDiscipline(string discName)
        {
            RL_Datacontext.Disciplines newDisc = new RL_Datacontext.Disciplines()
            {
                Discipline = discName
            };

            dc.Disciplines.InsertOnSubmit(newDisc);
            dc.SubmitChanges();

            return newDisc;
        }

        /// <summary>
        /// Adds the result-point table to a category
        /// </summary>
        /// <param name="disc"></param>
        /// <param name="worstRes"></param>
        /// <param name="resIncr"></param>
        /// <param name="minPts"></param>
        /// <param name="ptsIncr"></param>
        /// <param name="resIsDistance"></param>
        public static void addPointsToDiscipline(RL_Datacontext.Disciplines disc, float worstRes, float resIncr, int minPts, int ptsIncr, bool resIsDistance, bool male)
        {

            float currentRes = worstRes;
            int currentPts = minPts;

            if (male)
            {
                for (int pts = minPts; pts <= 100; pts += ptsIncr)
                {
                    RL_Datacontext.MaleDisciplinePoints newDiscPoint = new RL_Datacontext.MaleDisciplinePoints()
                    {
                        Result = Math.Round(currentRes, 1),
                        Points = currentPts
                    };

                    disc.MaleDisciplinePoints.Add(newDiscPoint);

                    if (resIsDistance)
                        currentRes += resIncr;
                    else
                        currentRes -= resIncr;

                    currentPts += ptsIncr;
                }
            }
            else
            {
                for (int pts = minPts; pts <= 100; pts += ptsIncr)
                {
                    RL_Datacontext.FemaleDisciplinePoints newDiscPoint = new RL_Datacontext.FemaleDisciplinePoints()
                    {
                        Result = Math.Round(currentRes, 1),
                        Points = currentPts
                    };

                    disc.FemaleDisciplinePoints.Add(newDiscPoint);

                    if (resIsDistance)
                        currentRes += resIncr;
                    else
                        currentRes -= resIncr;

                    currentPts += ptsIncr;
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
                                                                where d.Discipline == discName
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
