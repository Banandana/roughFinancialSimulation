using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialSimulation
{
    internal class FinancialState
    {
        Amortization mortgage;
        Investment investment;

        public double getNet(int month)
        {
            double balance = 0;

            if (mortgage != null)
            {
                balance = mortgage.getRawEquityBalanceForMonth(month);
            }

            return balance + investment.getAmountForMonth(month);
        }
    }
}
