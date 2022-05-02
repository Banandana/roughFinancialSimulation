using financialSimulation;



int downPayment = 10000;
int cost = 500000;

// fix down at 5%
downPayment = (int)(cost * 0.05f);

float rent = 1960;
Amortization amortization = new Amortization(cost, downPayment, 0.04, 0.002, 30);
double totalCost = amortization.CalculateTotalCost();
double firstMonthPayment = amortization.getMonthlyPayment(0);
double firstMonthPrincipal = amortization.getPrincipalForMonth(0);

Console.WriteLine("Down Payment: " + downPayment);
Console.WriteLine("Cost of House: " + cost);
Console.WriteLine("Loan Amount: " + (cost - downPayment));

int calc20perc = amortization.Get20PercentMonth();
int years = calc20perc / 12;
int months = calc20perc % 12;

Console.WriteLine("20% Equity: " + years + " years, " + months + " months");
Console.WriteLine("Total PMI: " + amortization.getTotalPMICost());
Console.WriteLine("Monthly PMI: " + amortization.getPMI());

Console.WriteLine("Loan Payment = " + firstMonthPayment);
Console.WriteLine("Loan Insurance = " + amortization.getPMI());
Console.WriteLine("Loan Total Cost = " + totalCost);
//Console.WriteLine("Total PMI Cost = " + amortization.)

double rentingInvestment = firstMonthPayment - rent;
Investment simpleInvestment = new Investment(downPayment, rentingInvestment, 0.06);
//Console.WriteLine("Rent = " + rent);
//Console.WriteLine("Monthly Renting Investment = " + rentingInvestment);

//Investment postMortgageInvestment = new Investment(0, 0, 0.06);


for (int i = 0; i < 70 * 12; i++)
{
    if (i < 30 * 12)
    {
        //postMortgageInvestment.updateMonthlyPayment(i, amortization.getMonthlyPayment(0) - amortization.getMonthlyPayment(i));
    } else if (i > 30 * 12)
    {
        // Stop contributing at 35 year mark
        //postMortgageInvestment.updateMonthlyPayment(i, 0);
        simpleInvestment.updateMonthlyPayment(i, -rent);
    }
    //postMortgageInvestment.getAmountForMonth(i);
    simpleInvestment.getAmountForMonth(i);
}

void printYear(int year)
{
    Console.WriteLine("Year  " + (year + 1));
    int month = year * 12;

    Console.WriteLine("\tMonthly Payment = " + amortization.getMonthlyPayment(month));
    Console.WriteLine("\tPrincipal Payment = " + amortization.getPrincipalForMonth(month));
    Console.WriteLine("\tOutstanding = " + amortization.getLoanBalanceAtMonth(month));
    Console.WriteLine("\tEquity Balance = " + amortization.getTotalEquityForMonth(month));
    //Console.WriteLine("\tPostmortgage Balance = " + postMortgageInvestment.getAmountForMonth(month));
    //Console.WriteLine("\n\tTotal House Buying Worth = " + (amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)));

    //Console.WriteLine("\tRenting Balance          = " + simpleInvestment.getAmountForMonth(month));
    //Console.WriteLine("\tHypothetical House Upside          = " + ((amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)) - simpleInvestment.getAmountForMonth(month)));
}

void printMonths(int year)
{
    Console.WriteLine("Year  " + (year + 1));

    double interest = 0;

    for (int month = (year * 12) - 12; month < (year * 12); month++)
    {
        Console.WriteLine("Month " + month);
        Console.WriteLine("\tMonthly Payment = " + amortization.getMonthlyPayment(month));
        Console.WriteLine("\tPrincipal Payment = " + amortization.getPrincipalForMonth(month));
        Console.WriteLine("\tOutstanding = " + amortization.getLoanBalanceAtMonth(month));
        Console.WriteLine("\tEquity Balance = " + amortization.getTotalEquityForMonth(month));
        Console.WriteLine("\tEquity % = " + (amortization.getTotalEquityForMonth(month) / cost));
        //Console.WriteLine("\tPostmortgage Balance = " + postMortgageInvestment.getAmountForMonth(month));
        //Console.WriteLine("\n\tTotal House Buying Worth = " + (amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)));

        //Console.WriteLine("\tRenting Balance          = " + simpleInvestment.getAmountForMonth(month));

       // Console.WriteLine("\tHypothetical House Upside          = " + ((amortization.getTotalEquityForMonth(month) + postMortgageInvestment.getAmountForMonth(month)) - simpleInvestment.getAmountForMonth(month)));

        Console.WriteLine("\tInterest accumulation = " + (amortization.getMonthlyPayment(month) - amortization.getPrincipalForMonth(month) - amortization.getPMI()));
        interest += amortization.getMonthlyPayment(month) - amortization.getPrincipalForMonth(month) - amortization.getPMI();
    }

    Console.WriteLine("Interest paid for year " + year + " = " + interest);


}

printYear(0);
printYear(1);
printYear(2);
printYear(3);
printYear(4);

printYear(9);

printYear(14);
printYear(19);
/**
for (int i = 2; i < 10; i++)
{
    Amortization amortization = new Amortization(399000, i * 10000, 0.035, 0.007, 30);
    double totalCost = amortization.CalculateTotalCost();
    double firstMonthPayment = amortization.getMonthlyPayment(0);
    double firstMonthPrincipal = amortization.getPrincipalForMonth(0);

    Console.WriteLine("Down Payment: " + (i * 10000));
    Console.WriteLine("Loan Payment = " + firstMonthPayment);
    Console.WriteLine("First Principal = " + firstMonthPrincipal);
    Console.WriteLine("Loan Insurance = " + amortization.getPMI());
    Console.WriteLine("Loan Total Cost = " + totalCost);
    Console.WriteLine("\n");
}
**/