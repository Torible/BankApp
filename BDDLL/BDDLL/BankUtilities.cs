using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDLL
{
    public class BankUtilities
    {
        public static float CalculateProfit(float deposit, float rate, int period)
        {
            return deposit * (rate / 100.0f) * (period / 12.0f);
        }

        public static DateTime GetClosingDate(DateTime OpeningDate, int period)
        {
            return OpeningDate.AddMonths(period);
        }
    }
}
