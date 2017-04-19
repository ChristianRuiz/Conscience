using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.DataAccess
{
    public class ConscienceDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConscienceContext>
    {
        public override void InitializeDatabase(ConscienceContext context)
        {
            if (!context.Roles.Any())
            {
                base.InitializeDatabase(context);
                Seed(context);
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

                var ford = new ConscienceAccount
                {
                    UserName = "ford",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                ford.Roles.Add(companyAdminRole);
                context.Accounts.Add(ford);

                var theresa = new ConscienceAccount
                {
                    UserName = "theresa",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                theresa.Roles.Add(companyQARole);
                context.Accounts.Add(theresa);

                var elsie = new ConscienceAccount
                {
                    UserName = "elsie",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                elsie.Roles.Add(companyBehaviourRole);
                context.Accounts.Add(elsie);

                var bernard = new ConscienceAccount
                {
                    UserName = "bernard",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                bernard.Roles.Add(companyBehaviourRole);
                bernard.Roles.Add(hostRole);
                context.Accounts.Add(bernard);

                var sizemore = new ConscienceAccount
                {
                    UserName = "sizemore",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                sizemore.Roles.Add(companyPlotRole);
                context.Accounts.Add(sizemore);

                var dolores = new ConscienceAccount
                {
                    UserName = "dolores",
                    PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
                };
                dolores.Roles.Add(hostRole);
                context.Accounts.Add(dolores);

                context.SaveChanges();
                
                context.Users.Add(new Employee { Account = ford });
                context.Users.Add(new Employee { Account = theresa });
                context.Users.Add(new Employee { Account = elsie });
                context.Users.Add(new Employee { Account = bernard });
                context.Users.Add(new Employee { Account = sizemore });

                context.Users.Add(new Host { Account = arnold, Hidden = true });
                context.Users.Add(new Host { Account = bernard, Hidden = true });
                context.Users.Add(new Host { Account = dolores });

                context.SaveChanges();
            }
        }
    }
}
