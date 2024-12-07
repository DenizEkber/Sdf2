using Sdf2.Database.CodeFirst.Entity;

namespace Sdf2.Database.Interface
{
    public interface IExpenseRepository : IRepository<Expense>
    {
        IEnumerable<Expense> GetExpenses();
        IEnumerable<ExpenseType> GetExpenseTypes();
        void AddExpense(Expense expense);
    }
}
