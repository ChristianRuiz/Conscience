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
    public class CharacterInPlotInputGraphType : ConscienceInputGraphType
    {
        public CharacterInPlotInputGraphType()
        {
            Name = "CharacterInPlotInput";

            Field<StringGraphType>("description");
            
            Field<PlotInputGraphType>("plot");
            Field<CharacterInPlotInputGraphType>("character");
        }
    }
}
