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

namespace Conscience.Application.Graph.Entities.Accounts
{
    public class AccountQuery : ObjectGraphType<object>
    {
        public AccountQuery(AccountRepository accountRepo, IUsersIdentityService accountService)
        {
            Name = "AccountQuery";

            Field<ListGraphType<AccountGraphType>>("all", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => accountRepo.GetAll().ApplyPaginationAndOrderBy(context)
                .AvoidLazyLoad(context, a => a.Host, a => a.Employee, a => a.Device))
                .AddAdminPermissions();
            
            Field<AccountGraphType>("current",
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => accountService.CurrentUser).CurrentUserQuery();

            Field<AccountGraphType>("byId",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "account id" }
                    ),
                resolve: context => accountRepo.GetById(context.GetArgument<int>("id")));
        }
    }
}
