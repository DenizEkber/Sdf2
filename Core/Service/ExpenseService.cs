using Sdf2.Core.Interface;
using Sdf2.Database.CodeFirst.Entity;
using Sdf2.Database.Interface;
using Sdf2.Services;

namespace Sdf2.Core.Service
{
    public class ExpenseService : BaseService<Expense>, IExpenseService
    {
        private readonly IExpenseRepository _repository;

        public ExpenseService(IExpenseRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public IEnumerable<Expense> GetExpenses()
        {
            return _repository.GetExpenses();
        }

        public IEnumerable<ExpenseType> GetExpenseTypes()
        {
            return _repository.GetExpenseTypes();
        }

        public void AddExpense(DateTime date, int expenseTypeId, decimal amount)
        {
            var expense = new Expense
            {
                Date = date,
                ExpenseTypeId = expenseTypeId,
                Amount = amount
            };

            _repository.AddExpense(expense);
        }
    }
}
