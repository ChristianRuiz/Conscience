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
        public HostQuery(HostRepository hostRepo, IUsersIdentityService accountService, LogEntryService logService, LogEntryRepository logRepo)
        {
            Name = "HostQuery";

            Field<ListGraphType<HostGraphType>>("all", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => hostRepo.GetAllHosts(accountService.CurrentUser)
                                            .ApplyPaginationAndOrderBy(context)
                .AvoidLazyLoad(context, h => h.Account, h => h.Notifications, h => h.Characters, h => h.CoreMemory1, h => h.CoreMemory2, h => h.CoreMemory3, h => h.Account, h => h.Account.Device)
                .ToList().Where(h => !accountService.CurrentUser.UserName.Contains("-") || h.Account.UserName.StartsWith(accountService.CurrentUser.UserName.Split('-').First())) //TODO: Remove this line, only to send both runs pre game 
                )
                .AddMaintenancePermissions();

            Field<HostGraphType>("byId",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "host id" }
                    ),
                resolve: context => hostRepo.GetById(accountService.CurrentUser, context.GetArgument<int>("id")))
                .AddMaintenancePermissions();

            Field<BooleanGraphType>("hasFailure",
                arguments: new QueryArguments
                {
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" }
                },
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var host = hostRepo.GetAll().First(h => h.Id == hostId);

                    var lastFailure = logRepo.GetAll().OrderByDescending(l => l.Id).FirstOrDefault(l => l.Description.Contains("has a critical failure"));
                    var lastFix = logRepo.GetAll().OrderByDescending(l => l.Id).FirstOrDefault(l => l.Description.Contains("fixed by employee"));

                    if ((lastFailure == null || (DateTime.Now - lastFailure.TimeStamp > TimeSpan.FromHours(1) && lastFailure.Host.Id != hostId))
                            && (lastFix != null && DateTime.Now - lastFix.TimeStamp < TimeSpan.FromHours(1)))
                    {
                        logService.Log(host, $"Host '{host.Account.UserName}' has a critical failure");
                        return true;
                    }

                    return false;
                })
                .AddMaintenancePermissions();
        }
    }
}
