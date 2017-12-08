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
    public class CharacterMutation : ConscienceMutation
    {
        private readonly CharacterRepository _characterRepo;
        private readonly PlotRepository _plotRepo;

        public CharacterMutation(CharacterRepository characterRepo, PlotRepository plotRepo)
        {
            Name = "CharacterMutation";

            _characterRepo = characterRepo;
            _plotRepo = plotRepo;

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
                        character = ModifyCharacter(character);
                    }
                    else
                    {
                        character = ModifyCharacter(character);
                    }
                    return character;
                })
                .AddQAPermissions();

            Field<IntGraphType>("deleteCharacter",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the character to delete" }
                    ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var character = characterRepo.GetById(id);
                    characterRepo.Delete(character);
                    return id;
                })
                .AddQAPermissions();
        }

        private Character ModifyCharacter(Character character)
        {
            var dbCharacter = _characterRepo.GetById(character.Id);

            dbCharacter.Name = character.Name;
            dbCharacter.Age = character.Age;
            dbCharacter.Story = character.Story;
            dbCharacter.NarrativeFunction = character.NarrativeFunction;
            dbCharacter.Gender = character.Gender;

            ModifyCollection(_characterRepo, character.Memories, dbCharacter.Memories, (memory, dbMemory) =>
            {
                dbMemory.Description = memory.Description;
            });

            ModifyCollection(_characterRepo, character.Triggers, dbCharacter.Triggers, (trigger, dbTrigger) =>
            {
                dbTrigger.Description = trigger.Description;
            });

            ModifyCollection(_characterRepo, character.Plots, dbCharacter.Plots, (plot, dbPlot) =>
            {
                dbPlot.Description = plot.Description;
                if (dbPlot.Plot == null)
                    dbPlot.Plot = _plotRepo.GetById(plot.Plot.Id);
            },
            (plot, dbPlot) => plot.Plot.Id == dbPlot.Plot.Id);

            ModifyCollection(_characterRepo, character.Relations, dbCharacter.Relations, (relation, dbRelation) =>
            {
                dbRelation.Description = relation.Description;
                dbRelation.Character = _characterRepo.GetById(relation.Character.Id);
            },
            (relation, dbRelation) => relation.Character.Id == dbRelation.Character.Id);

            character = _characterRepo.Modify(dbCharacter);
            return character;
        }
    }
}
