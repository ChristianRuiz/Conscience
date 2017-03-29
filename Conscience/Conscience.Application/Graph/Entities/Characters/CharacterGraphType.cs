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
    public class CharacterGraphType : ObjectGraphType<Character>
    {
        public CharacterGraphType()
        {
            Name = "Character";

            Field(c => c.Id);
            Field(c => c.Description);
            Field<ListGraphType<MemoryGraphType<Memory>>>("memories", resolve: context => context.Source.Memories);
            Field<ListGraphType<TriggerGraphType>>("triggers", resolve: context => context.Source.Triggers);
            Field<ListGraphType<CharacterInPlotGraphType>>("plots", resolve: context => context.Source.Plots);
            Field<ListGraphType<CharacterInPlotGraphType>>("plotEvents", resolve: context => context.Source.PlotEvents);
        }
    }
}
