using Sdf2.Database.CodeFirst.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdf2.Core.Interface
{
    public interface IExpenseService
    {
        IEnumerable<Expense> GetExpenses();
        IEnumerable<ExpenseType> GetExpenseTypes();
        void AddExpense(DateTime date, int expenseTypeId, decimal amount);
    }
}
