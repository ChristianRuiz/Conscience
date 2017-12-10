using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class LogEntry : IdentityEntity
    {
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

        public string Description
        {
            get;
            set;
        }

        public DateTime TimeStamp
        {
            get;
            set;
        }
    }
}
