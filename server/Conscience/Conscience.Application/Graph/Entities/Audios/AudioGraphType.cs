using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Audios
{
    public class AudioGraphType : ConscienceGraphType<Audio>
    {
        public AudioGraphType()
        {
            Name = "Audio";

            Field(c => c.Transcription);
            Field(c => c.IsEmbeded);
            Field(c => c.Path);
        }
    }
}
