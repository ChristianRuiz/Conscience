using Concience.DataAccess.Repositories;
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
        public AccountQuery(AccountRepository accountRepo)
        {
            Name = "AccountQuery";

            Field<ListGraphType<AccountGraphType>>("getAll", 
                arguments: ConscienceArguments.PaginationsAndSortingArgument,
                resolve: context => accountRepo.GetAll().ApplyPaginationAndOrderBy(context));
        }
    }
}
