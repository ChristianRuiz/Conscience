using Conscience.Application.Graph;
using Conscience.Application.Services;
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
            return PostAsync(request);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request)
        {
            var queries = await TryGetQueriesFromRequestAsync(request);
            
            var hasAnyErrors = false;
            var json = string.Empty;
            
            if (queries == null)
            {
                var query = await TryGetQueryFromRequestAsync(request);

                var result = await _executer.ExecuteQuery(query, _usersService.CurrentUser);

                hasAnyErrors = hasAnyErrors || result.Errors?.Count > 0;

                json = _executer.GetJSON(result);
            }
            else
            {
                List<string> jsons = new List<string>();

                foreach(var query in queries)
                {
                    var result = await _executer.ExecuteQuery(query, _usersService.CurrentUser);

                    hasAnyErrors = hasAnyErrors || result.Errors?.Count > 0;

                    jsons.Add(_executer.GetJSON(result));
                }

                json = JArray.Parse("[" + string.Join(",", jsons) + "]").ToString();
            }

            var httpResult = hasAnyErrors
                ? HttpStatusCode.BadRequest
                : HttpStatusCode.OK;
            
            var response = request.CreateResponse(httpResult);
            response.Content = new StringContent(json, Encoding.UTF8, "application/json");

            return response;
        }

        private async Task<GraphQLQuery[]> TryGetQueriesFromRequestAsync(HttpRequestMessage request)
        {
            var queries = await TryGetFromRequestAsync<GraphQLQuery[]>(request);
            if (queries != null)
                foreach (var query in queries)
                    FixOperationName(query);
            return queries;
        }

        private async Task<GraphQLQuery> TryGetQueryFromRequestAsync(HttpRequestMessage request)
        {
            var query = await TryGetFromRequestAsync<GraphQLQuery>(request);
            FixOperationName(query);
            return query;
        }

        private static void FixOperationName(GraphQLQuery query)
        {
            if (query.OperationName == "null")
                query.OperationName = null;
            if (query.Variables == null)
                query.Variables = new Dictionary<string, object>();
        }

        private async Task<T> TryGetFromRequestAsync<T>(HttpRequestMessage request)
        {
            try
            {
                var stream = await request.Content.ReadAsStreamAsync();
                stream.Position = 0;
                var json = new StreamReader(stream).ReadToEnd();
                var query = JsonConvert.DeserializeObject<T>(json);
                return query;
            }
            catch
            {
                return default(T);
            }
        }
    }
}