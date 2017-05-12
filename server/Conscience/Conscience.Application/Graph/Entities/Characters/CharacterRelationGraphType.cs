using Conscience.Application.Graph.Entities.Memories;
using Conscience.Application.Graph.Entities.Plots;
using Conscience.Application.Graph.Entities.Triggers;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Characters
{
    public class CharacterRelationGraphType : ConscienceGraphType<CharacterRelation>
    {
        public CharacterRelationGraphType()
        {
            Name = "CharacterRelation";
            
            Field(c => c.Description);
            Field<CharacterGraphType>("character", resolve: context => context.Source.Character);
        }
    }
}
