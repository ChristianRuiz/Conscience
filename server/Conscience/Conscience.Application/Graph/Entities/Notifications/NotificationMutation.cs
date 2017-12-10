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

namespace Conscience.Application.Graph.Entities.Notifications
{
    public class NotificationMutation : ConscienceMutationBase
    {
        public NotificationMutation(NotificationsService notificationsService)
        {
            Name = "NotificationMutation";

            Field<NotificationGraphType>("markAsRead", 
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "id", Description = "notification id" }
                    ),
                resolve: context =>
                {
                    var notificationId = context.GetArgument<int>("id"); ;
                    return notificationsService.MarkAsRead(notificationId);
                }
                )
                .CurrentUserQuery();
        }
    }
}
