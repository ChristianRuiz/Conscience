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
        public HostMutation(HostRepository hostRepo, CharacterRepository characterRepo, LogEntryService logService)
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
                    return host;
                    })
                .AddBehaviourAndPlotPermissions();

            Field<HostGraphType>("assignHost",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "hostId", Description = "host id" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "characterId", Description = "character id" }
                    ),
                resolve: context => {
                    var hostId = context.GetArgument<int>("hostId");
                    var characterId = context.GetArgument<int>("characterId");
                    var host = hostRepo.GetAll().First(h => h.Id == hostId);
                    var character = characterRepo.GetAll().First(h => h.Id == characterId);

                    
                    logService.Log(host, $"Assign character '{host.Account.UserName}' to host '{character.Name}'");
                    
                    host = hostRepo.AssignHost(host, character);
                    return host;
                })
                .AddPlotAssignHostPermissions();
        }
    }
}
