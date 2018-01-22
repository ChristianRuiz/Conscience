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

namespace Conscience.Application.Graph.Entities.Characters
{
    public class CharacterQuery : ObjectGraphType<object>
    {
        public CharacterQuery(CharacterRepository characterRepo, IUsersIdentityService accountService)
        {
            Name = "CharacterQuery";

            Field<ListGraphType<CharacterGraphType>>("all", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => characterRepo.GetAllCharacters(accountService.CurrentUser)
                                            .ApplyPaginationAndOrderBy(context)
                                            .ToList())
                                            .AddMaintenancePermissions();

            Field<CharacterGraphType>("byId",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "character id" }
                    ),
                resolve: context => characterRepo.GetById(context.GetArgument<int>("id")))
                .AddMaintenancePermissions();
        }
    }
}
