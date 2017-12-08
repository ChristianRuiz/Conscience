using Conscience.DataAccess.Repositories;
using Conscience.Application.Services;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Plots
{
    public class PlotMutation : ConscienceMutation
    {
        public PlotMutation(PlotRepository plotRepo, CharacterRepository characterRepo)
        {
            Name = "PlotMutation";

            Field<PlotGraphType>("addOrModifyPlot",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PlotInputGraphType>> { Name = "plot", Description = "new or edited plot" }
                    ),
                resolve: context => {
                    var plot = context.GetArgument<Plot>("plot");
                    if (plot.Id == 0)
                    {
                        var dbPlot = plotRepo.Add(new Plot());
                        plot.Id = dbPlot.Id;
                        plot = ModifyPlot(plotRepo, characterRepo, plot);
                    }
                    else
                    {
                        plot = ModifyPlot(plotRepo, characterRepo, plot);
                    }
                    return plot;
                })
                .AddQAPermissions();
        }

        private Plot ModifyPlot(PlotRepository plotRepo, CharacterRepository characterRepo, Plot plot)
        {
            var dbPlot = plotRepo.GetById(plot.Id);

            dbPlot.Name = plot.Name;
            dbPlot.Description = plot.Description;

            ModifyCollection(plotRepo, plot.Characters, dbPlot.Characters,
                (character, dbCharacter) =>
                {
                    dbCharacter.Description = character.Description;
                },
                (c, dbc) => dbc.Character.Id == c.Character.Id);
            
            plot = plotRepo.Modify(dbPlot);
            return plot;
        }
    }
}
