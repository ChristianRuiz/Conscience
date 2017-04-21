using Conscience.Application.Graph.Entities.Devices;
using Conscience.Application.Graph.Entities.Employees;
using Conscience.Application.Graph.Entities.Hosts;
using Conscience.Application.Graph.Entities.Roles;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Accounts
{
    public class AccountGraphType : ConscienceGraphType<Account>
    {
        public AccountGraphType()
        {
            Name = "Account";
            
            Field(a => a.UserName);
            Field(a => a.Email);
            Field<HostGraphType>("host", resolve: context => context.Source.Host);
            Field<EmployeeGraphType>("employee", resolve: context => context.Source.Employee);
            Field<DeviceGraphType>("device", resolve: context => context.Source.Device);
            Field<ListGraphType<RoleGraphType>>("roles", resolve: context => context.Source.Roles).AddAdminPermissions();
        }
    }

    public class RoleEnumeration : EnumerationGraphType<RoleTypes>
    {
        public RoleEnumeration()
        {
            Name = "Roles";
        }
    }
}
