using Conscience.Application.Graph.Entities.Characters;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Plots
{
    public class PlotEventInputGraphType : ConscienceInputGraphType
    {
        public PlotEventInputGraphType()
        {
            Name = "PlotEventInput";

            Field<StringGraphType>("description");
            Field<StringGraphType>("location");
            Field<IntGraphType>("hour");
            Field<IntGraphType>("minute");

            Field<PlotInputGraphType>("plot");
            Field<ListGraphType<CharacterInputGraphType>>("characters");
        }
    }
}
