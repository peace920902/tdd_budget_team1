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
        if (start.ToString("yyyyMM") != end.ToString("yyyyMM"))
        {
            var result = 0m;
            var currentStart = start.AddMonths(1);

            while (currentStart < end)
            {
                var monthBudget = GetMonthBudget(currentStart);
                result += monthBudget.Amount;
                currentStart = currentStart.AddMonths(1);
            }
            
            var budgetStart = GetMonthBudget(start);
            var budgetEnd = GetMonthBudget(end);
            
            var startBudgetPerDay = GetBudgetPerDay(start , budgetStart.Amount);
            var endBudgetPerDay = GetBudgetPerDay(end , budgetEnd.Amount);
            
            var dayDiffStart = GetDayDiff(start, new DateTime(start.Year,start.Month,DateTime.DaysInMonth(start.Year,start.Month)));
            var dayDiffEnd = GetDayDiff(new DateTime(end.Year,end.Month,01), end);
            
            var startAmount = CalculateAmount(dayDiffStart , startBudgetPerDay);
            var endAmount = CalculateAmount(dayDiffEnd , endBudgetPerDay);
            
            var amount = startAmount + endAmount;
            return result + amount;
        }   
        var budget = GetMonthBudget(start);
        var budgetPerDay = GetBudgetPerDay(start, budget.Amount);
        var diffDays = GetDayDiff(start, end);
        return CalculateAmount(diffDays, budgetPerDay);
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