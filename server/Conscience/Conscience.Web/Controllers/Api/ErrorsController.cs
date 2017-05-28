using Conscience.Application.Graph;
using Conscience.Application.Services;
using Conscience.Web.Hubs;
using Microsoft.AspNet.SignalR;
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
            var stream = await request.Content.ReadAsStreamAsync();
            stream.Position = 0;
            var error = new StreamReader(stream).ReadToEnd();

            if (AccountsHub.Current != null)
                AccountsHub.Current.ReportError(error);

            var response = request.CreateResponse(HttpStatusCode.OK);
            return response;
        }
    }
}