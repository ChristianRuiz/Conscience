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

            Field<ListGraphType<NotificationGraphType>>("markAsRead", 
                arguments: new QueryArguments(
                    new QueryArgument<ListGraphType<IntGraphType>> { Name = "ids", Description = "notification id" }
                    ),
                resolve: context =>
                {
                    var notificationIds = context.GetArgument<List<int>>("ids"); ;
                    var notifications = new List<Notification>();

                    foreach (var notificationId in notificationIds)
                    {
                        notifications.Add(notificationsService.MarkAsRead(notificationId));
                    }

                    return notifications;
                })
                .CurrentUserQuery();
        }
    }
}
