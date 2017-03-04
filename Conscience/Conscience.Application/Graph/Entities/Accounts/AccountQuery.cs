using Concience.DataAccess.Repositories;
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

            Field<ListGraphType<AccountGraphType>>("getAll", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => accountRepo.GetAll().ApplyPaginationAndOrderBy(context))
                .AddAdminPermissions();

            Field<ListGraphType<AccountGraphType>>("getAllEmployees",
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => accountRepo.GetAll().Where(a => a.User is Employee).ApplyPaginationAndOrderBy(context))
                .AddQAPermissions();

            Field<ListGraphType<AccountGraphType>>("getAllHosts",
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => accountRepo.GetAll().Where(a => a.User is Host).ApplyPaginationAndOrderBy(context))
                .AddBehaviourAndPlotPermissions();

            Field<AccountGraphType>("getCurrent",
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => accountService.CurrentUser);
        }
    }
}
