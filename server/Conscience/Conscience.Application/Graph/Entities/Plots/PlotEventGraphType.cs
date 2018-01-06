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
    public class PlotEventGraphType : ConscienceGraphType<PlotEvent>
    {
        public PlotEventGraphType()
        {
            Name = "PlotEvent";
            
            Field(p => p.Description);
            Field(p => p.Location);
            Field(p => p.Hour);
            Field(p => p.Minute);
            Field<PlotGraphType>("plot", resolve: context => context.Source.Plot);
            //TODO: Remove after a new version has been approved on the store
            //Hack: Added to allow compatibility with the old versions published in the Apple Store
            Field<ListGraphType<CharacterGraphType>>("characters", resolve: context => new List<Character>());
        }
    }
}
