using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class LogEntry : IdentityEntity
    {
        public Host Host
        {
            get;
            set;
        }

        public Employee Employee
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }
    }
}
