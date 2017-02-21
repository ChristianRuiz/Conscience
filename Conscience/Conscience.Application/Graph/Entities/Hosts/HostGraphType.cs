using Conscience.Application.Graph.Entities.Characters;
using Conscience.Application.Graph.Entities.Users;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Hosts
{
    public class HostGraphType : UserGraphType<Host>
    {
        public HostGraphType()
        {
            Name = "Host";

            Field<CharacterGraphType>("currentCharacter", resolve: context => context.Source.CurrentCharacter);
            Field<ListGraphType<CharacterGraphType>>("characters", resolve: context => context.Source.Characters);
        }
    }
}
