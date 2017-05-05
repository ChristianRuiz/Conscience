using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Mobile.Hosts.Core.Services
{
    public class GraphQLService
    {
        private readonly AppState _appState;
        
        public GraphQLService(AppState appState)
        {
            _appState = appState;
        }

        public async Task<T> ExecuteAsync<T>(string query, Func<JToken, JToken> getDataFunction = null)
        {
            return await ExecuteAsync<T>(query, null, getDataFunction);
        }

        public async Task<T> ExecuteAsync<T>(string query, Dictionary<string, object> variables = null, Func<JToken, JToken> getDataFunction = null)
        {
            if (variables == null)
                variables = new Dictionary<string, object>();

            using (var handler = new HttpClientHandler() { CookieContainer = _appState.CookieContainer })
            {
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(_appState.ServerUrl) })
                {
                    var graphQuery = new { query, variables };
                    var content = new StringContent(JsonConvert.SerializeObject(graphQuery), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("/api/graphql", content);
                    var json = await response.Content.ReadAsStringAsync();

                    var graphResult = JsonConvert.DeserializeObject<GraphResult>(json);
                    
                    if (graphResult.Errors != null && graphResult.Errors.Any())
                    {
                        throw new GraphQLException(graphResult.Errors.First().Message);
                    }

                    var data = graphResult.Data;

                    if (getDataFunction != null)
                        data = getDataFunction(data);

                    return data.ToObject<T>();
                }
            }
        }
    }

    public class GraphResult
    {
        public JToken Data { get; set; }
        public GraphError[] Errors { get; set; }
    }

    public class GraphError
    {
        public string Message { get; set; }
    }

    public class GraphQLException : Exception
    {
        public GraphQLException() : base() { }
        public GraphQLException(string message) : base(message) { }
    }
}
