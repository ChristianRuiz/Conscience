using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Plots
{
    public class EmployeesInputGraphType : ConscienceInputGraphType
    {
        public EmployeesInputGraphType()
        {
            Name = "EmployeesInput";
            
            Field<StringGraphType>("name");
        }
    }
}
