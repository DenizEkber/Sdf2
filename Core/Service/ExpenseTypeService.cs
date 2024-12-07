using Sdf2.Database.CodeFirst.Entity;
using Sdf2.Database.Interface;
using Sdf2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdf2.Core.Service
{
    public class ExpenseTypeService : BaseService<ExpenseType>
    {
        public ExpenseTypeService(IRepository<ExpenseType> repository) : base(repository)
        {
        }
    }
}
