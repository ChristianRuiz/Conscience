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

namespace Conscience.Application.Graph.Entities.LogEntries
{
    public class LogEntryQuery : ObjectGraphType<object>
    {
        public LogEntryQuery(LogEntryRepository logRepo, IUsersIdentityService userService)
        {
            Name = "LogEntryQuery";

            Field<ListGraphType<LogEntryGraphType>>("byEmployee", 
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "employee id" },
                    new QueryArgument<IntGraphType> { Name = "first", Description = "number of items to take" },
                    new QueryArgument<IntGraphType> { Name = "offset", Description = "offset of the first item to return" }
                    ),
                resolve: context => logRepo.GetByEmployee(context.GetArgument<int>("id"), userService.CurrentUser.Employee).OrderByDescending(l => l.Id)
                                        .ApplyPagination(context)
                )
                .AddQAPermissions();

            Field<ListGraphType<LogEntryGraphType>>("byHost",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "host id" },
                    new QueryArgument<IntGraphType> { Name = "first", Description = "number of items to take" },
                    new QueryArgument<IntGraphType> { Name = "offset", Description = "offset of the first item to return" }
                    ),
                resolve: context => logRepo.GetByHost(context.GetArgument<int>("id")).OrderByDescending(l => l.Id)
                                        .ApplyPagination(context)
                )
                .AddMaintenancePermissions();
        }
    }
}
