using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Conscience.Domain;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.Identity;
using Conscience.Application.Services;
using Conscience.DataAccess.Repositories;
using Microsoft.Practices.Unity;
using Conscience.Domain.Enums;
using Conscience.Web.Logger;
using System.Threading;

namespace Conscience.Web.Hubs
{
    [Authorize]
    [HubName("AccountsHub")]
    public class AccountsHub : Hub
    {
        private IUnityContainer _parentContainer;

        private IUnityContainer _childContainer;

        private IUnityContainer ChildContainer
        {
            get
            {
                if (_childContainer == null)
                    _childContainer = _parentContainer.CreateChildContainer();
                return _childContainer;
            }
        }
        
        public AccountsHub(IUnityContainer container)
        {
            _parentContainer = container;
        }

        static AccountsHub()
        {
            NotificationsService.NotificationSend += NotificationsService_NotificationSend;
            AccountUpdatesService.AccountUpdated += HostUpdatesService_HostUpdated;
        }
        
        protected override void Dispose(bool disposing)
        {
            if (_childContainer != null)
            {
                _childContainer.Dispose(); // child container destroyed. all resolved objects disposed.
                _childContainer = null;
            }
            base.Dispose(disposing);
        }

        public AccountRepository AccountRepository
        {
            get
            {
                return ChildContainer.Resolve<AccountRepository>();
            }
        }
        
        public const string GroupHosts = "Hosts";
        public const string GroupWeb = "Web";
        public const string GroupAdmins = "Admins";

        private static object _syncUsers = new object();
        private static Dictionary<string, int> Users = new Dictionary<string, int>();
        
        public override Task OnConnected()
        {
            RegisterCurrentUserIfNeeded();

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            RegisterCurrentUserIfNeeded();

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Users.ContainsKey(Context.ConnectionId))
            {
                var accountId = Users[Context.ConnectionId];
                Clients.Group(GroupWeb).AccountDisconnected(accountId);
                Users.Remove(Context.ConnectionId);
            }

            return base.OnDisconnected(stopCalled);
        }
        
        private void RegisterCurrentUserIfNeeded()
        {
            lock (_syncUsers)
            {
                if (!Users.ContainsKey(Context.ConnectionId))
                {
                    var accountId = Context.Request.User.Identity.GetUserId<int>();
                    Users.Add(Context.ConnectionId, accountId);
                }
            }
        }
        
        public void SubscribeWeb()
        {
            Groups.Add(Context.ConnectionId, GroupWeb);

            var accountId = Context.Request.User.Identity.GetUserId<int>();
            var account = AccountRepository.GetById(accountId);
            if (account.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()))
            {
                Groups.Add(Context.ConnectionId, GroupAdmins);
            }
        }

        private static void HostUpdatesService_HostUpdated(object sender, AccountUpdatedEventArgs e)
        {
            ExecuteWithChildContainerInANewThread(child =>
            {
                var accountRepo = child.Resolve<AccountRepository>();
                var account = accountRepo.GetById(e.AccountId);

                var hub = GlobalHost.ConnectionManager.GetHubContext<AccountsHub>();
                hub.Clients.Group(GroupWeb).locationUpdated(account.Id, account.Device.CurrentLocation, account.Device.BatteryLevel, account.Device.BatteryStatus.ToString().ToUpperInvariant(), account.Device.LastConnection, account.Host != null ? account.Host.Status.ToString().ToUpperInvariant() : null);
            });
        }

        private static void NotificationsService_NotificationSend(object sender, NotificationEventArgs e)
        {
            ExecuteWithChildContainerInANewThread(child =>
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<AccountsHub>();

                switch (e.Notification.NotificationType)
                {
                    case NotificationTypes.CharacterModified:
                        hub.Clients.Group(GroupWeb).characterUpdated(e.Notification.Host.CurrentCharacter.Id);
                        break;
                    case NotificationTypes.StatsModified:
                        hub.Clients.Group(GroupWeb).statsModified(e.Notification.Host.Id);
                        break;
                    case NotificationTypes.CharacterAssigned:
                        foreach(var character in e.Notification.Host.Characters)
                            hub.Clients.Group(GroupWeb).characterUpdated(character.Id);
                        break;
                    case NotificationTypes.Panic:
                        hub.Clients.Group(GroupAdmins).panicButton(e.Notification.Id);
                        break;
                }
            });
        }

        private static void ExecuteWithChildContainerInANewThread(Action<IUnityContainer> action)
        {
            new Thread(new ThreadStart(() =>
            {
                var child = UnityConfig.GetConfiguredContainer().CreateChildContainer();
                try
                {
                    action(child);
                }
                finally
                {
                    child.Dispose();
                }
            })).Start();
        }
    }
}