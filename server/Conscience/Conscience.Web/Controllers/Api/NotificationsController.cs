using Conscience.Application.Graph;
using Conscience.Application.Services;
using Conscience.DataAccess.Repositories;
using Conscience.Domain;
using Conscience.Domain.Enums;
using Conscience.Web.Hubs;
using Conscience.Web.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Conscience.Web.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly IUsersIdentityService _usersService;
        private readonly AccountRepository _accountsRepo;

        public NotificationsController(
            IUsersIdentityService usersService, AccountRepository accountsRepo)
        {
            _usersService = usersService;
            _accountsRepo = accountsRepo;
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetAsync(HttpRequestMessage request)
        {
            return PostAsync(request);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request)
        {
            try
            {
                var updates = await request.Content.DeserializeAsync<DeviceUpdates>();

                var firstLocation = updates.locations.FirstOrDefault();

                var currentUser = _usersService.CurrentUser;

                Log4NetLogger.Current.WriteDebug(string.Format("Location Updates - {0} - {1} - {2} - {3}", currentUser.Id, updates.locations.Count, firstLocation != null ? firstLocation.Latitude : 0, firstLocation != null ? firstLocation.Longitude : 0));

                foreach (var location in updates.locations)
                    if (location.TimeStamp == default(DateTime))
                        location.TimeStamp = DateTime.Now;

                //TODO: Uncomment this lines for the local execution
                //_accountsRepo.UpdateDevice(currentUser.Id, updates.deviceId);
                //var account = _accountsRepo.UpdateLocations(currentUser.Id, updates.locations, updates.charging ? BatteryStatus.Charging : BatteryStatus.NotCharging, updates.batteryLevel);

                //var hub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AccountsHub>();
                //hub.Clients.Group(AccountsHub.GroupWeb).locationUpdated(account.Id, account.Device.CurrentLocation);

                //TODO: Return unread notifications

                var response = request.CreateResponse(HttpStatusCode.OK);
                return response;
            }
            catch(Exception ex)
            {
                Log4NetLogger.Current.WriteError("Error getting device updates", ex);
                throw;
            }
        }
    }

    public class DeviceUpdates
    {
        public DeviceUpdates()
        {
            locations = new List<Location>();
        }

        public bool charging { get; set; }
        public double batteryLevel { get; set; }
        public string deviceId { get; set; }
        public List<Location> locations { get; set; }
    }

}