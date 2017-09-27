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
    public class EmployeeQuery : ObjectGraphType<object>
    {
        public EmployeeQuery(EmployeeRepository employeeRepo)
        {
            Name = "EmployeeQuery";

            Field<ListGraphType<EmployeeGraphType>>("all", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => employeeRepo.GetAllEmployees().ApplyPaginationAndOrderBy(context)
                .AvoidLazyLoad(context, e => e.Account, e => e.Notifications))
                .AddQAPermissions();

            Field<EmployeeGraphType>("byId",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "host id" }
                    ),
                resolve: context => employeeRepo.GetById(context.GetArgument<int>("id")))
                .AddBehaviourAndPlotPermissions();
        }
    }
}
