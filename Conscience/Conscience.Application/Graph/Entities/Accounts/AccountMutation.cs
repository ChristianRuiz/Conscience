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
    public class AccountMutation : ObjectGraphType<object>
    {
        public AccountMutation(IUsersIdentityService accountService)
        {
            Name = "AccountMutation";

            Field<AccountGraphType>("register", 
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "userName", Description = "user name" },
                    new QueryArgument<StringGraphType> { Name = "email", Description = "email" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password", Description = "password" }
                    ),
                resolve: context => accountService.RegisterAsync(context.GetArgument<string>("userName"), context.GetArgument<string>("email"), context.GetArgument<string>("password")))
                .AddPermission(RoleTypes.Anonymous);

            Field<AccountGraphType>("login",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "userName", Description = "user name" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password", Description = "password" }
                    ),
                resolve: context => accountService.LoginAsync(context.GetArgument<string>("userName"), context.GetArgument<string>("password")))
                .AddPermission(RoleTypes.Anonymous);

            Field<AccountGraphType>("logout",
                resolve: context => { accountService.LogoffAsync(); return null; });
        }
    }
}
