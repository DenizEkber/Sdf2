using Sdf2.Database.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sdf2.Services
{
    public class BaseService<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public BaseService(IRepository<T> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<T>> GetAllAsync() => _repository.GetAllAsync();
        public Task<T> GetByIdAsync(Expression<Func<T, bool>> predicate) => _repository.FindFirstAsync(predicate);
        public Task AddAsync(T entity) => _repository.AddAsync(entity);
        public Task UpdateAsync(T entity) => _repository.UpdateAsync(entity);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }

}
