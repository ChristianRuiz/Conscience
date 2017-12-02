using Conscience.DataAccess.Repositories;
using Conscience.Application.Services;
using Conscience.Domain;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph.Entities.Characters
{
    public class CharacterMutation : ObjectGraphType<object>
    {
        public CharacterMutation(CharacterRepository characterRepo)
        {
            Name = "CharacterMutation";

            Field<CharacterGraphType>("addOrModifyCharacter",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CharacterInputGraphType>> { Name = "character", Description = "new or edited character" }
                    ),
                resolve: context =>
                {
                    var character = context.GetArgument<Character>("character");
                    if (character.Id == 0)
                    {
                        var dbCharacter = characterRepo.Add(new Character());
                        character.Id = dbCharacter.Id;
                        character = ModifyCharacter(characterRepo, character);
                    }
                    else
                    {
                        character = ModifyCharacter(characterRepo, character);
                    }
                    return character;
                })
                .AddQAPermissions();
        }

        private Character ModifyCharacter(CharacterRepository characterRepo, Character character)
        {
            var dbCharacter = characterRepo.GetById(character.Id);

            dbCharacter.Name = character.Name;
            dbCharacter.Age = character.Age;
            dbCharacter.Story = character.Story;
            dbCharacter.NarrativeFunction = character.NarrativeFunction;
            dbCharacter.Gender = character.Gender;

            var memoriesToRemove = dbCharacter.Memories.Where(dbm => !character.Memories.Any(m => dbm.Id == m.Id)).ToList();
            foreach (var memoryToRemove in memoriesToRemove)
            {
                dbCharacter.Memories.Remove(memoryToRemove);
            }

            foreach (var memory in character.Memories)
            {
                var dbMemory = dbCharacter.Memories.FirstOrDefault(c => memory.Id != 0 && c.Id == memory.Id);
                if (dbMemory == null)
                {
                    dbMemory = new Memory();
                    dbCharacter.Memories.Add(dbMemory);
                }

                dbMemory.Description = memory.Description;
            }

            character = characterRepo.Modify(dbCharacter);
            return character;
        }
    }
}
