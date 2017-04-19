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

namespace Conscience.Web.Hubs
{
    [Authorize]
    [HubName("HostsHub")]
    public class HostsHub : Hub
    {
        private IUnityContainer _container;

        public HostsHub(IUnityContainer container)
        {
            _container = container.CreateChildContainer();
        }

        protected override void Dispose(bool disposing)
        {
            _container.Dispose(); // child container destroyed. all resolved objects disposed.
            base.Dispose(disposing);
        }

        public UserRepository UserRepository
        {
            get
            {
                return _container.Resolve<UserRepository>();
            }
        }

        public AccountRepository AccountRepository
        {
            get
            {
                return _container.Resolve<AccountRepository>();
            }
        }
        
        private const string GroupHosts = "Hosts";
        private const string GroupWeb = "Web";

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
                var user = UserRepository.GetById(userId);
                UserRepository.UserDisconnected(user);
            }

            return base.OnDisconnected(stopCalled);
        }
        
        private void RegisterCurrentUser()
        {
            if (!Users.ContainsKey(Context.ConnectionId))
            {
                var accountId = Context.Request.User.Identity.GetUserId<int>();
                var user = UserRepository.GetByAccountId(accountId);
                Users.Add(Context.ConnectionId, user.Id);
            }
        }

        public void SubscribeHost(string deviceId)
        {
            RegisterCurrentUser();

            var userId = Users[Context.ConnectionId];
            var user = UserRepository.GetById(userId);
            UserRepository.UpdateDevice(user, deviceId);
            Clients.Group(GroupWeb).HostConnected(user.Id, user.Account.UserName, user.Device.CurrentLocation);
            Groups.Add(Context.ConnectionId, GroupHosts);
        }

        public void SubscribeWeb()
        {
            Groups.Add(Context.ConnectionId, GroupWeb);
        }

        public void LocationUpdates(List<Location> locations)
        {
            var userId = Users[Context.ConnectionId];

            //ToDo: Location must be send by the client
            foreach (var location in locations)
                location.TimeStamp = DateTime.Now;

            var user = UserRepository.UpdateLocations(userId, locations);
            
            Clients.Group(GroupWeb).LocationUpdated(user.Id, user.Account.UserName, user.Device.CurrentLocation);
        }

        public void SendNotification(int userId)
        {
            var userRegistration = Users.First(u => u.Value == userId);
            Clients.Client(userRegistration.Key).NotificationAudio(new NotificationAudio());
        }
    }
}