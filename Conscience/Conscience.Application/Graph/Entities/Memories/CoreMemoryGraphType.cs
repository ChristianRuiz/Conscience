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
    public class CoreMemoryGraphType : MemoryGraphType<CoreMemory>
    {
        public CoreMemoryGraphType()
        {
            Name = "CoreMemory";
        }
    }
}
