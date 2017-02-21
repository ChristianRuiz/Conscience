using Conscience.Application.Graph.Entities.Users;
using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Employees
{
    public class EmployeeGraphType : UserGraphType<Employee>
    {
        public EmployeeGraphType()
        {
            Name = "Employee";
        }
    }
}
