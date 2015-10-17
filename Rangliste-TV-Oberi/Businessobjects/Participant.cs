using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Rangliste_TV_Oberi.Businessobjects
{
    class Participant
    {
        private static RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

        /// <summary>
        /// fills the status-table 
        /// </summary>
        public void fillStatusTable()
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
        /// Adds a new participant to the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="yearOfBirth"></param>
        public void addParticipant(string name, string gender, int yearOfBirth, int cBSelInd)
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

        }

        public RL_Datacontext.Participants returnParticipant(int startNumber)
        {
            RL_Datacontext.Participants part = null;
            try
            {
                part = (from p in dc.Participants
                        where p.Startnumber == startNumber
                        select p).First();
            }
            catch (Exception)
            {
                MessageBox.Show("Teilnehmer existiert nicht");
                return null;
            }

            return part;
        }

        /// <summary>
        /// finds out the participants category by his age
        /// </summary>
        /// <param name="yearOfBirth"></param>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        public string getCategory(int yearOfBirth, int currentYear)
        {
            IEnumerable<RL_Datacontext.Categories> cats = from c in dc.Categories
                                                          where c.Age == currentYear - yearOfBirth
                                                          select c;

            return cats.ElementAt(0).Category;
        }

        /// <summary>
        /// fills the Category Table with the standard ages
        /// </summary>
        public void fillCategoriesTable()
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
        /// gets the biggest startnumber in the database
        /// </summary>
        /// <returns></returns>
        public int checkStartnumbers()
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
    }

}
