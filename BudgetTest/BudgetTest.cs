using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace BudgetTest;

public class BudgetTest
{
    private IBudgetRepo _budgetRepo;

    public BudgetTest()
    {
        _budgetRepo = Substitute.For<IBudgetRepo>();
    }

    [Fact]
    public void Partial_Month()
    {
        _budgetRepo.GetAll().Returns(new List<Budget>()
        {
            new()
            {
                YearMonth = "202210",
                Amount = 3100
            }
        });
        
        var budgetService = new BudgetService(_budgetRepo);
        var result = budgetService.Query(new DateTime(2022, 10, 01), new DateTime(2022, 10, 02));
        result.Should().Be(200m);
    }
}