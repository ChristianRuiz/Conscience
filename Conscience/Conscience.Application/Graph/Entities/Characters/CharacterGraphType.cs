using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Characters
{
    public class CharacterGraphType : ObjectGraphType<Character>
    {
        public CharacterGraphType()
        {
            Name = "Character";

            Field(c => c.Id);
            Field(c => c.Description);
        }
    }
}
