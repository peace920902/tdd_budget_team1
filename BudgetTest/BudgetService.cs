using System;
using System.Linq;

namespace BudgetTest;

public class BudgetService
{
    private readonly IBudgetRepo _budgetRepo;

    public BudgetService(IBudgetRepo budgetRepo)
    {
        _budgetRepo = budgetRepo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        var budgets = _budgetRepo.GetAll();
        var budget = budgets.First(x => x.YearMonth == "202210");
        var budgetPerDay = GetBudgetPerDay(start, budget);
        var diffDays = (end - start).Days + 1;
        return diffDays * budgetPerDay * 100 / 100m;
    }

    private static int GetBudgetPerDay(DateTime start, Budget budget)
    {
        return budget.Amount / DateTime.DaysInMonth(start.Year, start.Month);
    }
}