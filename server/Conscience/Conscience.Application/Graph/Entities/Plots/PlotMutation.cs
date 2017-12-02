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
    public class PlotMutation : ObjectGraphType<object>
    {
        public PlotMutation(PlotRepository plotRepo, CharacterRepository characterRepo)
        {
            Name = "PlotMutation";

            Field<PlotGraphType>("addOrModifyPlot",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<PlotInputGraphType>> { Name = "plot", Description = "new plot" }
                    ),
                resolve: context => {
                    var plot = context.GetArgument<Plot>("plot");
                    if (plot.Id == 0)
                    {
                        plot = plotRepo.Add(plot);
                    }
                    else
                    {
                        var dbPlot = plotRepo.GetById(plot.Id);
                        dbPlot.Name = plot.Name;
                        dbPlot.Description = plot.Description;

                        var charactersToRemove = dbPlot.Characters.Where(dbc => !plot.Characters.Any(c => dbc.Id == c.Id || dbc.Character.Id == c.Character.Id)).ToList();
                        foreach (var characterToRemove in charactersToRemove)
                        {
                            dbPlot.Characters.Remove(characterToRemove);
                        }

                        foreach(var characterInPlot in plot.Characters)
                        {
                            var dbCharacter = dbPlot.Characters.FirstOrDefault(c => c.Id == characterInPlot.Id || c.Character.Id == characterInPlot.Character.Id);
                            if (dbCharacter == null)
                            {
                                dbCharacter = new CharacterInPlot
                                {
                                    Character = characterRepo.GetById(characterInPlot.Character.Id)
                                };
                                dbPlot.Characters.Add(dbCharacter);
                            }

                            dbCharacter.Description = characterInPlot.Description;
                        }

                        plot = plotRepo.Modify(dbPlot);
                    }
                    return plot;
                })
                .AddQAPermissions();
        }
    }
}
