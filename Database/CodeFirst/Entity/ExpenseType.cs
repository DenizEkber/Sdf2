﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sdf2.Database.CodeFirst.Entity
{
    public class ExpenseType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Expense> Expenses { get; set; }
    }
}
