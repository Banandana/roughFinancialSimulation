using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace financialSimulation
{
    internal class Amortization
    {
        int amount;
        int down;
        double interestRate;
        int numberOfYears;

        double mortgageInsuranceRate;

        Dictionary<int, double> calculatedPrinciple = new Dictionary<int, double>();
        Dictionary<int, double> calculatedPayment = new Dictionary<int, double>();
        Dictionary<int, double> calculatedBalance = new Dictionary<int, double>();
        Dictionary<int, double> calculatedEquity = new Dictionary<int, double>();
        double calculatedTotalCost;

        int calculated20PercentMonth = 0;

        public Amortization(int amount, int down, double interestRate, double insuranceRate, int numberOfYears)
        {
            this.down = down;
            this.amount = amount - down;
            this.interestRate = interestRate;
            this.mortgageInsuranceRate = insuranceRate;
            this.numberOfYears = numberOfYears;
        }

        public double getPreInsuranceMonthlyPayment()
        {
            double P = amount;
            double r = interestRate;
            double n = 12;
            double t = numberOfYears;

            return P * (r / n) * Math.Pow((1 + r / n), (n * t)) / (Math.Pow((1 + r / n), (n * t)) - 1);
        }

        public double CalculateTotalCost()
        {
            calculatedTotalCost = 0;
            for (int i = 0; i < numberOfYears * 12; i++)
            {
                getPrincipalForMonth(i);
                getLoanBalanceAtMonth(i);
                getRawEquityBalanceForMonth(i);

                calculatedTotalCost += getMonthlyPayment(i);
            }
            return calculatedTotalCost;
        }

        public int Get20PercentMonth()
        {
            return calculated20PercentMonth;
        }

        public void CalculateIterations(int totalYears)
        {
            for (int i = 0; i < totalYears * 12; i++)
            {
                getPrincipalForMonth(i);
                getLoanBalanceAtMonth(i);
                getRawEquityBalanceForMonth(i);
                getMonthlyPayment(i);
            }
        }

        public double getPMI()
        {
            // Calculate insurance based on rate
            double insurancePerYear = amount * mortgageInsuranceRate;
            double pmi = (insurancePerYear / 12);
            return pmi;
        }

        public double getTotalPMICost()
        {
            double pmi = getPMI();
            int pmiLength = Get20PercentMonth();

            return pmi * pmiLength;
        }

        public double getMonthlyPayment(int month)
        {
            if (calculatedPayment.ContainsKey(month))
            {
                return calculatedPayment[month];
            }

            if (month > numberOfYears * 12)
            {
                return 0;
            }

            double payment = getPreInsuranceMonthlyPayment();

            

            // Calculate 
            double equity = getTotalEquityForMonth(month);
            double percentageEquity = equity / amount;
            if (percentageEquity < 0.2)
            {
                payment += getPMI();
            }


            //Console.WriteLine("r / n = " + (r / n));

            //Console.WriteLine("1 + r / n ^ n * t = " +  Math.Pow((1 + r / n), (n * t)));

            //Console.WriteLine("1 + r / n ^ n * t - 1 = " + (Math.Pow((1 + r / n), (n * t)) - 1));

            // Magic Number 280: Rough property tax monthly amount.
            // Magic number 70: Homeowners insurance rate.
            calculatedPayment[month] = payment;// + 70 + 280.15;
            return payment;// + 70 + 280.15;
        }

        public double getLoanAmountPrepaymentAtMonth(int paymentNumber)
        {
            double resultingAmount = amount;

            for (int i = 0; i < paymentNumber; i++)
            {
                resultingAmount -= getPrincipalForMonth(i);
            }

            return resultingAmount;
        }

        public double getLoanBalanceAtMonth(int month)
        {
            if (month + 1 >= numberOfYears * 12)
            {
                calculatedBalance[month] = 0;
                return 0;
            }

            double balance = getLoanAmountPrepaymentAtMonth(month) - getPrincipalForMonth(month);

            calculatedBalance[month] = balance;

            return balance;
        }

        public double getPrincipalForMonth(int month)
        {
            if (calculatedPrinciple.ContainsKey(month))
            {
                return calculatedPrinciple[month];
            }

            double monthlyPayment = getPreInsuranceMonthlyPayment();
            double outstandingBalance = getLoanAmountPrepaymentAtMonth(month);

            double principle = monthlyPayment - (outstandingBalance * (interestRate / 12));
            calculatedPrinciple.Add(month, principle);

            return principle;
        }

        double getEquityPercent(double equity)
        {
            return (equity / (amount + down));
        }

        public double getRawEquityBalanceForMonth(int month)
        {
            if (calculatedEquity.ContainsKey(month))
            {
                return calculatedEquity[month];
            }

            double equity = amount - getLoanBalanceAtMonth(month);
            calculatedEquity[month] = equity;
            if (calculated20PercentMonth == 0 && month != 0)
            {
                if (getEquityPercent(calculatedEquity[month] + down) > 0.2 && getEquityPercent(calculatedEquity[month - 1] + down) < 0.2)
                {
                    calculated20PercentMonth = month;
                }
            } else if (month == 0)
            {
                return 0 + down;
            }


            return equity;
        }

        public double getTotalEquityForMonth(int month)
        {
            if (month >= numberOfYears * 12)
            {
                return amount + down;
            }

            return getRawEquityBalanceForMonth(month) + down;
        }
    }
}
