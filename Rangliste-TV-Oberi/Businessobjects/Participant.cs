using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

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
            int startnumber = checkStartnumbers() + 1;

            RL_Datacontext.Participants newPart = new RL_Datacontext.Participants()
            {
                Startnumber = startnumber,
                Name = name,
                Gender = gender,
                YearOfBirth = yearOfBirth,
                StatusIndex = cBSelInd
            };
            if (getCategory(yearOfBirth) == "")
                return;
            else
                newPart.Category = getCategory(yearOfBirth);

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

        public IEnumerable<RL_Datacontext.Participants> returnParticipants(string category, string gender)
        {
            IEnumerable<RL_Datacontext.Participants> participants = null;

            participants = from p in dc.Participants
                           where p.Category == category
                           where p.Gender == gender
                           select p;

            return participants;
        }

        public IEnumerable<RL_Datacontext.Participants> returnParticipants()
        {
            IEnumerable<RL_Datacontext.Participants> participants = from p in dc.Participants
                                                                    select p;

            return participants;
        }

        private void calculatePoints(int Id)
        {
            IEnumerable<RL_Datacontext.Participants> participants = returnParticipants();

            foreach (var v in participants)
            {
                IEnumerable<RL_Datacontext.Results> results = v.Results;
                int totalPoints = 0;

                foreach (var v2 in results)
                {
                    totalPoints += (int)v2.Points;
                }

                v.TotalPoints = totalPoints;

                dc.SubmitChanges();
            }
        }

        public IEnumerable<RL_Datacontext.Participants> returnOrderedParticipantForRating(string gender, string category)
        {
            IEnumerable<RL_Datacontext.Participants> participants = from p in dc.Participants
                                                                    where p.Category == category
                                                                    where p.Gender == gender
                                                                    select p;

            foreach (var v in participants)
                calculatePoints(v.Id);

            participants = from p in dc.Participants
                           where p.Category == category
                           where p.Gender == gender
                           orderby p.TotalPoints descending
                           select p;
            int rank = 0;
            
            for(int i = 0; i <= participants.Count()-1 ;i ++)
            {
                if(i > 0 && participants.ElementAt(i).TotalPoints == participants.ElementAt(i-1).TotalPoints)
                {
                    participants.ElementAt(i).Rank = rank;
                }
                else
                {
                    rank++;
                    participants.ElementAt(i).Rank = rank;  
                }
            }

            return participants;



        }

        public bool updateParticipant(int participantId, string name, int yearOfBirth, int statusIndex, string gender, WrapPanel wPDisciplines)
        {
            RL_Datacontext.Participants participant = (from p in dc.Participants
                                                       where p.Id == participantId
                                                       select p).First();
            if (getCategory(yearOfBirth) == "")
                return false;

            participant.Name = name;
            participant.YearOfBirth = yearOfBirth;
            participant.Category = getCategory(yearOfBirth);
            participant.StatusIndex = statusIndex;
            participant.Gender = gender;


            IEnumerable<WrapPanel> panels = wPDisciplines.Children.OfType<WrapPanel>();
            List<Businessobjects.Result> results = new List<Businessobjects.Result>();
            Businessobjects.Result newRes = new Result(gender);

            foreach (var v in panels)
            {
                foreach (Label lbl in v.Children.OfType<Label>())
                {
                    if (lbl.Name == "lblDiscName")
                    {
                        newRes.DisciplineName = lbl.Content.ToString();
                    }
                }
                foreach (TextBox tB in v.Children.OfType<TextBox>())
                {
                    tB.Text = tB.Text.Replace(".", ",");
                    float result = 0;
                    try
                    {
                        result = (float)Convert.ToDouble(tB.Text);
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                    newRes.result = result;
                }
                newRes.getPoints();
                results.Add(newRes);
                newRes = new Result(gender);
            }

            participant.Results.Clear();

            foreach (var v in results)
            {
                RL_Datacontext.Results res = new RL_Datacontext.Results()
                {
                    Discipline = v.DisciplineName,
                    Result = v.result,
                    Points = v.Points
                };
                participant.Results.Add(res);
            }

            dc.SubmitChanges();

            return true;
        }

        /// <summary>
        /// finds out the participants category by his age
        /// </summary>
        /// <param name="yearOfBirth"></param>
        /// <param name="currentYear"></param>
        /// <returns></returns>
        public string getCategory(int yearOfBirth)
        {
            DateTime dt = DateTime.Now;
            int currentYear = dt.Year;
            if (currentYear - yearOfBirth <= 5 || currentYear - yearOfBirth >= 16)
                return "";

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
