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

            if (queries == null)
                queries = new GraphQLQuery[] { await TryGetQueryFromRequestAsync(request) };

            if (queries.Count() == 0)
                throw new ArgumentException("No GraphQL query found in the request");

            var hasAnyErrors = false;
            var json = string.Empty;
            
            if (queries.Count() == 1)
            {
                var result = await _executer.ExecuteQuery(queries.First(), _usersService.CurrentUser);

                hasAnyErrors = hasAnyErrors || result.Errors?.Count > 0;

                json = _executer.GetJSON(result);
            }

            if (queries != null)
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
            return await TryGetFromRequestAsync<GraphQLQuery[]>(request);
        }

        private async Task<GraphQLQuery> TryGetQueryFromRequestAsync(HttpRequestMessage request)
        {
            return await TryGetFromRequestAsync<GraphQLQuery>(request);
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