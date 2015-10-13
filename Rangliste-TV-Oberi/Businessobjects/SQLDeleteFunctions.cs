using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
