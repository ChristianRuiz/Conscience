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
    public class AccountMutation : ObjectGraphType<object>
    {
        public AccountMutation(IUsersIdentityService accountService, AccountRepository accountRepo)
        {
            Name = "AccountMutation";
            
            Field<AccountGraphType>("addAccount", 
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "userName", Description = "user name" },
                    new QueryArgument<StringGraphType> { Name = "email", Description = "email" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password", Description = "password" }
                    ),
                resolve: context => accountService.RegisterAsync(context.GetArgument<string>("userName"), context.GetArgument<string>("email"), context.GetArgument<string>("password")))
                .AddAdminPermissions().RequiresMembership();

            Field<AccountGraphType>("addRole",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "accountId", Description = "account id" },
                    new QueryArgument<NonNullGraphType<RoleEnumeration>> { Name = "role", Description = "role" }
                    ),
                resolve: context => accountRepo.AddRole(context.GetArgument<int>("accountId"), context.GetArgument<RoleTypes>("role")))
                .AddAdminPermissions().RequiresMembership();

            Field<AccountGraphType>("changePassword",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "accountId", Description = "account id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password", Description = "password" }
                    ),
                resolve: context =>
                {
                    accountService.ChangePasswordAsync(context.GetArgument<int>("accountId"), context.GetArgument<string>("password")).Wait();
                    return accountRepo.GetById(context.GetArgument<int>("accountId"));
                })
                .AddAdminPermissions().RequiresMembership();

            Field<AccountGraphType>("login",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "userName", Description = "user name" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password", Description = "password" }
                    ),
                resolve: context => accountService.LoginAsync(context.GetArgument<string>("userName"), context.GetArgument<string>("password"))).CurrentUserQuery();

            Field<AccountGraphType>("logout",
                resolve: context => { accountService.LogoffAsync(); return null; }).RequiresMembership();
        }
    }
}
