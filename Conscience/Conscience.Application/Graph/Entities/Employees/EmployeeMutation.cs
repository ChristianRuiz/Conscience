using Conscience.DataAccess.Repositories;
using Conscience.Application.Services;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Employees
{
    public class EmployeeMutation : ObjectGraphType<object>
    {
        public EmployeeMutation(UserRepository userRepo)
        {
            Name = "EmployeeMutation";

            Field<EmployeeGraphType>("addEmployee", 
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "accountId", Description = "account id" }
                    ),
                resolve: context => userRepo.AddEmployee(context.GetArgument<int>("accountId")))
                .AddQAPermissions();
        }
    }
}
