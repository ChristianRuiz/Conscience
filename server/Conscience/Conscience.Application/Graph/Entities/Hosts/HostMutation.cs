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
using Conscience.Application.Graph.Entities.Stats;

namespace Conscience.Application.Graph.Entities.Hosts
{
    public class HostMutation : ObjectGraphType<object>
    {
        public HostMutation(HostRepository hostRepo, CharacterRepository characterRepo, EmployeeRepository employeeRepo, 
            LogEntryService logService, NotificationsService notificationsService, IUsersIdentityService usersService,
            AccountUpdatesService hostUpdatesService)
        {
            Name = "HostMutation";

            Field<HostGraphType>("modifyStats", 
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" },
                    new QueryArgument<ListGraphType<StatsInputGraphType>> { Name = "stats", Description = "stats" }
                    ),
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var stats = context.GetArgument<List<Domain.Stats>>("stats");
                    var host = hostRepo.GetAll().First(h => h.Id == hostId);

                    var modifiedStats = stats.Where(s => host.Stats.Any(hs => s.Name.ToLowerInvariant() == hs.Name.ToLowerInvariant() && s.Value != hs.Value));
                    foreach(var modifiedStat in modifiedStats)
                    {
                        var originalStat = host.Stats.First(s => s.Name.ToLowerInvariant() == modifiedStat.Name.ToLowerInvariant());
                        logService.Log(host, $"Modified stat '{modifiedStat.Name}' from '{originalStat.Value}' to '{modifiedStat.Value}'");
                    }

                    host = hostRepo.ModifyStats(hostId, stats);

                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);

                    notificationsService.Notify(host.Account.Id, $"{employee.Name} has modified your stats.",
                        NotificationTypes.StatsModified, host: host, employee: employee);

                    return host;
                    })
                .AddBehaviourAndPlotPermissions();

            Field<HostGraphType>("assignCharacter",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "characterId", Description = "character id" }
                    ),
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var characterId = context.GetArgument<int>("characterId");
                    var host = hostRepo.GetAll().First(h => h.Id == hostId);
                    var character = characterRepo.GetAll().First(h => h.Id == characterId);
                    
                    logService.Log(host, $"Assign character '{character.Name}' to host '{host.Account.UserName}'");

                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);

                    notificationsService.Notify(host.Account.Id, $"{employee.Name} has assigned you a new character '{character.Name}'.",
                        NotificationTypes.CharacterAssigned, host: host, employee: employee);

                    host = hostRepo.AssignHost(host, character);
                    return host;
                })
                .AddPlotAssignHostPermissions();

            Field<HostGraphType>("call",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" }
                    ),
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var host = hostRepo.GetAll().First(h => h.Id == hostId);
                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);

                    logService.Log(host, $"Called host '{host.Account.UserName}' by employee '{usersService.CurrentUser.Employee.Name}'");
                    notificationsService.Notify(host.Account.Id, $"You are being called by {employee.Name} from {employee.Account.Roles.FirstOrDefault().Name.Replace("Company", "")}", 
                        NotificationTypes.CallHost, host: host, employee: employee);

                    return host;
                })
                .AddMaintenancePermissions();

            Field<HostGraphType>("reset",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" }
                    ),
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var host = hostRepo.GetAll().First(h => h.Id == hostId);
                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);

                    logService.Log(host, $"Reset host '{host.Account.UserName}' by employee '{usersService.CurrentUser.Employee.Name}'");
                    if (host.CoreMemory1.Locked)
                        notificationsService.Notify(host.Account.Id, "Reset", NotificationTypes.Reset, host: host, employee: employee);
                    else
                        notificationsService.Notify(host.Account.Id, "They are trying to reset you", NotificationTypes.NoReset, host: host, employee: employee);

                    return host;
                })
                .AddMaintenancePermissions();

            Field<HostGraphType>("unlockCoreMemory",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "coreMemoryId", Description = "core memory id" }
                    ),
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var memoryId = context.GetArgument<int>("coreMemoryId");

                    var host = hostRepo.GetAll().First(h => h.Id == hostId);
                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);

                    var memory = host.CoreMemory1;

                    if (memoryId == 2)
                        memory = host.CoreMemory2;
                    else if (memoryId == 3)
                        memory = host.CoreMemory3;

                    memory.Locked = false;

                    notificationsService.Notify(host.Account.Id, $"Core memory {memoryId} unlocked", NotificationTypes.CoreMemory, host: host, employee: employee, audio: memory.Audio);
                    
                    return host;
                })
                .AddMaintenancePermissions();


            Field<HostGraphType>("changeStatus",
                arguments: new QueryArguments
                {
                    new QueryArgument<HostStatusEnumeration>() { Name = "status", Description = "host status" }
                },
                resolve: context => {
                    var status = context.GetArgument<HostStatus>("status");

                    var host = hostRepo.GetAll().First(h => h.Id == usersService.CurrentUser.Host.Id);
                    var currentStatus = host.Status;
                    host = hostRepo.ChangeStatus(usersService.CurrentUser.Host.Id, status);

                    logService.Log(host, $"Host '{host.Account.UserName}' status changed to '{status.ToString()}'");

                    if (currentStatus == HostStatus.Ok && status == HostStatus.Hurt)
                        notificationsService.Notify(RoleTypes.CompanyQA, $"Host '{host.CurrentCharacter.Character.Name}' hurt", NotificationTypes.HostHurt, host);
                    else if (status == HostStatus.Dead)
                        notificationsService.Notify(RoleTypes.CompanyQA, $"Host '{host.CurrentCharacter.Character.Name}' dead", NotificationTypes.HostDead, host);

                    hostUpdatesService.BroadcastAccountUpdated(usersService.CurrentUser.Host.Id);

                    return host;
                })
                .AllowCurrentUser();
            
            Field<HostGraphType>("fixed",
                arguments: new QueryArguments
                {
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" }
                },
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var host = hostRepo.GetAll().First(h => h.Id == hostId);
                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);

                    host = hostRepo.ChangeStatus(host.Id, HostStatus.Ok);

                    logService.Log(host, $"Host '{host.Account.UserName}' fixed by employee '{employee.Name}'");

                    notificationsService.Notify(host.Account.Id, "You have been fixed", NotificationTypes.HostFixed, host: host, employee: employee);
                    
                    hostUpdatesService.BroadcastAccountUpdated(host.Id);

                    return host;
                })
                .AddMaintenancePermissions();
        }
    }
}
