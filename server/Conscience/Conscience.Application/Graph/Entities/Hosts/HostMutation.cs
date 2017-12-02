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
        public HostMutation(HostRepository hostRepo)
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
                    var host = hostRepo.ModifyStats(hostId, stats);
                    return host;
                    })
                .AddQAPermissions();
        }
    }
}
