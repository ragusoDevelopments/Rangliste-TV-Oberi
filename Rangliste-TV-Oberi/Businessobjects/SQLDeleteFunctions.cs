using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Rangliste_TV_Oberi.Businessobjects
{
    static class SQLDeleteFunctions
    {
        private static RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

        /// <summary>
        /// Deletes a discipline from the database
        /// </summary>
        /// <param name="disciplineName"></param>
        public static void deleteDiscipline(string disciplineName)
        {
            RL_Datacontext.Disciplines delDisc = (from d in dc.Disciplines
                                                  where d.DisciplineName == disciplineName
                                                  select d).FirstOrDefault();
            delDisc.MaleDisciplinePoints.Clear();
            delDisc.FemaleDisciplinePoints.Clear();


            dc.Disciplines.DeleteOnSubmit(delDisc);
            dc.SubmitChanges();
        }

        public static void deleteDiscSet(string discSetname)
        {
            RL_Datacontext.DisciplineSet delDiscSet = (from d in dc.DisciplineSet
                                                  where d.Name == discSetname
                                                  select d).First();

            delDiscSet.DisciplinesFromSet.Clear();


            dc.DisciplineSet.DeleteOnSubmit(delDiscSet);
            dc.SubmitChanges();
        }

        public static void filllBDiscSets(ListBox lBDiscSets)
        {
            lBDiscSets.Items.Clear();
            IEnumerable<RL_Datacontext.DisciplineSet> discSets = from d in dc.DisciplineSet
                                                                 select d;

            foreach(var v in discSets)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = v.Name;
                lBDiscSets.Items.Add(newItem);
            }
        }
    }
}
