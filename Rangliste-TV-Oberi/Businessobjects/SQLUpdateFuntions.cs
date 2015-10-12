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
                                               where d.Discipline == oldDicsName
                                               select d).First();
            disc.Discipline = newDiscName;
            dc.SubmitChanges();

            return disc;

        }

        public static void handlePointsToDiscipline(RL_Datacontext.Disciplines disc, float worstRes, float resIncr, int minPts, int ptsIncr, bool resIsDistance, bool male, string mode)
        {
            if (mode == "udate")
            {
                disc.MaleDisciplinePoints.Clear();
                disc.FemaleDisciplinePoints.Clear();
                dc.SubmitChanges();
            }

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
    }
}
