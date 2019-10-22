using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telephone
{
    class SalaryCALC
    {
        private string rank;
        double first;
        double second;


        public SalaryCALC(string r, double f, double s)
        {
            this.rank = r;
            this.first = f;
            this.second = s;
        }

        public double count()
        {
            double sec_T = first - second;
            double sal;


            if (rank == "1") 
                sal = (((int)sec_T / 3600) * 59.25) + ((sec_T % 3600 / 60) / 60) * 59.25; 
            
            else
                sal = (((int)sec_T / 3600) * 29.5) + ((sec_T % 3600 / 60) / 60) * 29.5;


            sal = Math.Round(sal, 2);
            return sal;
        }
    }
}
