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
    public class CharacterMutation : ConscienceMutationBase
    {
        private readonly CharacterRepository _characterRepo;
        private readonly PlotRepository _plotRepo;
        private readonly LogEntryService _logService;

        public CharacterMutation(CharacterRepository characterRepo, PlotRepository plotRepo, LogEntryService logService,
            NotificationsService notificationsService, EmployeeRepository employeeRepo, IUsersIdentityService usersService)
        {
            Name = "CharacterMutation";

            _characterRepo = characterRepo;
            _plotRepo = plotRepo;
            _logService = logService;

            Field<CharacterGraphType>("addOrModifyCharacter",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<CharacterInputGraphType>> { Name = "character", Description = "new or edited character" },
                    new QueryArgument<ListGraphType<CharacterRelationInputGraphType>> { Name = "inverseRelations", Description = "inverse relations" }
                    ),
                resolve: context =>
                {
                    var character = context.GetArgument<Character>("character");
                    var inverseRelations = context.GetArgument<List<CharacterRelation>>("inverseRelations");
                    if (character.Id == 0)
                    {
                        var dbCharacter = characterRepo.Add(new Character());
                        character.Id = dbCharacter.Id;
                        character = ModifyCharacter(character, inverseRelations);
                        _logService.Log(GetHost(character), $"Added a new character with name '{character.Name}'");
                    }
                    else
                    {
                        var dbCharacter = _characterRepo.GetById(character.Id);
                        var host = GetHost(dbCharacter);
                        _logService.Log(host, $"Modified character with name '{character.Name}'");
                        character = ModifyCharacter(character, inverseRelations);

                        if (host != null)
                        {
                            var employee = employeeRepo.GetById(usersService.CurrentUser.Employee.Id);
                            notificationsService.Notify(host.Account.Id, $"{employee.Name} has modified your character.", NotificationTypes.CharacterModified, host: host, employee: employee);
                        }
                    }
                    return character;
                })
                .AddPlotEditorPermissions();

            Field<IntGraphType>("deleteCharacter",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id", Description = "id of the character to delete" }
                    ),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    var character = characterRepo.GetById(id);
                    characterRepo.Delete(character);
                    _logService.Log($"Deleted a character with name '{character.Name}'");
                    return id;
                })
                .AddPlotEditorPermissions();
        }
        
        private Character ModifyCharacter(Character character, List<CharacterRelation> inverseRelations)
        {
            var dbCharacter = _characterRepo.GetById(character.Id);

            dbCharacter.Name = character.Name;
            dbCharacter.Age = character.Age;
            dbCharacter.Story = character.Story;
            dbCharacter.NarrativeFunction = character.NarrativeFunction;
            dbCharacter.Gender = character.Gender;

            ModifyCollection(_characterRepo, _logService, character.Memories, dbCharacter.Memories, (memory, dbMemory) =>
            {
                dbMemory.Description = memory.Description;
            });

            ModifyCollection(_characterRepo, _logService, character.Triggers, dbCharacter.Triggers, (trigger, dbTrigger) =>
            {
                dbTrigger.Description = trigger.Description;
            });

            ModifyCollection(_characterRepo, _logService, character.Plots, dbCharacter.Plots, (plot, dbPlot) =>
            {
                dbPlot.Description = plot.Description;
                if (dbPlot.Plot == null)
                    dbPlot.Plot = _plotRepo.GetById(plot.Plot.Id);
            },
            (plot, dbPlot) => plot.Plot.Id == dbPlot.Plot.Id);

            ModifyCollection(_characterRepo, _logService, character.Relations, dbCharacter.Relations, (relation, dbRelation) =>
            {
                dbRelation.Description = relation.Description;
                dbRelation.Character = _characterRepo.GetById(relation.Character.Id);
            },
            (relation, dbRelation) => relation.Character.Id == dbRelation.Character.Id);

            character = _characterRepo.Modify(dbCharacter);

            if (inverseRelations != null)
            {
                foreach (var inverseRelation in inverseRelations)
                {
                    var relatedCharacter = _characterRepo.GetById(inverseRelation.Character.Id);

                    if (string.IsNullOrEmpty(inverseRelation.Description))
                    {
                        var itemToRemove = relatedCharacter.Relations.FirstOrDefault(r => r.Character.Id == character.Id);
                        if (itemToRemove != null)
                        {
                            relatedCharacter.Relations.Remove(itemToRemove);
                            _characterRepo.DeleteChild(itemToRemove);
                        }
                    }
                    else
                    {
                        var relationsToAddOrModify = new List<CharacterRelation>
                        {
                            new CharacterRelation
                            {
                                Id = inverseRelation.Id,
                                Description = inverseRelation.Description,
                                Character = character
                            }
                        };

                        ModifyCollection(_characterRepo, _logService, relationsToAddOrModify, relatedCharacter.Relations, (relation, dbRelation) =>
                        {
                            dbRelation.Description = relation.Description;
                            dbRelation.Character = relation.Character;
                        },
                        (relation, dbRelation) => relation.Character.Id == dbRelation.Character.Id,
                        shouldRemove: false);
                    }
                }
            }

            return _characterRepo.GetById(character.Id);
        }

        private static Host GetHost(Character character)
        {
            return character.CurrentHost != null ? character.CurrentHost.Host : null;
        }
    }
}
