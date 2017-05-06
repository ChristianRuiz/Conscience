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
    public class CharacterGraphType : ConscienceGraphType<Character>
    {
        public CharacterGraphType()
        {
            Name = "Character";
            
            Field(c => c.Name);
            Field(c => c.Age);
            Field(c => c.Story);
            Field(c => c.NarrativeFunction);
            Field<GenderEnumeration>("gender", resolve: context => context.Source.Gender);
            Field<ListGraphType<MemoryGraphType>>("memories", resolve: context => context.Source.Memories);
            Field<ListGraphType<TriggerGraphType>>("triggers", resolve: context => context.Source.Triggers);
            Field<ListGraphType<CharacterInPlotGraphType>>("plots", resolve: context => context.Source.Plots);
            Field<ListGraphType<PlotEventGraphType>>("plotEvents", resolve: context => context.Source.PlotEvents);
            Field<ListGraphType<CharacterRelationGraphType>>("relations", resolve: context => context.Source.Relations);
        }
    }

    public class GenderEnumeration : EnumerationGraphType<Genders>
    {
        public GenderEnumeration()
        {
            Name = "Genders";
        }
    }
}
