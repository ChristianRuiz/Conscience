using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Roles
{
    public class RoleGraphType : ConscienceGraphType<Role>
    {
        public RoleGraphType()
        {
            Name = "Role";
            
            Field(r => r.Name);
        }
    }
}
