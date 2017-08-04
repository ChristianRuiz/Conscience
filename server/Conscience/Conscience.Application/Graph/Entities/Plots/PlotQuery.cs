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
    public class PlotQuery : ObjectGraphType<object>
    {
        public PlotQuery(PlotRepository plotRepo)
        {
            Name = "PlotQuery";

            Field<ListGraphType<PlotGraphType>>("all", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => plotRepo.GetAll().ApplyPaginationAndOrderBy(context))
                .AddQAPermissions();

            Field<PlotGraphType>("byId",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "plot id" }
                    ),
                resolve: context => plotRepo.GetById(context.GetArgument<int>("id")));
        }
    }
}
