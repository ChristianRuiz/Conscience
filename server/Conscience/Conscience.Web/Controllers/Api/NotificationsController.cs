﻿using Conscience.Application.Graph;
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
        private const double MinimumBatteryLevel = 0.25;
        private const double MaximumBatteryLevel = 0.75;

        private readonly IUsersIdentityService _usersService;
        private readonly AccountRepository _accountsRepo;
        private readonly NotificationsService _notificationsService;
        private readonly AccountUpdatesService _hostUpdatesService;

        public NotificationsController(
            IUsersIdentityService usersService, AccountRepository accountsRepo, NotificationsService notificationsService, AccountUpdatesService hostUpdatesService)
        {
            _usersService = usersService;
            _accountsRepo = accountsRepo;
            _notificationsService = notificationsService;
            _hostUpdatesService = hostUpdatesService;
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

                var currentBatteryLevel = currentUser.Device != null ? currentUser.Device.BatteryLevel : 0;

                //_accountsRepo.UpdateDevice(currentUser.Id, updates.deviceId);
                //var account = _accountsRepo.UpdateLocations(currentUser.Id, updates.locations, updates.charging ? BatteryStatus.Charging : BatteryStatus.NotCharging, updates.batteryLevel);
                var account = _accountsRepo.GetById(currentUser.Id);

                _hostUpdatesService.BroadcastAccountUpdated(account.Id);

                if (account.Host != null)
                {
                    if (currentBatteryLevel > MinimumBatteryLevel && updates.batteryLevel < MinimumBatteryLevel)
                    {
                        _notificationsService.Notify(account.Id, "Low battery", NotificationTypes.LowBattery, account.Host);
                        _notificationsService.Notify(RoleTypes.CompanyMaintenance, $"Low battery host '{account.Host.CurrentCharacter.Character.Name}'", NotificationTypes.LowBattery, account.Host);
                    }
                    else if (currentBatteryLevel < MaximumBatteryLevel && updates.batteryLevel > MaximumBatteryLevel && updates.charging)
                    {
                        _notificationsService.Notify(account.Id, "Battery charged, bring the external battery to the Saloon.", NotificationTypes.BatteryCharged, account.Host);
                    }
                    
                    if (account.Host.CurrentCharacter != null)
                    {
                        var events = account.Host.CurrentCharacter.Character.Plots.SelectMany(p => p.Plot.Events).ToList();
                        var now = DateTime.Now;
                        if (events.Select(e => new DateTime(now.Year, now.Month, now.Day, e.Hour, e.Minute, 0)).Any(date => date > DateTime.Now && (date - DateTime.Now).TotalMinutes < 15))
                        {
                            _notificationsService.Notify(account.Id, "You have an event in 15 minutes.", NotificationTypes.EventIn15Min, account.Host);
                        }
                    }
                }
                

                var response = request.CreateResponse(_notificationsService.HasUnprocessedNotifications(currentUser.Id) ? HttpStatusCode.Accepted : HttpStatusCode.OK);
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