using Microsoft.EntityFrameworkCore;
using Sdf2.Database.CodeFirst.Context;
using Sdf2.Database.CodeFirst.Entity;
using Sdf2.Database.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sdf2.Database.Repository
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Expense> GetExpenses()
        {
            return _context.Expenses.ToList();
        }

        public IEnumerable<ExpenseType> GetExpenseTypes()
        {
            return _context.ExpenseTypes.ToList();
        }

        public void AddExpense(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<Expense>> GetAllAsync()
        {
            return await Task.FromResult(_context.Expenses.ToList());
        }

        public async Task<Expense> GetByIdAsync(int id)
        {
            return await Task.FromResult(_context.Expenses.FirstOrDefault(e => e.Id == id));
        }

        public async Task AddAsync(Expense entity)
        {
            await Task.Run(() =>
            {
                _context.Expenses.Add(entity);
                _context.SaveChanges();
            });
        }

        public async Task UpdateAsync(Expense entity)
        {
            var existing = _context.Expenses.FirstOrDefault(e => e.Id == entity.Id);
            if (existing != null)
            {
                existing.Amount = entity.Amount;
                existing.Date = entity.Date;
                existing.ExpenseTypeId = entity.ExpenseTypeId;
                await Task.Run(() => _context.SaveChanges());
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _context.Expenses.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                _context.Expenses.Remove(entity);
                await Task.Run(() => _context.SaveChanges());
            }
        }

        public async Task<Expense> FindFirstAsync(Expression<Func<Expense, bool>> predicate)
        {
            return await _context.Expenses.FirstOrDefaultAsync(predicate);
        }
    }
}
