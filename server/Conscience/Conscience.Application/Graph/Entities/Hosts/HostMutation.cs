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
using Conscience.DataAccess;

namespace Conscience.Application.Graph.Entities.Hosts
{
    public class HostMutation : ObjectGraphType<object>
    {
        public HostMutation(HostRepository hostRepo, CharacterRepository characterRepo, EmployeeRepository employeeRepo, 
            AccountRepository accountRepo, LogEntryService logService, NotificationsService notificationsService,
            IUsersIdentityService usersService, AccountUpdatesService hostUpdatesService)
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
                .AddMaintenancePermissions();

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
                    var currentHost = character.CurrentHost;
                    var currentCharacter = host.CurrentCharacter;

                    logService.Log(host, $"Assign character '{character.Name}' to host '{host.Account.UserName}'");

                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);
                    
                    host = hostRepo.AssignHost(host, character);

                    notificationsService.Notify(host.Account.Id, $"{employee.Name} has assigned you a new character '{character.Name}'.",
                        NotificationTypes.CharacterAssigned, host: host, employee: employee);

                    if (currentHost != null)
                        notificationsService.Notify(currentHost.Host.Account.Id, $"{employee.Name} has unassigned your character '{character.Name}'.",
                            NotificationTypes.CharacterAssigned, host: currentHost.Host, employee: employee);

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
                    else if (host.Account.Employee != null)
                        notificationsService.Notify(host.Account.Id, "Reset", NotificationTypes.ResetHuman, host: host, employee: employee);
                    else
                        notificationsService.Notify(host.Account.Id, "They are trying to reset you", NotificationTypes.NoReset, host: host, employee: employee);

                    return host;
                })
                .AddMaintenancePermissions();

            Field<ListGraphType<HostGraphType>>("resetAll",
                arguments: new QueryArguments(
                    ),
                resolve: context => {
                    var hosts = hostRepo.GetAll().Where(h => !h.Hidden).ToList();
                    var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);

                    foreach (var host in hosts)
                    {
                        try
                        {
                            logService.Log(host, $"Reset host '{host.Account.UserName}' by employee '{employee.Name}'");
                            if (host.CoreMemory1.Locked)
                                notificationsService.Notify(host.Account.Id, "Reset", NotificationTypes.Reset, host: host, employee: employee);
                            else if (host.Account.Employee != null)
                                notificationsService.Notify(host.Account.Id, "Reset", NotificationTypes.ResetHuman, host: host, employee: employee);
                            else
                                notificationsService.Notify(host.Account.Id, "They are trying to reset you", NotificationTypes.NoReset, host: host, employee: employee);
                        }
                        catch { }
                    }

                    return hosts;
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

                    if (memoryId == 3)
                    {
                        notificationsService.Notify(host.Account.Id, $"You are completly unlocked, you will start remembering the events of the last few days.", NotificationTypes.CoreMemory, host: host, employee: employee);
                    }
                    
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

            Field<HostGraphType>("changeStatusToHost",
                arguments: new QueryArguments
                {
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" },
                    new QueryArgument<HostStatusEnumeration>() { Name = "status", Description = "host status" }
                },
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var status = context.GetArgument<HostStatus>("status");

                    var host = hostRepo.GetAll().First(h => h.Id == hostId);
                    var currentStatus = host.Status;
                    host = hostRepo.ChangeStatus(hostId, status);

                    logService.Log(host, $"Host '{host.Account.UserName}' status changed to '{status.ToString()}'");

                    if (currentStatus == HostStatus.Ok && status == HostStatus.Hurt)
                        notificationsService.Notify(RoleTypes.CompanyQA, $"Host '{host.CurrentCharacter.Character.Name}' hurt", NotificationTypes.HostHurt, host);
                    else if (status == HostStatus.Dead)
                        notificationsService.Notify(RoleTypes.CompanyQA, $"Host '{host.CurrentCharacter.Character.Name}' dead", NotificationTypes.HostDead, host);

                    hostUpdatesService.BroadcastAccountUpdated(hostId);

                    return host;
                })
                .AddMaintenancePermissions();

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


            Field<HostGraphType>("addHost",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "accountName", Description = "Account name" }
                   ),
               resolve: context => {
                   var accountName = context.GetArgument<string>("accountName");

                   var account = accountRepo.Add(new ConscienceAccount
                   {
                       UserName = accountName,
                       PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                   });

                   accountRepo.AddRole(account.Id, RoleTypes.Host);

                   var host = new Host { Account = account };

                   host = hostRepo.Add(host);

                   foreach (var statName in Enum.GetNames(typeof(StatNames)))
                       host.Stats.Add(new Domain.Stats { Name = statName, Value = 10 });

                   hostRepo.Modify(host);
                   
                   host = hostRepo.GetAll().First(h => h.Id == host.Id);
                   var randomLat = new Random().Next(100) / 100000f;
                   var randomLon = new Random().Next(100) / 100000f;

                   host.Account.Device = new Device
                   {
                       DeviceId = "Dummy",
                       BatteryLevel = 1,
                       BatteryStatus = Domain.Enums.BatteryStatus.NotCharging,
                       LastConnection = DateTime.Now
                   };

                   hostRepo.Modify(host);

                   host.Account.Device.Locations = new List<Location>
                {
                    new Location
                    {
                        Latitude = 37.048601 + randomLat,
                        Longitude = -2.4248117 + randomLon,
                        TimeStamp = DateTime.Now - TimeSpan.FromDays(1)
                    }
                };
                   hostRepo.Modify(host);

                   host.Account.Device.CurrentLocation = host.Account.Device.Locations.Last();
                   hostRepo.Modify(host);

                   host = hostRepo.GetAll().First(h => h.Id == host.Id);

                   return host;
               })
               .AddMaintenancePermissions();
        }
    }
}
