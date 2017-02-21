using Conscience.Application.Graph.Entities.Accounts;
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

            Field<AccountQuery>("accounts", resolve: context => container.Resolve<AccountQuery>());
        }
    }
}
