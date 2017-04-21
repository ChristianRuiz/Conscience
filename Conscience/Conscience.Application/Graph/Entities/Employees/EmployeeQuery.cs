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
        public EmployeeQuery(EmployeeRepository userRepo)
        {
            Name = "EmployeeQuery";

            Field<ListGraphType<EmployeeGraphType>>("getAll", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => userRepo.GetAllEmployees().ApplyPaginationAndOrderBy(context)
                .AvoidLazyLoad(context, e => e.Account, e => e.Notifications))
                .AddQAPermissions();
        }
    }
}
