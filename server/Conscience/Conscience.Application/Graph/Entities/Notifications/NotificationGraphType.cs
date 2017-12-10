using Conscience.Application.Graph.Entities.Accounts;
using Conscience.Application.Graph.Entities.Audios;
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

namespace Conscience.Application.Graph.Entities.Notifications
{
    public class NotificationGraphType : ConscienceGraphType<Notification>
    {
        public NotificationGraphType()
        {
            Name = "Notification";
            
            Field(a => a.Description);
            Field(a => a.TimeStamp);
            Field(a => a.Read);
            Field<NotificationTypesEnumeration>("notificationType", resolve: context => context.Source.NotificationType);
            Field<AccountGraphType>("owner", resolve: context => context.Source.Owner);
            Field<HostGraphType>("host", resolve: context => context.Source.Host);
            Field<EmployeeGraphType>("employee", resolve: context => context.Source.Employee);
            Field<AudioGraphType>("audio", resolve: context => context.Source.Audio);
        }
    }

    public class NotificationTypesEnumeration : EnumerationGraphType<NotificationTypes>
    {
        public NotificationTypesEnumeration()
        {
            Name = "NotificationTypes";
        }
    }
}
