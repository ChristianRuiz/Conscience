using Conscience.Application.Graph;
using Conscience.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Conscience.Web.Controllers
{
    public class GraphQLController : ApiController
    {
        private readonly ConscienceGraphQueryExecuter _executer;
        private readonly IUsersIdentityService _usersService;

        public GraphQLController(
            ConscienceGraphQueryExecuter executer,
            IUsersIdentityService usersService)
        {
            _executer = executer;
            _usersService = usersService;
        }

        // This will display an example error
        [HttpGet]
        public Task<HttpResponseMessage> GetAsync(HttpRequestMessage request)
        {
            return PostAsync(request, new GraphQLQuery { Query = "query foo { hero }", Variables = "" });
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request, GraphQLQuery query)
        {
            var result = await _executer.ExecuteQuery(query, _usersService.CurrentUser);

            var httpResult = result.Errors?.Count > 0
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.OK;

            var json = _executer.GetJSON(result);

            var response = request.CreateResponse(httpResult);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return response;
        }
    }
}