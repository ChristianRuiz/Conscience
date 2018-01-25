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
    public class PlotMutation : ConscienceMutationBase
    {
        public PlotMutation(PlotRepository plotRepo, CharacterRepository characterRepo, LogEntryService logService,
             NotificationsService notificationsService, IUsersIdentityService usersService, EmployeeRepository employeeRepo)
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
                        plot = ModifyPlot(logService, plotRepo, characterRepo, employeeRepo, plot);
                        logService.Log($"Added a new plot with name '{plot.Name}'");

                        NotifyPlotModification(notificationsService, usersService, employeeRepo, plot);
                    }
                    else
                    {
                        logService.Log($"Edited a plot with name '{plot.Name}'");
                        plot = ModifyPlot(logService, plotRepo, characterRepo, employeeRepo, plot);

                        NotifyPlotModification(notificationsService, usersService, employeeRepo, plot);
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

                    NotifyPlotModification(notificationsService, usersService, employeeRepo, plot);
                    return id;
                })
                .AddPlotEditorPermissions();
        }

        private static void NotifyPlotModification(NotificationsService notificationsService, IUsersIdentityService usersService, EmployeeRepository employeeRepo, Plot plot)
        {
            var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);
            foreach (var character in plot.Characters.Where(c => c.Character.CurrentHost != null))
            {
                var host = character.Character.CurrentHost.Host;
                notificationsService.Notify(host.Account.Id, $"{employee.Name} has modified the plot {plot.Name}.",
                    NotificationTypes.PlotModified, host: host, employee: employee);
            }
        }

        private Plot ModifyPlot(LogEntryService logService, PlotRepository plotRepo, CharacterRepository characterRepo, EmployeeRepository employeeRepo, Plot plot)
        {
            var dbPlot = plotRepo.GetById(plot.Id);

            dbPlot.Name = plot.Name;
            dbPlot.Description = plot.Description;
            dbPlot.Writer = employeeRepo.GetById(plot.Writer.Id);

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
