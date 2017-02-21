using Conscience.Application.Graph.Entities.Accounts;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Users
{
    public class UserGraphType<T> : ObjectGraphType<T> where T : User
    {
        public UserGraphType()
        {
            Name = "User";

            Field(u => u.Id);
            Field<AccountGraphType>("account", resolve: context => context.Source.Account);
        }
    }
}
