using Conscience.Application.Graph.ValidationRules;
using GraphQL;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public class ConscienceGraphQueryExecuter
    {
        private readonly IUnityContainer _container;
        private readonly IDocumentWriter _writer;

        private readonly IDictionary<string, string> _namedQueries;

        public ConscienceGraphQueryExecuter(IUnityContainer container, IDocumentWriter writer)
        {
            _container = container;
            _writer = writer;

            _namedQueries = new Dictionary<string, string>
            {
                //["a-query"] = @"query foo { hero { name } }"
            };
        }

        public async Task<ExecutionResult> ExecuteQuery(GraphQLQuery query, object userContext = null)
        {
            var childContainer = _container.CreateChildContainer();

            try
            {
                var executer = childContainer.Resolve<IDocumentExecuter>();
                var schema = childContainer.Resolve<ConscienceSchema>();
                
                var queryToExecute = query.Query;

                if (!string.IsNullOrWhiteSpace(query.NamedQuery))
                {
                    queryToExecute = _namedQueries[query.NamedQuery];
                }

                var result = await executer.ExecuteAsync(_ =>
                {
                    _.Schema = schema;
                    _.Query = queryToExecute;
                    _.OperationName = query.OperationName;
                    _.Inputs = JObject.FromObject(query.Variables).ToString().ToInputs();

                    _.ValidationRules = new List<IValidationRule> { new MembershipValidationRule(), new RolesValidationRule() };
                    _.UserContext = userContext;

                    _.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 25 };
                    _.FieldMiddleware.Use<InstrumentFieldsMiddleware>();

                }).ConfigureAwait(false);
                return result;
            }
            finally
            {
                if (childContainer != null)
                    childContainer.Dispose();
            }
        }

        public string GetJSON(ExecutionResult result)
        {
            return _writer.Write(result);
        }
    }
}
