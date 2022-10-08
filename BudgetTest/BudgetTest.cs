using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BudgetTest;

public class BudgetTest
{
    private IBudgetRepo _budgetRepo;
    private BudgetService _budgetService;

    public BudgetTest()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
        _budgetService = new BudgetService(_budgetRepo);
    }

    private void GivenBudgets(List<Budget> budgets)
    {
        _budgetRepo.GetAll().Returns(budgets);
    }

    [Fact]
    public void Invalid_Input()
    {
        var result = _budgetService.Query(new DateTime(2022, 12, 3), new DateTime(2022, 10, 31));
        result.Should().Be(0);
    }

    [Fact]
    public void Partial_Month()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202210",
                Amount = 3100
            }
        });
        var result = _budgetService.Query(new DateTime(2022, 10, 01), new DateTime(2022, 10, 02));
        result.Should().Be(200m);
    }

    [Fact]
    public void All_Month()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202210",
                Amount = 3100
            }
        });
        var result = _budgetService.Query(new DateTime(2022, 10, 01), new DateTime(2022, 10, 31));
        result.Should().Be(3100m);
    }

    [Fact]
    public void Cross_A_Month()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202210",
                Amount = 3100
            },
            new(yearMonth: "202211", amount: 300)
        });
        var result = _budgetService.Query(new DateTime(2022, 10, 31), new DateTime(2022, 11, 5));
        result.Should().Be(150m);
    }

    [Fact]
    public void Cross_Over_Two_Month()
    {
        GivenBudgets(new List<Budget>()
        {
            new()
            {
                YearMonth = "202210",
                Amount = 3100
            },
            new(yearMonth: "202211", amount: 300),
            new(yearMonth: "202212", amount:31),
        });
        var result = _budgetService.Query(new DateTime(2022, 10, 31), new DateTime(2022, 12, 3));
        result.Should().Be(100m+300m+3);
    }
}