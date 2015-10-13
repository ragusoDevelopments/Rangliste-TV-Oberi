using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rangliste_TV_Oberi.Businessobjects
{
    static class SQLUpdateFuntions
    {
        private static RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

        public static RL_Datacontext.Disciplines updateDiscipline(string newDiscName, string oldDicsName)
        {
            RL_Datacontext.Disciplines disc = (from d in dc.Disciplines
                                               where d.DisciplineName == oldDicsName
                                               select d).First();
            disc.DisciplineName = newDiscName;
            dc.SubmitChanges();

            return disc;

        }
    }
}
