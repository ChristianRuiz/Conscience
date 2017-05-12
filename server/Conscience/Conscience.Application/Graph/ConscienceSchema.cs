using GraphQL.Types;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public class ConscienceSchema : Schema
    {
        public ConscienceSchema(IUnityContainer container)
            : base(type => (GraphType)container.Resolve(type))
        {
            Query = container.Resolve<ConscienceQuery>();
            Mutation = container.Resolve<ConscienceMutation>();
        }
    }
}
