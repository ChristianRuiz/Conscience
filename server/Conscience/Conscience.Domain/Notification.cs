using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Notification : IdentityEntity
    {
        public DateTime TimeStamp
        {
            get;
            set;
        }

        public bool Processed
        {
            get;
            set;
        }
    }
}
