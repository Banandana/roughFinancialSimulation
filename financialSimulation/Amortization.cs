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
            if (equity / amount <= 0.2)
            {
                payment += getPMI();
            }


            //Console.WriteLine("r / n = " + (r / n));

            //Console.WriteLine("1 + r / n ^ n * t = " +  Math.Pow((1 + r / n), (n * t)));

            //Console.WriteLine("1 + r / n ^ n * t - 1 = " + (Math.Pow((1 + r / n), (n * t)) - 1));

            calculatedPayment[month] = payment;
            return payment;
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

        public double getRawEquityBalanceForMonth(int month)
        {
            if (calculatedEquity.ContainsKey(month))
            {
                return calculatedEquity[month];
            }

            double equity = amount - getLoanBalanceAtMonth(month);
            calculatedEquity[month] = equity;


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
