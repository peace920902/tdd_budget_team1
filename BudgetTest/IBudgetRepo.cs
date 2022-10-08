using System.Collections.Generic;

namespace BudgetTest;

public interface IBudgetRepo
{
    IEnumerable<Budget> GetAll();
}