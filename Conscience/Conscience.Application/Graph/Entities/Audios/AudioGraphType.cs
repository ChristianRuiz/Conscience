using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Audios
{
    public class AudioGraphType : ObjectGraphType<Audio>
    {
        public AudioGraphType()
        {
            Name = "Audio";

            Field(c => c.Id);
            Field(c => c.IsEmbeded);
            Field(c => c.Path);
        }
    }
}
