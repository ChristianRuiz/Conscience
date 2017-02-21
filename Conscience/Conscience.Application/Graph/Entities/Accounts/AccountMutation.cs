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
    public class AccountMutation : ObjectGraphType<object>
    {
        public AccountMutation(AccountRepository accountRepo)
        {
            Name = "AccountMutation";

            Field<ListGraphType<AccountGraphType>>("register", 
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "email", Description = "email" },
                    new QueryArgument<StringGraphType> { Name = "password", Description = "password" }
                    ),
                resolve: context => { throw new NotImplementedException(); });
        }
    }
}
