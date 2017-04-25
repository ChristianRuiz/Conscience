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
                .AvoidLazyLoad(context, h => h.Account, h => h.Notifications, h => h.Characters, h => h.CoreMemories, h => h.CurrentCharacter, h => h.Account, h => h.Account.Device))
                .AddBehaviourAndPlotPermissions();
        }
    }
}
