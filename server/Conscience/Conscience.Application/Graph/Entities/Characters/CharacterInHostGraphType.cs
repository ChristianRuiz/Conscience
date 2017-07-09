using Conscience.Application.Graph.Entities.Hosts;
using Conscience.Application.Graph.Entities.Memories;
using Conscience.Application.Graph.Entities.Plots;
using Conscience.Application.Graph.Entities.Triggers;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Characters
{
    public class CharacterInHostGraphType : ConscienceGraphType<CharacterInHost>
    {
        public CharacterInHostGraphType()
        {
            Name = "CharacterInHost";
            
            Field(c => c.AssignedOn);
            Field<HostGraphType>("host", resolve: context => context.Source.Host);
            Field<CharacterGraphType>("character", resolve: context => context.Source.Character);
        }
    }
}
