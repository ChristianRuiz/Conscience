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
    public class ConscienceMutation : ObjectGraphType<object>
    {
        public ConscienceMutation(IUnityContainer container)
        {
            Name = "Mutation";

            Field<AccountMutation>("accounts", resolve: context => container.Resolve<AccountMutation>());
        }
    }
}
