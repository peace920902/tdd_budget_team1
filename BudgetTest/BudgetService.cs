using System;
using System.Collections.Generic;
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
        if (end < start) return 0m;

        var monthPerDayAmountMap = new Dictionary<string, int>();

        var result = 0m;
        while (start <= end)
        {
            var monthYear = start.ToString("yyyyMM");
            int monthPerDayAmount;
            if(monthPerDayAmountMap.ContainsKey(monthYear))
            { 
                monthPerDayAmount = monthPerDayAmountMap[monthYear];
            }
            else
            {
                var budget = GetMonthBudget(start);
                monthPerDayAmount = GetBudgetPerDay(start , budget.Amount);
                monthPerDayAmountMap.Add(monthYear, monthPerDayAmount);
            }
           
            result += monthPerDayAmount;
            start = start.AddDays(1);
        }
        return result * 100 / 100m;
    }

    private Budget GetMonthBudget(DateTime date)
    {
        var budgets = _budgetRepo.GetAll();
        var budget = budgets.FirstOrDefault(x => x.YearMonth == $"{date:yyyyMM}");
        if (budget == null)
        {
            return new Budget
            {
                YearMonth = date.ToString("yyyyMM"),
                Amount = 0
            };
        }

        return budget;
    }

    private static int GetDayDiff(DateTime start, DateTime end)
    {
        var diffDays = (end - start).Days + 1;
        return diffDays;
    }

    private static decimal CalculateAmount(int diffDays, int budgetPerDay)
    {
        return diffDays * budgetPerDay * 100 / 100m;
    }

    private static int GetBudgetPerDay(DateTime date, int amount)
    {
        return amount / DateTime.DaysInMonth(date.Year, date.Month);
    }
}