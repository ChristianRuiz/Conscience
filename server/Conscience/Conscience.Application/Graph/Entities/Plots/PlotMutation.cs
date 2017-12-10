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
        public PlotMutation(PlotRepository plotRepo, CharacterRepository characterRepo, LogEntryService logService)
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
                        plot = ModifyPlot(logService, plotRepo, characterRepo, plot);
                        logService.Log($"Added a new plot with name '{plot.Name}'");
                    }
                    else
                    {
                        logService.Log($"Edited a plot with name '{plot.Name}'");
                        plot = ModifyPlot(logService, plotRepo, characterRepo, plot);
                    }
                    return plot;
                })
                .AddPlotEditorPermissions();

            Field<IntGraphType>("deletePlot",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the plot to delete" }
                    ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var plot = plotRepo.GetById(id);
                    logService.Log($"Deleted a plot with name '{plot.Name}'");
                    plotRepo.Delete(plot);
                    return id;
                })
                .AddPlotEditorPermissions();
        }

        private Plot ModifyPlot(LogEntryService logService, PlotRepository plotRepo, CharacterRepository characterRepo, Plot plot)
        {
            var dbPlot = plotRepo.GetById(plot.Id);

            dbPlot.Name = plot.Name;
            dbPlot.Description = plot.Description;

            ModifyCollection(plotRepo, logService, plot.Characters, dbPlot.Characters,
                (character, dbCharacter) =>
                {
                    dbCharacter.Description = character.Description;
                    dbCharacter.Character = characterRepo.GetById(character.Character.Id);
                },
                (c, dbc) => dbc.Character.Id == c.Character.Id);

            ModifyCollection(plotRepo, logService, plot.Events, dbPlot.Events,
                (e, dbe) =>
                {
                    dbe.Description = e.Description;
                    dbe.Location = e.Location;
                    dbe.Hour = e.Hour;
                    dbe.Minute = e.Minute;
                });

            plot = plotRepo.Modify(dbPlot);
            return plot;
        }
    }
}
