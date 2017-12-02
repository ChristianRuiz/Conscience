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
    public class CharacterInputGraphType : ConscienceInputGraphType
    {
        public CharacterInputGraphType() : base()
        {
            Name = "CharacterInput";

            Field<StringGraphType>("name");
            Field<IntGraphType>("age");
            Field<StringGraphType>("story");
            Field<StringGraphType>("narrativeFunction");
            Field<GenderEnumeration>("gender");

            Field<ListGraphType<MemoryInputGraphType>>("memories");
            Field<ListGraphType<TriggerInputGraphType>>("triggers");
            Field<ListGraphType<CharacterInPlotInputGraphType>>("plots");

            Field<ListGraphType<PlotEventInputGraphType>>("plotEvents");
            Field<ListGraphType<CharacterRelationInputGraphType>>("relations");
        }
    }
}
