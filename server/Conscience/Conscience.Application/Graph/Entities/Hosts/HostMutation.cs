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
    public class HostMutation : ObjectGraphType<object>
    {
        public HostMutation(HostRepository hostRepo)
        {
            Name = "HostMutation";

            Field<HostGraphType>("addHost", 
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "accountId", Description = "account id" }
                    ),
                resolve: context => hostRepo.AddHost(context.GetArgument<int>("accountId")))
                .AddQAPermissions();
        }
    }
}
