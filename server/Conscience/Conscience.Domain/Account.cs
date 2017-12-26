using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Account : IdentityEntity
    {
        public Account()
        {
            Roles = new HashSet<Role>();
        }
        
        public string Email
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string PasswordHash
        {
            get;
            set;
        }

        public string PictureUrl
        {
            get;
            set;
        }

        public virtual Host Host
        {
            get;
            set;
        }

        public virtual Employee Employee
        {
            get;
            set;
        }

        public virtual Device Device
        {
            get;
            set;
        }

        public virtual ICollection<Role> Roles
        {
            get;
            set;
        }
    }
}
