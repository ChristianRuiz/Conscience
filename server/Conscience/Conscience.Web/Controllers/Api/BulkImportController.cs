using Conscience.Application.Services;
using Conscience.DataAccess;
using Conscience.Domain;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Conscience.Web.Controllers.Api
{
    [Authorize]
    public class BulkImportController : ApiController
    {
        private readonly IUnityContainer _container;

        public BulkImportController(IUnityContainer container)
        {
            _container = container;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync(HttpRequestMessage request)
        {
            var result = new BulkImportResult();

            var childContainer = _container.CreateChildContainer();

            var rowIndex = 0;
            var name = string.Empty; 

            try
            {
                var identityService = childContainer.Resolve<IUsersIdentityService>();
                if (!identityService.CurrentUser.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()))
                    throw new UnauthorizedAccessException("Current user is not a platform Admin");

                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count > 0)
                {
                    var file = httpRequest.Files[0];

                    //TODO: Use GraphQL instead of manually working with the Entity Framework context from a Controller
                    var context = childContainer.Resolve<ConscienceContext>();

                    ClearDb(context, identityService.CurrentUser);

                    var workbook = WorkbookFactory.Create(file.InputStream);

                    var sheet = workbook.GetSheet("employees");
                    
                    for (rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        try
                        {
                            if (sheet.GetRow(rowIndex) != null && !string.IsNullOrWhiteSpace(sheet.GetRow(rowIndex).GetCell(0).ToCleanString())) //null is when the row only contains empty cells 
                            {
                                var row = sheet.GetRow(rowIndex);
                                
                                name = row.GetCell(0).ToCleanString().Trim();
                                var displayName = row.GetCell(1).ToCleanString().Trim();
                                var password = row.GetCell(2).ToCleanString().Trim();
                                var role = row.GetCell(3).ToCleanString().Trim();

                                if (role.ToLowerInvariant() != RoleTypes.Host.ToString().ToLowerInvariant())
                                {
                                    var account = await CreateOrUpdateAccount(identityService, context, name, password, role);

                                    if (account.Employee == null && role.ToLowerInvariant() != RoleTypes.Admin.ToString().ToLowerInvariant())
                                    {
                                        context.Employees.Add(new Employee { Name = displayName, Account = account });
                                        context.SaveChanges();
                                    }

                                    result.Successes.Add(rowIndex + " employee - " + name);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Errors.Add(new BulkImportError { Line = rowIndex + " employee - " + name, Error = ex.ToString() });
                        }
                    }

                    sheet = workbook.GetSheet("plots");

                    var plots = new Dictionary<string, Plot>();

                    for (rowIndex = 1; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        try
                        {
                            if (sheet.GetRow(rowIndex) != null && !string.IsNullOrWhiteSpace(sheet.GetRow(rowIndex).GetCell(0).ToCleanString())) //null is when the row only contains empty cells 
                            {
                                var row = sheet.GetRow(rowIndex);

                                var plotCode = row.GetCell(0).ToCleanString().Trim();
                                name = row.GetCell(1).ToCleanString();
                                var description = row.GetCell(2).ToCleanString();
                                var eventName = row.GetCell(3).ToCleanString();
                                var eventLocation = row.GetCell(4).ToCleanString();
                                var eventTime = row.GetCell(5).DateCellValue;

                                var plot = new Plot
                                {
                                    Code = plotCode,
                                    Name = name,
                                    Description = description
                                };

                                if (!string.IsNullOrWhiteSpace(eventName))
                                {
                                    plot.Events.Add(new PlotEvent
                                    {
                                        Description = eventName,
                                        Location = eventLocation,
                                        Hour = eventTime.Hour,
                                        Minute = eventTime.Minute
                                    });
                                }

                                context.Plots.Add(plot);
                                context.SaveChanges();

                                plots.Add(plotCode, plot);
                                result.Successes.Add(rowIndex + " plot - " + name);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Errors.Add(new BulkImportError { Line = rowIndex + " plot - " + name, Error = ex.ToString() });
                        }
                    }

                    var characters = new Dictionary<int, Character>();

                    sheet = workbook.GetSheet("hosts");

                    for (rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        try
                        {
                            if (sheet.GetRow(rowIndex) != null && sheet.GetRow(rowIndex).GetCell(0) != null && !string.IsNullOrWhiteSpace(sheet.GetRow(rowIndex).GetCell(0).ToCleanString())) //null is when the row only contains empty cells 
                            {
                                var row = sheet.GetRow(rowIndex);

                                name = row.GetCell(0).ToCleanString().Trim();
                                var password = row.GetCell(1).ToCleanString().Trim();

                                var account = await CreateOrUpdateAccount(identityService, context, name, password, RoleTypes.Host.ToString());

                                var host = new Host { Account = account };
                                context.Hosts.Add(host);
                                
                                foreach (var statName in Enum.GetNames(typeof(StatNames)))
                                    host.Stats.Add(new Stats { Name = statName, Value = 10 });

                                host.Stats.First(s => s.Name == StatNames.Apperception.ToString())
                                    .Value = int.Parse(row.GetCell(7).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Candor.ToString())
                                    .Value = int.Parse(row.GetCell(8).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Vivacity.ToString())
                                    .Value = int.Parse(row.GetCell(9).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Coordination.ToString())
                                    .Value = int.Parse(row.GetCell(10).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Meekness.ToString())
                                    .Value = int.Parse(row.GetCell(11).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Humility.ToString())
                                    .Value = int.Parse(row.GetCell(12).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Cruelty.ToString())
                                    .Value = int.Parse(row.GetCell(13).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.SelfPreservation.ToString())
                                    .Value = int.Parse(row.GetCell(14).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Patience.ToString())
                                    .Value = int.Parse(row.GetCell(15).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Decisiveness.ToString())
                                    .Value = int.Parse(row.GetCell(16).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Imagination.ToString())
                                    .Value = int.Parse(row.GetCell(17).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Curiosity.ToString())
                                    .Value = int.Parse(row.GetCell(18).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Agression.ToString())
                                    .Value = int.Parse(row.GetCell(19).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Loyalty.ToString())
                                    .Value = int.Parse(row.GetCell(20).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Empathy.ToString())
                                    .Value = int.Parse(row.GetCell(21).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Tenacity.ToString())
                                    .Value = int.Parse(row.GetCell(22).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Courage.ToString())
                                    .Value = int.Parse(row.GetCell(23).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Sensuality.ToString())
                                    .Value = int.Parse(row.GetCell(24).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Charm.ToString())
                                    .Value = int.Parse(row.GetCell(25).ToCleanString().Trim());
                                host.Stats.First(s => s.Name == StatNames.Humor.ToString())
                                    .Value = int.Parse(row.GetCell(26).ToCleanString().Trim());

                                var charName = row.GetCell(2).ToCleanString().Trim();
                                var gender = row.GetCell(3).ToCleanString().Trim().ToLowerInvariant() == "female" ? Genders.Female : Genders.Male;
                                var age = int.Parse(row.GetCell(4).ToCleanString().Trim());
                                var assignedOnMonths = int.Parse(row.GetCell(5).ToCleanString().Trim());
                                var narrativeFunction = row.GetCell(27).ToCleanString().Trim();
                                var story = row.GetCell(28).ToCleanString().Trim();
                                var memories = row.GetCell(29).ToCleanString().Trim();
                                var triggers = row.GetCell(30).ToCleanString().Trim();

                                var character = new Character
                                {
                                    Name = charName,
                                    Gender = gender,
                                    Age = age,
                                    NarrativeFunction = narrativeFunction,
                                    Story = story
                                };
                                context.Characters.Add(character);
                                characters.Add(rowIndex, character);

                                host.Characters.Add(new CharacterInHost
                                {
                                    Character = character,
                                    AssignedOn = DateTime.Now - TimeSpan.FromDays(30 * assignedOnMonths)
                                });

                                using (var reader = new StringReader(memories))
                                {
                                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                                    {
                                        var memory = new Memory { Description = line };
                                        character.Memories.Add(memory);
                                    }
                                }

                                using (var reader = new StringReader(triggers))
                                {
                                    for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
                                    {
                                        var trigger = new Trigger { Description = line };
                                        character.Triggers.Add(trigger);
                                    }
                                }

                                context.SaveChanges();

                                AddCharacterInPlot(context, plots, row, character, 31);

                                AddCharacterInPlot(context, plots, row, character, 36);

                                AddCharacterInPlot(context, plots, row, character, 41);

                                AddCharacterInPlot(context, plots, row, character, 46);

                                AddCharacterInPlot(context, plots, row, character, 51);
                                
                                host.CoreMemory1 = GetCoreMemory(row, 66);

                                host.CoreMemory2 = GetCoreMemory(row, 67);

                                host.CoreMemory3 = GetCoreMemory(row, 68);

                                context.SaveChanges();

                                result.Successes.Add(rowIndex + " host - " + name);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Errors.Add(new BulkImportError { Line = rowIndex + " host - " + name, Error = ex.ToString() });
                        }
                    }

                    for (rowIndex = 2; rowIndex <= sheet.LastRowNum; rowIndex++)
                    {
                        try
                        {
                            if (sheet.GetRow(rowIndex) != null && sheet.GetRow(rowIndex).GetCell(0) != null && !string.IsNullOrWhiteSpace(sheet.GetRow(rowIndex).GetCell(0).ToCleanString())) //null is when the row only contains empty cells 
                            {
                                var row = sheet.GetRow(rowIndex);

                                name = row.GetCell(0).ToCleanString().Trim();
                                var lastCharacter = row.GetCell(6).ToCleanString().Trim();

                                if (!string.IsNullOrWhiteSpace(lastCharacter))
                                {
                                    var relatedCharacter = characters.Values.FirstOrDefault(c => c.Name.ToLowerInvariant() == lastCharacter.ToLowerInvariant());

                                    if (relatedCharacter == null)
                                    {
                                        result.Errors.Add(new BulkImportError { Line = "Characrer relations - " + name, Error = "There is no character with name " + lastCharacter });
                                    }
                                }

                                AddRelation(context, result, characters, row, characters[rowIndex], name, 56);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 57);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 58);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 59);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 60);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 61);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 62);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 63);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 64);
                                AddRelation(context, result, characters, row, characters[rowIndex], name, 65);

                                result.Successes.Add(rowIndex + " host relations - " + name);
                            }
                        }
                        catch (Exception ex)
                        {
                            result.Errors.Add(new BulkImportError { Line = rowIndex + " host - " + name, Error = ex.ToString() });
                        }
                    }
                }
                else
                    throw new Exception("Unable to open uploaded file");
            }
            catch(Exception ex)
            {
                result.Errors.Add(new BulkImportError { Line = rowIndex + " " + name, Error = ex.ToString() });
            }
            finally
            {
                childContainer.Dispose();
            }

            var response = request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(JsonConvert.SerializeObject(result), Encoding.UTF8, "application/json");
            return response;
        }

        private static CoreMemory GetCoreMemory(IRow row, int coreMemoryLine)
        {
            if (row.GetCell(coreMemoryLine) == null)
                return null;

            var transcription = row.GetCell(coreMemoryLine).ToCleanString().Trim();

            if (string.IsNullOrWhiteSpace(transcription))
                return null;

            var memory = new CoreMemory
            {
                Locked = true,
                Audio = new Audio
                {
                    Transcription = transcription
                }
            };
            return memory;
        }

        private static void AddRelation(ConscienceContext context, BulkImportResult result, Dictionary<int, Character> characters, IRow row, Character character, string hostName, int relationColumn)
        {
            if (row.GetCell(relationColumn) == null)
                return;

            var relation = row.GetCell(relationColumn).ToCleanString().Trim();

            if (string.IsNullOrWhiteSpace(relation))
                return;

            var reader = new StringReader(relation);
            var name = reader.ReadLine().Trim();
            var description = reader.ReadLine();
            
            var relatedCharacter = characters.Values.FirstOrDefault(c => c.Name.ToLowerInvariant() == name.ToLowerInvariant());

            if (relatedCharacter == null)
            {
                result.Errors.Add(new BulkImportError { Line = "Characrer relations - " + hostName, Error = "There is no character with name " + name });
                return;
            }

            character.Relations.Add(new CharacterRelation
            {
                Character = relatedCharacter,
                Description = description
            });

            context.SaveChanges();
        }

        private static void AddCharacterInPlot(ConscienceContext context, Dictionary<string, Plot> plots, IRow row, Character character, int plotColumn)
        {
            var plotCode = row.GetCell(plotColumn).ToCleanString().Trim();

            if (string.IsNullOrWhiteSpace(plotCode))
                return;

            var plotFunction = row.GetCell(plotColumn + 3).ToCleanString().Trim();
            var hasEvent = row.GetCell(plotColumn + 4).ToCleanString().Trim().ToLowerInvariant() == "yes";

            if (!plots.ContainsKey(plotCode))
                throw new Exception("There is no plot with code " + plotCode);

            var plot = plots[plotCode];
            character.Plots.Add(new CharacterInPlot
            {
                Description = plotFunction,
                Plot = plot
            });

            if (hasEvent)
                plot.Events.First().Characters.Add(character);

            context.SaveChanges();
        }

        private static async Task<ConscienceAccount> CreateOrUpdateAccount(IUsersIdentityService identityService, ConscienceContext context, string name, string password, string role)
        {
            var account = context.Accounts.FirstOrDefault(a => a.UserName.ToLower() == name.ToLower());
            if (account == null)
            {
                account = new ConscienceAccount
                {
                    UserName = name,
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                var roleName = Enum.GetNames(typeof(RoleTypes)).First(r => r.ToLowerInvariant() == role.ToLowerInvariant());
                account.Roles.Add(new Role
                {
                    Name = roleName
                });
                context.Accounts.Add(account);
                context.SaveChanges();
            }

            await identityService.ChangePasswordAsync(account.Id, password);

            return account;
        }

        private void ClearDb(ConscienceContext context, ConscienceAccount currentUser)
        {
            var roles = currentUser.Roles.ToList();

            context.Database.Delete();
            context.Database.Create();

            var account = new ConscienceAccount
            {
                UserName = currentUser.UserName,
                PasswordHash = currentUser.PasswordHash
            };
            context.Accounts.Add(account);
            foreach (var role in roles)
                account.Roles.Add(new Role { Name = role.Name });
            context.SaveChanges();
        }

        private void ClearSet<T>(ConscienceContext context, DbSet<T> set, Expression<Func<T, bool>> excludePredicate = null) where T : class
        {
            List<T> values = null; 

            if (excludePredicate != null)
                values = set.Where(excludePredicate).ToList();
            else
                values = set.ToList();

            foreach (var value in values)
                set.Remove(value);

            context.SaveChanges();
        }
    }

    public class BulkImportResult
    {
        public BulkImportResult()
        {
            Successes = new List<string>();
            Errors = new List<BulkImportError>();
        }

        public List<string> Successes { get; set; }
        public List<BulkImportError> Errors { get; set; }
    }

    public class BulkImportError
    {
        public string Line { get; set; }
        public string Error { get; set; }
        public int Hash
        {
            get
            {
                return this.GetHashCode();
            }
        }
    }

    public static class CellExtensions
    {
        public static string ToCleanString(this ICell cell)
        {
            if (cell == null)
                return string.Empty;

            cell.RemoveHyperlink();
            try
            {
                return cell.StringCellValue;
            }
            catch
            {
                return cell.ToString();
            }
        }
    }
}