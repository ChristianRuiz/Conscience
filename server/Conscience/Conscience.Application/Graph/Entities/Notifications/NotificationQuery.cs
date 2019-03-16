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
    public class NotificationQuery : ObjectGraphType<object>
    {
        public NotificationQuery(NotificationRepository notificationsRepo, IUsersIdentityService userService)
        {
            Name = "NotificationQuery";

            Field<ListGraphType<NotificationGraphType>>("current", 
                arguments: new QueryArguments(
                    new QueryArgument<IntGraphType> { Name = "first", Description = "number of items to take" },
                    new QueryArgument<IntGraphType> { Name = "offset", Description = "offset of the first item to return" }
                    ),
                resolve: context =>
                {
                    var notifications = notificationsRepo.GetByAccount(userService.CurrentUser.Id)
                                        .Where(n => n.Host == null || (!n.Host.Hidden || n.Host.Id == n.OwnerId))
                                        .OrderByDescending(l => l.TimeStamp);

                    
                    var unprocessedNotifications = notifications.Count(n => !n.Processed);

                    if (notifications.Any(n => !n.Processed))
                    {
                        foreach(var notification in notifications.Where(n => !n.Processed).ToList())
                        {
                            notification.Processed = true;
                            notificationsRepo.Modify(notification);
                        }
                    }

                    int? first = null;

                    if (context.Arguments["first"] != null)
                        first = context.GetArgument<int>("first");

                    if (unprocessedNotifications > 0 && (first.HasValue && first.Value < unprocessedNotifications))
                        first = unprocessedNotifications;

                    return notifications.ApplyPagination(context, first: first);
                }
                )
                .CurrentUserQuery();
        }
    }
}
