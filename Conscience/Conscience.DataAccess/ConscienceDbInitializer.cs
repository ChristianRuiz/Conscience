using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concience.DataAccess
{
    public class ConscienceDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConscienceContext>
    {
        protected override void Seed(ConscienceContext context)
        {
            var adminRole = new Role { Name = RoleTypes.Admin.ToString() };
            context.Roles.Add(adminRole);
            context.Roles.Add(new Role { Name = RoleTypes.CompanyAdmin.ToString() });
            context.Roles.Add(new Role { Name = RoleTypes.CompanyQA.ToString() });
            context.Roles.Add(new Role { Name = RoleTypes.CompanyBehaviour.ToString() });
            context.Roles.Add(new Role { Name = RoleTypes.CompanyPlot.ToString() });
            context.Roles.Add(new Role { Name = RoleTypes.CompanyMaintenance.ToString() });
            context.Roles.Add(new Role { Name = RoleTypes.Host.ToString() });
            context.SaveChanges();

            var arnold = new ConscienceAccount
            {
                UserName = "arnold",
                PasswordHash = "AA+JyvawjlityY0fyaRSr1qZkCgaIvU+bqTe6uShANUqcoG0EH2F6BcXq7hwnN7N1Q=="
            };

            arnold.Roles.Add(adminRole);
            context.Accounts.Add(arnold);
            context.SaveChanges();
        }
    }
}
