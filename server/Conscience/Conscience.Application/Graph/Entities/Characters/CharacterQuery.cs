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
                resolve: context => characterRepo.GetAll()
                                            .ApplyPaginationAndOrderBy(context)
                                            .ToList().Where(c => !accountService.CurrentUser.UserName.Contains("-") 
                                                                || !c.Hosts.Any() 
                                                                ||  c.Hosts.Any(h => h.Host.Account.UserName.StartsWith(accountService.CurrentUser.UserName.Split('-').First()))) //TODO: Remove this line, only to send both runs pre game 
                                            )
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
