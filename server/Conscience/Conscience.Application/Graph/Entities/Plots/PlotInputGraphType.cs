using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Plots
{
    public class PlotInputGraphType : ConscienceInputGraphType
    {
        public PlotInputGraphType()
        {
            Name = "PlotInput";

            Field<StringGraphType>("name");
            Field<StringGraphType>("description");
            
            Field<ListGraphType<PlotEventInputGraphType>>("events");
            Field<ListGraphType<CharacterInPlotInputGraphType>>("characters");

            Field<EmployeesInputGraphType>("writer");
        }
    }
}
