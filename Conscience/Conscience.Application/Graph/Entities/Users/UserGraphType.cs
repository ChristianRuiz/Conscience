using Conscience.Application.Graph.Entities.Accounts;
using Conscience.Application.Graph.Entities.Devices;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Users
{
    public class UserGraphType<T> : ConscienceGraphType<T> where T : User
    {
        public UserGraphType()
        {
            Name = "User";
            
            Field<AccountGraphType>("account", resolve: context => context.Source.Account);
            Field<DeviceGraphType>("device", resolve: context => context.Source.Device);
        }
    }
}
