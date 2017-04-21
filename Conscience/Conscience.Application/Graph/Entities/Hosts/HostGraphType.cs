using Conscience.Application.Graph.Entities.Accounts;
using Conscience.Application.Graph.Entities.Characters;
using Conscience.Application.Graph.Entities.Memories;
using Conscience.Application.Graph.Entities.Stats;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Hosts
{
    public class HostGraphType : ConscienceGraphType<Host>
    {
        public HostGraphType()
        {
            Name = "Host";

            Field<AccountGraphType>("account", resolve: context => context.Source.Account);
            Field<CharacterGraphType>("currentCharacter", resolve: context => context.Source.CurrentCharacter);
            Field<ListGraphType<CharacterGraphType>>("characters", resolve: context => context.Source.Characters);
            Field<ListGraphType<CoreMemoryGraphType>>("coreMemories", resolve: context => context.Source.CoreMemories);
            Field<ListGraphType<StatsGraphType>>("stats", resolve: context => context.Source.Stats);
        }
    }
}
