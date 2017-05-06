using Conscience.Application.Graph.Entities.Audios;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Memories
{
    public class MemoryGraphType : ConscienceGraphType<Memory>
    {
        public MemoryGraphType()
        {
            Name = "Memory";
            
            Field(c => c.Description);
        }
    }
}
