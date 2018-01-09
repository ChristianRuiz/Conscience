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
using Conscience.Application.Graph.Entities.Accounts;

namespace Conscience.Application.Graph.Entities.Notifications
{
    public class NotificationMutation : ConscienceMutationBase
    {
        public NotificationMutation(NotificationsService notificationsService, AccountRepository accountRepo)
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

            Field<NotificationGraphType>("addCustomNotification",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "accountId", Description = "account id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "text", Description = "notification's text" },
                    new QueryArgument<StringGraphType> { Name = "audioPath", Description = "audio path" },
                    new QueryArgument<StringGraphType> { Name = "audioTranscript", Description = "audio trascript" }
                    ),
                resolve: context =>
                {
                    var accountId = context.GetArgument<int>("accountId");
                    var text = context.GetArgument<string>("text");
                    var audioPath = context.GetArgument<string>("audioPath");
                    var audioTranscript = context.GetArgument<string>("audioTranscript");

                    var account = accountRepo.GetById(accountId);

                    Audio audio = null;

                    if (!string.IsNullOrWhiteSpace(audioPath))
                    {
                        audio = new Audio { Path = audioPath };

                        if (!string.IsNullOrWhiteSpace(audioTranscript))
                            audio.Transcription = audioTranscript;
                    }

                    var notification = notificationsService.Notify(accountId, 
                        text, NotificationTypes.Custom, account.Host, null, audio);

                    return notification;
                })
                .AddAdminPermissions();

            Field<ListGraphType<NotificationGraphType>>("addCustomNotificationToRole",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<RoleEnumeration>> { Name = "role", Description = "role" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "text", Description = "notification's text" },
                    new QueryArgument<StringGraphType> { Name = "audioPath", Description = "audio path" },
                    new QueryArgument<StringGraphType> { Name = "audioTranscript", Description = "audio trascript" }
                    ),
                resolve: context =>
                {
                    var role = context.GetArgument<RoleTypes>("role");
                    var text = context.GetArgument<string>("text");
                    var audioPath = context.GetArgument<string>("audioPath");
                    var audioTranscript = context.GetArgument<string>("audioTranscript");
                    
                    Audio audio = null;

                    if (!string.IsNullOrWhiteSpace(audioPath))
                    {
                        audio = new Audio { Path = audioPath };

                        if (!string.IsNullOrWhiteSpace(audioTranscript))
                            audio.Transcription = audioTranscript;
                    }

                    var notifications = notificationsService.Notify(role,
                        text, NotificationTypes.Custom, null, null, audio);

                    return notifications;
                })
                .AddAdminPermissions();
        }
    }
}
