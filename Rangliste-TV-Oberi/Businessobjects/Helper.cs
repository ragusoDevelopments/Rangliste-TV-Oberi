using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rangliste_TV_Oberi.Businessobjects
{
    class Helper
    {
        public static int getCategory(int year)
        {
            DateTime dt = DateTime.Now;

            switch (dt.Year - year)
            {
                case 5:
                    return 0;
                case 6:
                    return 0;
                case 7:
                    return 0;
                case 8:
                    return 0;
                case 9:
                    return 1;
                case 10:
                    return 1;
                case 11:
                    return 1;
                case 12:
                    return 1;
                case 13:
                    return 2;
                case 14:
                    return 2;
                case 15:
                    return 2;
                case 16:
                    return 2;

                default:
                    return 3;
            }
        }
    }
}
