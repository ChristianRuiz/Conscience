using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Role : IdentityEntity
    {
        public Role()
        {
            Accounts = new HashSet<Account>();
        }
        
        public string Name
        {
            get;
            set;
        }

        public virtual ICollection<Account> Accounts
        {
            get;
            set;
        }
    }

    public enum RoleTypes
    {
        Admin,
        CompanyAdmin,
        CompanyQA,
        CompanyBehaviour,
        CompanyPlot,
        CompanyPlotEditor,
        CompanyPlotAssignHost,
        CompanyMaintenance,
        Host
    }
}
