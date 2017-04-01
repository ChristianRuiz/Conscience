using Conscience.Application.Graph.Entities.Users;
using Conscience.Domain;
using GraphQL.Types;
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

            Field<StringGraphType>("department", resolve: c => {
                var role = c.Source.Account.Roles.FirstOrDefault(r => r.Name.StartsWith("Company"));
                if (role == null)
                    return string.Empty;
                return role.Name.Replace("Company", string.Empty);
            });
        }
    }
}
