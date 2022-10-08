namespace BudgetTest;

public class Budget
{
    public Budget()
    {
    }

    public Budget(string yearMonth, int amount)
    {
        YearMonth = yearMonth;
        Amount = amount;
    }

    public string YearMonth { get; set; } = default!;
    public int Amount { get; set; }
}