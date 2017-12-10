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

namespace Conscience.Application.Graph.Entities.LogEntries
{
    public class LogEntryGraphType : ConscienceGraphType<LogEntry>
    {
        public LogEntryGraphType()
        {
            Name = "LogEntry";
            
            Field(a => a.Description);
            Field(a => a.TimeStamp);
            Field<HostGraphType>("host", resolve: context => context.Source.Host);
            Field<EmployeeGraphType>("employee", resolve: context => context.Source.Employee);
        }
    }
}
