using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rangliste_TV_Oberi.Businessobjects
{
    class Result
    {
        RL_Datacontext.RLDBDataContext dc = new RL_Datacontext.RLDBDataContext();

        public string Gender;
        public string DisciplineName;
        public float result;
        public float roundedResult;
        public int Points;

        public Result(string gender)
        {
            Gender = gender;
        }

        public void getPoints()
        {
            roundedResult = (float) Math.Round(result, 1);

            RL_Datacontext.Disciplines disc = (from d in dc.Disciplines
                                               where d.DisciplineName == DisciplineName
                                               select d).First();

            if(Gender == "male")
            {
                IEnumerable<RL_Datacontext.MaleDisciplinePoints> pointsTable = disc.MaleDisciplinePoints;

                foreach (var v in pointsTable)
                {
                    if (v.Result == result)
                    {
                        Points = v.Points;
                        break;
                    }
                }
            }
            else
            {
                IEnumerable<RL_Datacontext.FemaleDisciplinePoints> pointsTable = disc.FemaleDisciplinePoints; 
                
                foreach (var v in pointsTable)
                {
                    if (v.Result == result)
                    {
                        Points = v.Points;
                        break;
                    }
                }
            }

            
        }
    }
}
