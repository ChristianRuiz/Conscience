using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Plots
{
    public class CharacterInPlotGraphType : ObjectGraphType<CharacterInPlot>
    {
        public CharacterInPlotGraphType()
        {
            Name = "CharacterInPlot";

            Field(c => c.Id);
            Field(c => c.Description);
            Field<PlotGraphType>("plot", resolve: context => context.Source.Plot);
        }
    }
}
