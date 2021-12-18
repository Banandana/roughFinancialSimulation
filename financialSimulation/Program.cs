using financialSimulation;

Console.WriteLine("Hello, World!");


int downPayment = 30000;
int cost = 450000;
float rent = 1960;

Amortization amortization = new Amortization(cost, downPayment, 0.035, 0.007, 30);
double totalCost = amortization.CalculateTotalCost();
double firstMonthPayment = amortization.getMonthlyPayment(0);
double firstMonthPrincipal = amortization.getPrincipalForMonth(0);

Console.WriteLine("Loan Payment = " + firstMonthPayment);
Console.WriteLine("Loan Insurance = " + amortization.getPMI());
Console.WriteLine("Loan Total Cost = " + totalCost);

double rentingInvestment = firstMonthPayment - rent;
Investment simpleInvestment = new Investment(downPayment, rentingInvestment, 0.06);
Console.WriteLine("Rent = " + rent);
Console.WriteLine("Monthly Investment = " + rentingInvestment);

Investment postMortgageInvestment = new Investment(0, 0, 0.06);


for (int i = 0; i < 70 * 12; i++)
{
    if (i < 30 * 12)
    {
        postMortgageInvestment.updateMonthlyPayment(i, amortization.getMonthlyPayment(0) - amortization.getMonthlyPayment(i));
    } else
    {
        // Stop contributing at 30 year mark
        postMortgageInvestment.updateMonthlyPayment(i, 0);
        simpleInvestment.updateMonthlyPayment(i, -rent);
    }
    postMortgageInvestment.getAmountForMonth(i);
    simpleInvestment.getAmountForMonth(i);
}

void printYear(int year)
{
    Console.WriteLine("Year  " + year);
    int month = year * 12;

    Console.WriteLine("\tMonthly Payment = " + amortization.getMonthlyPayment(month));
    Console.WriteLine("\tPrincipal Payment = " + amortization.getPrincipalForMonth(month));
    Console.WriteLine("\tOutstanding = " + amortization.getLoanBalanceAtMonth(month));
    Console.WriteLine("\tEquity Balance = " + amortization.getTotalEquityForMonth(month));
    Console.WriteLine("\tPostmortgage Balance = " + postMortgageInvestment.getAmountForMonth(month));
    Console.WriteLine("\n\tTotal House Buying Worth = " + (amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)));

    Console.WriteLine("\tRenting Balance          = " + simpleInvestment.getAmountForMonth(month));
    Console.WriteLine("\tHypothetical House Upside          = " + ((amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)) - simpleInvestment.getAmountForMonth(month)));
}

void printMonths(int year)
{
    Console.WriteLine("Year  " + year);

    double interest = 0;

    for (int month = (year * 12) - 12; month < (year * 12); month++)
    {
        Console.WriteLine("Month " + month);
        Console.WriteLine("\tMonthly Payment = " + amortization.getMonthlyPayment(month));
        Console.WriteLine("\tPrincipal Payment = " + amortization.getPrincipalForMonth(month));
        Console.WriteLine("\tOutstanding = " + amortization.getLoanBalanceAtMonth(month));
        Console.WriteLine("\tEquity Balance = " + amortization.getTotalEquityForMonth(month));
        Console.WriteLine("\tEquity % = " + (amortization.getTotalEquityForMonth(month) / cost));
        Console.WriteLine("\tPostmortgage Balance = " + postMortgageInvestment.getAmountForMonth(month));
        Console.WriteLine("\n\tTotal House Buying Worth = " + (amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)));

        Console.WriteLine("\tRenting Balance          = " + simpleInvestment.getAmountForMonth(month));

        Console.WriteLine("\tHypothetical House Upside          = " + ((amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)) - simpleInvestment.getAmountForMonth(month)));

        Console.WriteLine("\tInterest accumulation = " + (amortization.getMonthlyPayment(month) - amortization.getPrincipalForMonth(month) - amortization.getPMI()));
        interest += amortization.getMonthlyPayment(month) - amortization.getPrincipalForMonth(month) - amortization.getPMI();
    }

    Console.WriteLine("Interest paid for year " + year + " = " + interest);


}

printMonths(1);
printMonths(2);
printMonths(3);
printMonths(4);
printMonths(5);
printMonths(6);
printMonths(7);
printMonths(8);
printYear(30);
printYear(40);
printYear(50);
printYear(60);
printYear(70);
