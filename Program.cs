using Sdf2.Core.Interface;
using Sdf2.Core.Service;
using Sdf2.Database.CodeFirst.Context;
using Sdf2.Database.CodeFirst.Entity;
using Sdf2.Database.Interface;
using Sdf2.Database.Repository;
using Sdf2.UI;

namespace Sdf2
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            var dbContext = new AppDbContext();


            IRepository<User> userRepository = new Repository<User>(dbContext);
            IRepository<ExpenseType> expenseTypeRepository = new Repository<ExpenseType>(dbContext);
            IExpenseRepository expenseRepository = new ExpenseRepository(dbContext);


            IExpenseService expenseService = new ExpenseService(expenseRepository);
            ExpenseTypeService expenseTypeService = new ExpenseTypeService(expenseTypeRepository);


            ApplicationConfiguration.Initialize();


            Application.Run(new LoginForm(userRepository, expenseService, expenseTypeService));
            //Application.Run(new MainForm(expenseService, expenseTypeService));

        }
    }
}
