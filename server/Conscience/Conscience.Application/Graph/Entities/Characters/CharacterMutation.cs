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
        }

        private Character ModifyCharacter(Character character)
        {
            var dbCharacter = _characterRepo.GetById(character.Id);

            dbCharacter.Name = character.Name;
            dbCharacter.Age = character.Age;
            dbCharacter.Story = character.Story;
            dbCharacter.NarrativeFunction = character.NarrativeFunction;
            dbCharacter.Gender = character.Gender;

            ModifyCollection(character.Memories, dbCharacter.Memories, (memory, dbMemory) =>
            {
                dbMemory.Description = memory.Description;
            });

            ModifyCollection(character.Triggers, dbCharacter.Triggers, (trigger, dbTrigger) =>
            {
                dbTrigger.Description = trigger.Description;
            });

            ModifyCollection(character.Plots, dbCharacter.Plots, (plot, dbPlot) =>
            {
                dbPlot.Description = plot.Description;
                if (dbPlot.Plot == null)
                    dbPlot.Plot = _plotRepo.GetById(plot.Plot.Id);
            },
            (plot, dbPlot) => plot.Plot.Id == dbPlot.Plot.Id);

            ModifyCollection(character.Relations, dbCharacter.Relations, (relation, dbRelation) =>
            {
                dbRelation.Description = relation.Description;
                dbRelation.Character = _characterRepo.GetById(relation.Character.Id);
            },
            (relation, dbRelation) => relation.Character.Id == dbRelation.Character.Id);

            character = _characterRepo.Modify(dbCharacter);
            return character;
        }

        private void ModifyCollection<T>(ICollection<T> collection, ICollection<T> dbCollection, Action<T, T> setProperties, Func<T, T, bool> additionalPredicate = null)
            where T : IdentityEntity, new()
        {
            var itemsToRemove = dbCollection.Where(dbItem => !collection.Any(item => dbItem.Id == item.Id && (additionalPredicate == null || additionalPredicate(item, dbItem)))).ToList();
            foreach (var itemToRemove in itemsToRemove)
            {
                dbCollection.Remove(itemToRemove);
                _characterRepo.DeleteChild(itemToRemove);
            }

            foreach (var item in collection)
            {
                var dbItem = dbCollection.FirstOrDefault(c => item.Id != 0 && c.Id == item.Id);
                if (dbItem == null)
                {
                    dbItem = new T();
                    dbCollection.Add(dbItem);
                }

                setProperties(item, dbItem);
            }
        }
    }
}
