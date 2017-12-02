using Conscience.Application.Graph.Entities.Accounts;
using Conscience.Application.Graph.Entities.Characters;
using Conscience.Application.Graph.Entities.Employees;
using Conscience.Application.Graph.Entities.Hosts;
using Conscience.Application.Graph.Entities.Plots;
using GraphQL.Types;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public class ConscienceMutation : ObjectGraphType<object>
    {
        public ConscienceMutation(IUnityContainer container)
        {
            Name = "Mutation";

            Field<AccountMutation>("accounts", resolve: context => container.Resolve<AccountMutation>());

            Field<EmployeeMutation>("employees", resolve: context => container.Resolve<EmployeeMutation>()).RequiresMembership();

            Field<HostMutation>("hosts", resolve: context => container.Resolve<HostMutation>()).RequiresMembership();

            Field<PlotMutation>("plots", resolve: context => container.Resolve<PlotMutation>()).RequiresMembership();

            Field<CharacterMutation>("characters", resolve: context => container.Resolve<CharacterMutation>()).RequiresMembership();
        }
    }
}
