using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities
{
    public class InputConscienceGraphType : InputObjectGraphType
    {
        public InputConscienceGraphType(string name, Dictionary<string, Type> arguments)
        {
            Name = name;

            foreach(var argument in arguments)
            {
                Field(argument.Value, argument.Key);
            }
        }
    }
}
