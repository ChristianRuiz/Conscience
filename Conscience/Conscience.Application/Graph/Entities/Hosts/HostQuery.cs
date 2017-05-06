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

namespace Conscience.Application.Graph.Entities.Hosts
{
    public class HostQuery : ObjectGraphType<object>
    {
        public HostQuery(HostRepository hostRepo, IUsersIdentityService accountService)
        {
            Name = "HostQuery";

            Field<ListGraphType<HostGraphType>>("all", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => hostRepo.GetAllHosts(accountService.CurrentUser).ApplyPaginationAndOrderBy(context)
                .AvoidLazyLoad(context, h => h.Account, h => h.Notifications, h => h.Characters, h => h.CoreMemory1, h => h.CoreMemory2, h => h.CoreMemory3, h => h.CurrentCharacter, h => h.Account, h => h.Account.Device))
                .AddBehaviourAndPlotPermissions();

            Field<HostGraphType>("byId",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "host id" }
                    ),
                resolve: context => hostRepo.GetById(accountService.CurrentUser, context.GetArgument<int>("id")))
                .AddBehaviourAndPlotPermissions();
        }
    }
}
