using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialSimulation
{
    internal class Investment
    {
        Dictionary<int, double> amountForMonth = new Dictionary<int, double>();

        double amount;
        double monthlyContribution;

        double interestRate;

        public Investment(double amount, double monthlyContribution, double interestRate)
        {
            this.amount = amount;
            this.monthlyContribution = monthlyContribution;
            this.interestRate = interestRate;
        }

        public void updateMonthlyPayment(int currentMonth, double contribution)
        {
            if (currentMonth > 0)
            {
                getAmountForMonth(currentMonth - 1);
            }

            monthlyContribution = contribution;
        }

        public void CalculateIterations(int numberOfYears)
        {
            for (int i = 0; i < 12 * numberOfYears; i++)
            {
                getAmountForMonth(i);   
            }
        }

        public double getAmountForMonth(int month)
        {
            if (amountForMonth.ContainsKey(month))
            {
                return amountForMonth[month];
            }

            if (month == 0)
            {
                return amount;
            }

            double previousMonthAmount = getAmountForMonth(month - 1);


            double currentMonthAmount = (previousMonthAmount + (previousMonthAmount * (interestRate / 12))) + monthlyContribution;
            amountForMonth[month] = currentMonthAmount;

            return currentMonthAmount;
        }
    }
}
