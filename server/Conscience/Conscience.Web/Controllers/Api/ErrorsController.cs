using Conscience.Application.Graph;
using Conscience.Application.Services;
using Conscience.DataAccess.Repositories;
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
    public class ErrorsController : ApiController
    {
        private readonly IUsersIdentityService _usersService;
        
        public ErrorsController(
            IUsersIdentityService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetAsync(HttpRequestMessage request)
        {
            return PostAsync(request);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request)
        {
            var error = await request.Content.ReadToEndAsync();

            var errorContext = string.Empty;

            if (_usersService.CurrentUser != null)
            {
                errorContext += "Account: " + _usersService.CurrentUser.Id + " " + _usersService.CurrentUser.UserName + Environment.NewLine;

                if (_usersService.CurrentUser.Device != null)
                    errorContext += "Device: " + _usersService.CurrentUser.Device.DeviceId;
            }

            Log4NetLogger.Current.WriteError(errorContext + Environment.NewLine + error);

            var hub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<AccountsHub>();
            hub.Clients.Group(AccountsHub.GroupAdmins).broadcastError(errorContext, error);

            var response = request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}