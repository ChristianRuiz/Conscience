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

namespace Conscience.Web.Hubs
{
    [Authorize]
    [HubName("HostsHub")]
    public class HostsHub : Hub
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

        public HostsHub(IUnityContainer container)
        {
            _parentContainer = container;
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
        
        private const string GroupHosts = "Hosts";
        private const string GroupWeb = "Web";

        private static object _syncUsers = new object();
        private static Dictionary<string, int> Users = new Dictionary<string, int>();
        
        public override Task OnConnected()
        {
            RegisterCurrentUser();

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            RegisterCurrentUser();

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Users.ContainsKey(Context.ConnectionId))
            {
                var userId = Users[Context.ConnectionId];
                Clients.Group("Web").HostDisconnected(userId);
                Users.Remove(Context.ConnectionId);
            }

            return base.OnDisconnected(stopCalled);
        }
        
        private void RegisterCurrentUser()
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

        public void SubscribeHost(string deviceId)
        {
            RegisterCurrentUser();

            var accountId = Users[Context.ConnectionId];
            var account = AccountRepository.GetById(accountId);
            AccountRepository.UpdateDevice(account, deviceId);
            Clients.Group(GroupWeb).HostConnected(account.Host.Id, account.UserName, account.Device.CurrentLocation);
            Groups.Add(Context.ConnectionId, GroupHosts);
        }

        public void SubscribeWeb()
        {
            Groups.Add(Context.ConnectionId, GroupWeb);
        }

        public void LocationUpdates(List<Location> locations, BatteryStatus? status = null, PowerSource? powerSouce = null, int? batteryLevel = null)
        {
            var firstLocation = locations.FirstOrDefault();
            
            var accountId = Users[Context.ConnectionId];

            Log4NetLogger.Current.WriteDebug(string.Format("Location Updates - {0} - {1} - {2} - {3}", accountId, locations.Count, firstLocation != null ? firstLocation.Latitude : 0, firstLocation != null ? firstLocation.Longitude : 0));

            foreach (var location in locations)
                if (location.TimeStamp == default(DateTime))
                    location.TimeStamp = DateTime.Now;

            var account = AccountRepository.UpdateLocations(accountId, locations, status, powerSouce, batteryLevel);
            
            Clients.Group(GroupWeb).LocationUpdated(account.Host.Id, account.UserName, account.Device.CurrentLocation);
        }

        public void SendNotification(int userId)
        {
            var userRegistration = Users.First(u => u.Value == userId);
            Clients.Client(userRegistration.Key).NotificationAudio(new NotificationAudio());
        }
    }
}