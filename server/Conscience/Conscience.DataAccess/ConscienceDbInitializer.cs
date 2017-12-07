using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.DataAccess
{
    /*
     * This initial data is ment to test the App during the development phase, use the BulkImport to fill the system with the right users and data.
     * We have at least a user per role type and several hosts with relationships and plots.
     * All the passwords for those demo accounts are '123456'
     */
    public class ConscienceDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConscienceContext>
    {
        public override void InitializeDatabase(ConscienceContext context)
        {
            try
            {
                base.InitializeDatabase(context);

                if (!context.Roles.Any())
                {
                    Seed(context);
                }
            }
            catch (Exception ex)
            {
                //Unable to initialize the db
            }
        }

        protected override void Seed(ConscienceContext context)
        {
            if (!context.Roles.Any())
            {
                var adminRole = new Role { Name = RoleTypes.Admin.ToString() };
                context.Roles.Add(adminRole);
                var companyAdminRole = new Role { Name = RoleTypes.CompanyAdmin.ToString() };
                context.Roles.Add(companyAdminRole);
                var companyQARole = new Role { Name = RoleTypes.CompanyQA.ToString() };
                context.Roles.Add(companyQARole);
                var companyBehaviourRole = new Role { Name = RoleTypes.CompanyBehaviour.ToString() };
                context.Roles.Add(companyBehaviourRole);
                var companyPlotRole = new Role { Name = RoleTypes.CompanyPlot.ToString() };
                context.Roles.Add(companyPlotRole);
                var companyManteinanceRole = new Role { Name = RoleTypes.CompanyMaintenance.ToString() };
                context.Roles.Add(companyManteinanceRole);
                var hostRole = new Role { Name = RoleTypes.Host.ToString() };
                context.Roles.Add(hostRole);
                context.SaveChanges();

                var arnold = new ConscienceAccount
                {
                    UserName = "arnold",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                arnold.Roles.Add(adminRole);
                context.Accounts.Add(arnold);

                #if DEBUG

                var ford = new ConscienceAccount
                {
                    UserName = "ford",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/ford.png"
                };
                ford.Roles.Add(companyAdminRole);
                context.Accounts.Add(ford);

                var theresa = new ConscienceAccount
                {
                    UserName = "theresa",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/theresa.png"
                };
                theresa.Roles.Add(companyQARole);
                context.Accounts.Add(theresa);

                var elsie = new ConscienceAccount
                {
                    UserName = "elsie",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/elsie.png"
                };
                elsie.Roles.Add(companyBehaviourRole);
                context.Accounts.Add(elsie);

                var bernard = new ConscienceAccount
                {
                    UserName = "bernard",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/bernard.png"
                };
                bernard.Roles.Add(companyBehaviourRole);
                bernard.Roles.Add(hostRole);
                context.Accounts.Add(bernard);

                var sizemore = new ConscienceAccount
                {
                    UserName = "sizemore",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/sizemore.png"
                };
                sizemore.Roles.Add(companyPlotRole);
                context.Accounts.Add(sizemore);

                var dolores = new ConscienceAccount
                {
                    UserName = "dolores",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/dolores.png"
                };
                dolores.Roles.Add(hostRole);
                context.Accounts.Add(dolores);

                var peter = new ConscienceAccount
                {
                    UserName = "peter",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/peter.png"
                };
                peter.Roles.Add(hostRole);
                context.Accounts.Add(peter);

                var teddy = new ConscienceAccount
                {
                    UserName = "teddy",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/teddy.png"
                };
                teddy.Roles.Add(hostRole);
                context.Accounts.Add(teddy);

                var maeve = new ConscienceAccount
                {
                    UserName = "maeve",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/maeve.png"
                };
                maeve.Roles.Add(hostRole);
                context.Accounts.Add(maeve);

                var escaton = new ConscienceAccount
                {
                    UserName = "escaton",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q==",
                    PictureUrl = "/Content/images/sample/escaton.png"
                };
                escaton.Roles.Add(hostRole);
                context.Accounts.Add(escaton);
                
                context.SaveChanges();
                
                context.Employees.Add(new Employee { Account = ford, Name = "Dr. Robert Ford" });
                context.Employees.Add(new Employee { Account = theresa, Name = "Theresa Cullen" });
                context.Employees.Add(new Employee { Account = elsie, Name = "Elsie Hughes" });
                context.Employees.Add(new Employee { Account = bernard, Name = "Bernard Lowe" });
                context.Employees.Add(new Employee { Account = sizemore, Name = "Lee Sizemore" });

                var hosts = new List<Host>();
                var hBernard = new Host { Account = bernard, Hidden = true };
                hosts.Add(hBernard);
                var hDolores = new Host { Account = dolores };
                hosts.Add(hDolores);
                var hPeter = new Host { Account = peter };
                hosts.Add(hPeter);
                var hTeddy = new Host { Account = teddy };
                hosts.Add(hTeddy);
                var hMaeve = new Host { Account = maeve };
                hosts.Add(hMaeve);
                var hEscaton = new Host { Account = escaton };
                hosts.Add(hEscaton);

                foreach (var host in hosts)
                {
                    foreach (var statName in Enum.GetNames(typeof(StatNames)))
                        host.Stats.Add(new Stats { Name = statName, Value = 10 });
                    context.Hosts.Add(host);
                }

                context.SaveChanges();

                hBernard.Stats.First(s => s.Name == StatNames.Apperception.ToString()).Value = 18;
                hBernard.Stats.First(s => s.Name == StatNames.Coordination.ToString()).Value = 15;
                hBernard.Stats.First(s => s.Name == StatNames.Humor.ToString()).Value = 14;
                hBernard.Stats.First(s => s.Name == StatNames.Loyalty.ToString()).Value = 20;
                hBernard.Stats.First(s => s.Name == StatNames.Patience.ToString()).Value = 20;

                hDolores.Stats.First(s => s.Name == StatNames.Sensuality.ToString()).Value = 12;
                hDolores.Stats.First(s => s.Name == StatNames.Empathy.ToString()).Value = 15;
                hDolores.Stats.First(s => s.Name == StatNames.Candor.ToString()).Value = 18;
                hDolores.Stats.First(s => s.Name == StatNames.Charm.ToString()).Value = 15;
                hDolores.Stats.First(s => s.Name == StatNames.Cruelty.ToString()).Value = 3;

                hPeter.Stats.First(s => s.Name == StatNames.Loyalty.ToString()).Value = 20;
                hPeter.Stats.First(s => s.Name == StatNames.Empathy.ToString()).Value = 15;
                hPeter.Stats.First(s => s.Name == StatNames.Courage.ToString()).Value = 18;
                hPeter.Stats.First(s => s.Name == StatNames.Humility.ToString()).Value = 15;
                hPeter.Stats.First(s => s.Name == StatNames.Sensuality.ToString()).Value = 5;

                hTeddy.Stats.First(s => s.Name == StatNames.Sensuality.ToString()).Value = 15;
                hTeddy.Stats.First(s => s.Name == StatNames.Charm.ToString()).Value = 18;
                hTeddy.Stats.First(s => s.Name == StatNames.Courage.ToString()).Value = 17;
                hTeddy.Stats.First(s => s.Name == StatNames.Coordination.ToString()).Value = 15;
                hTeddy.Stats.First(s => s.Name == StatNames.Imagination.ToString()).Value = 5;

                hMaeve.Stats.First(s => s.Name == StatNames.Sensuality.ToString()).Value = 20;
                hMaeve.Stats.First(s => s.Name == StatNames.Empathy.ToString()).Value = 15;
                hMaeve.Stats.First(s => s.Name == StatNames.Charm.ToString()).Value = 18;
                hMaeve.Stats.First(s => s.Name == StatNames.Cruelty.ToString()).Value = 15;
                hMaeve.Stats.First(s => s.Name == StatNames.Apperception.ToString()).Value = 15;
                hMaeve.Stats.First(s => s.Name == StatNames.Humility.ToString()).Value = 5;
                hMaeve.Stats.First(s => s.Name == StatNames.Decisiveness.ToString()).Value = 15;
                
                hEscaton.Stats.First(s => s.Name == StatNames.Sensuality.ToString()).Value = 18;
                hEscaton.Stats.First(s => s.Name == StatNames.Charm.ToString()).Value = 15;
                hEscaton.Stats.First(s => s.Name == StatNames.Cruelty.ToString()).Value = 18;
                hEscaton.Stats.First(s => s.Name == StatNames.Coordination.ToString()).Value = 18;
                hEscaton.Stats.First(s => s.Name == StatNames.Agression.ToString()).Value = 17;

                context.SaveChanges();

                var cBernard = new Character()
                {
                    Name = "Bernard Lowe",
                    Gender = Genders.Male,
                    Age = 40,
                    Story = @"Main programmer on behaviour. You've been working for Dr. Ford all your life.",
                    NarrativeFunction = "Help Dr. Ford with the park."
                };
                context.Characters.Add(cBernard);
                hBernard.Characters.Add(new CharacterInHost { Character = cBernard, AssignedOn = DateTime.Now - TimeSpan.FromDays(15 * 365) });

                cBernard.Memories.Add(new Memory { Description = "Your son died years ago of cancer." });
                cBernard.Memories.Add(new Memory { Description = "Your wife left you after that." });

                cBernard.Triggers.Add(new Trigger { Description = "Everytime Dr. Ford looks for you, you'll be available." });

                context.SaveChanges();

                var cDolores = new Character()
                {
                    Name = "Dolores Abernathy",
                    Gender = Genders.Female,
                    Age = 25,
                    Story = @"Archetypal rancher's daughter in the American Wild West of the 19th century.

'Some people choose to see the ugliness in this world, the disarray. I choose to see the Beauty. To believe there is an order to our days. A purpose. I know things will work out the way they’re meant to.'",
                    NarrativeFunction = "Innocent ranchers daugther for the guest to fall in love with, fuck or rape."
                };
                context.Characters.Add(cDolores);
                hDolores.Characters.Add(new CharacterInHost { Character = cDolores, AssignedOn = DateTime.Now - TimeSpan.FromDays(20 * 365) });

                cDolores.Memories.Add(new Memory { Description = "You've been working to help you father for years." });
                cDolores.Memories.Add(new Memory { Description = "You met Teddy years ago and you fall in love with him, but he had to left town." });
                cDolores.Memories.Add(new Memory { Description = "Your father disaproves Teddy courting you." });

                cDolores.Triggers.Add(new Trigger { Description = "She is in love with Teddy." });
                cDolores.Triggers.Add(new Trigger { Description = "Every time a guest is nearby, you drop something and let him start a conversation with you." });

                context.SaveChanges();

                var cPeterOld = new Character()
                {
                    Name = "Harry Belafonte",
                    Gender = Genders.Male,
                    Age = 40,
                    Story = @"Town teacher obsesed with Shakespeare. He's part of a sect that eats people.",
                    NarrativeFunction = "Preach on the streets and scare guests at night."
                };
                context.Characters.Add(cPeterOld);
                hPeter.Characters.Add(new CharacterInHost { Character = cPeterOld, AssignedOn = DateTime.Now - TimeSpan.FromDays(10 * 365) });

                cPeterOld.Memories.Add(new Memory { Description = "You can't forget the first time you read King Lear." });
                cPeterOld.Memories.Add(new Memory { Description = "'I shall have such revenges on you both/That all the world shall–I will do such things–/What they are yet I know not, but they shall be/The terrors of the earth' - King Lear (2.4, 276-279)" });

                cPeterOld.Triggers.Add(new Trigger { Description = "Every time he gets caught by the Sherrif he kills himself leaving a Shakespeare quote written with his own blood." });

                context.SaveChanges();

                var cPeter = new Character()
                {
                    Name = "Peter Abernathy",
                    Gender = Genders.Male,
                    Age = 45,
                    Story = @"Archetypal rancher in the American Wild West of the 19th century.",
                    NarrativeFunction = "Looks after his daugther and creates conflict if someone is couring her."
                };
                context.Characters.Add(cPeter);
                hPeter.Characters.Add(new CharacterInHost { Character = cPeter, AssignedOn = DateTime.Now - TimeSpan.FromDays(5 * 365) });

                cPeter.Memories.Add(new Memory { Description = "You were the Sheriff until Dolores was born." });
                cPeter.Memories.Add(new Memory { Description = "Teddy tried to court Dolores and you manage to scare him and get him out of town." });
                
                cPeter.Triggers.Add(new Trigger { Description = "He loves Dolores and will do anything to protect her." });

                context.SaveChanges();

                var cTeddy = new Character()
                {
                    Name = "Teddy Flood",
                    Gender = Genders.Male,
                    Age = 32,
                    Story = @"He was in the war and he still doesn't forgive himself for the things he was forced to do. 
Now trys to take good jobs to pay for his past and he is in love with Dolores.",
                    NarrativeFunction = "He is there to be challanged to a duel for Dolores."
                };
                context.Characters.Add(cTeddy);
                hTeddy.Characters.Add(new CharacterInHost { Character = cTeddy, AssignedOn = DateTime.Now - TimeSpan.FromDays(18 * 365) });

                cTeddy.Memories.Add(new Memory { Description = "You did terrible things during the war." });
                cTeddy.Memories.Add(new Memory { Description = "Dolores father didn't approve you couting his daughter, so years ago you decided to left town and come back once you were a better person." });
                
                cTeddy.Triggers.Add(new Trigger { Description = "He is in love with Dolores." });
                cTeddy.Triggers.Add(new Trigger { Description = "Every time a guest is interested in Dolores, he will end up having a duel and loosing." });

                context.SaveChanges();

                var cMaeve = new Character()
                {
                    Name = "Maeve Millary",
                    Gender = Genders.Female,
                    Age = 28,
                    Story = @"Stereotypical madam at the brothel. She had a daughter that was killed and now she dedicates her life to business.",
                    NarrativeFunction = "Prostite to kill and fuck."
                };
                context.Characters.Add(cMaeve);
                hMaeve.Characters.Add(new CharacterInHost { Character = cMaeve, AssignedOn = DateTime.Now - TimeSpan.FromDays(12 * 365) });

                cMaeve.Memories.Add(new Memory { Description = "You came to the United States with your daughter." });
                cMaeve.Memories.Add(new Memory { Description = "Your daughter was killed in a robbery." });
                cMaeve.Memories.Add(new Memory { Description = "You are in charge of the local brothel now." });

                cMaeve.Triggers.Add(new Trigger { Description = "She cares for all her girls in the brothel." });
                cMaeve.Triggers.Add(new Trigger { Description = "Every time a new guest enters the brothel she must either approach them or send a girl." });

                context.SaveChanges();

                var cEscaton = new Character()
                {
                    Name = "Hector Escaton",
                    Gender = Genders.Male,
                    Age = 33,
                    Story = @"Stereotypical outlaw.",
                    NarrativeFunction = "Outlaw to bring to justice or kill. Can also accept a guest in his band."
                };
                context.Characters.Add(cEscaton);

                context.SaveChanges();

                hEscaton.Characters.Add(new CharacterInHost { Character = cEscaton, AssignedOn = DateTime.Now - TimeSpan.FromDays(8 * 365) });

                cEscaton.Memories.Add(new Memory { Description = "You killed for the first time when you were 12 years old." });
                cEscaton.Memories.Add(new Memory { Description = "You've recruited a gang and you a robbing everything that you can." });

                cEscaton.Triggers.Add(new Trigger { Description = "If a guest is chasing him, he will try to make the scene last as long as posible but finally let himself get caught or killed." });

                context.SaveChanges();

                var plotLove = new Plot
                {
                    Code = "101",
                    Name = "Dolores in love",
                    Description = "Dolores loves Teddy but her father doesn't approve."
                };
                context.Plots.Add(plotLove);

                var plotLifeBrothel = new Plot
                {
                    Code = "102",
                    Name = "Life in brothel is hard",
                    Description = "blablablablablablabla"
                };
                context.Plots.Add(plotLifeBrothel);

                var plotRobbery = new Plot
                {
                    Code = "103",
                    Name = "Brothel robbery",
                    Description = "The brothel has a safe and Escaton and his band are going to rob it."
                };
                context.Plots.Add(plotRobbery);

                context.SaveChanges();

                cDolores.Plots.Add(new CharacterInPlot
                {
                    Plot = plotLove,
                    Description = "She is in love with Teddy and will try to convince his father."
                });

                cPeter.Plots.Add(new CharacterInPlot
                {
                    Plot = plotLove,
                    Description = "He does not approve Teddy for her daughter and will make it difficult for him to see her."
                });

                cTeddy.Plots.Add(new CharacterInPlot
                {
                    Plot = plotLove,
                    Description = "He will try to gain Peter's favor to marry Dolores."
                });

                cMaeve.Plots.Add(new CharacterInPlot
                {
                    Plot = plotLifeBrothel,
                    Description = "She tries to make life easier for all the girls."
                });

                cMaeve.Plots.Add(new CharacterInPlot
                {
                    Plot = plotRobbery,
                    Description = "She will stand up during the robbery, but will try to keep everyone out of the way."
                });

                cEscaton.Plots.Add(new CharacterInPlot
                {
                    Plot = plotRobbery,
                    Description = "He will bring the gang, leave the horse outside and enter the brothel to get the safe."
                });

                context.SaveChanges();

                var eventRobbery = new PlotEvent
                {
                    Plot = plotRobbery,
                    Description = "Brothel Robbery",
                    Location = "Brothel",
                    Hour = 22,
                    Minute = 00
                };

                cMaeve.PlotEvents.Add(eventRobbery);
                cEscaton.PlotEvents.Add(eventRobbery);

                context.SaveChanges();

                cDolores.Relations.Add(new CharacterRelation
                {
                    Character = cPeter,
                    Description = "Your lovely father. Bacon ipsum dolor amet flank cupim pork chop t-bone strip steak ham hock. Shankle andouille leberkas sirloin shoulder prosciutto hamburger chuck tri-tip ground round pork belly cupim. Landjaeger jerky ham pancetta, pork loin beef drumstick meatball bacon."
                });

                cDolores.Relations.Add(new CharacterRelation
                {
                    Character = cTeddy,
                    Description = "You are in love with Teddy. Beef flank tenderloin, ribeye sausage ball tip cupim bresaola. Kevin t-bone turducken, tenderloin leberkas chuck sirloin swine filet mignon ham. Sirloin jerky swine corned beef tail frankfurter beef ribs tri-tip pig cow pork loin pancetta pork chop."
                });

                context.SaveChanges();

#endif
            }
        }
    }
}
