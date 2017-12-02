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
    public class ConscienceQuery : ObjectGraphType<object>
    {
        public ConscienceQuery(IUnityContainer container)
        {
            Name = "Query";

            Field<AccountQuery>("accounts", resolve: context => container.Resolve<AccountQuery>()).RequiresMembership();
            Field<EmployeeQuery>("employees", resolve: context => container.Resolve<EmployeeQuery>()).RequiresMembership();
            Field<HostQuery>("hosts", resolve: context => container.Resolve<HostQuery>()).RequiresMembership();
            Field<PlotQuery>("plots", resolve: context => container.Resolve<PlotQuery>()).RequiresMembership();
            Field<CharacterQuery>("characters", resolve: context => container.Resolve<CharacterQuery>()).RequiresMembership();
        }
    }
}
