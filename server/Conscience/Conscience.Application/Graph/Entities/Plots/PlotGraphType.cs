using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Plots
{
    public class PlotGraphType : ConscienceGraphType<Plot>
    {
        public PlotGraphType()
        {
            Name = "Plot";
            
            Field(p => p.Name);
            Field(p => p.Description);
            Field<ListGraphType<PlotEventGraphType>>("events", resolve: context => context.Source.Events);
            Field<ListGraphType<CharacterInPlotGraphType>>("characters", resolve: context => context.Source.Characters);
        }
    }
}
