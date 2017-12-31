using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class CoreMemory : IdentityEntity
    {
        public virtual Audio Audio
        {
            get;
            set;
        }

        public bool Locked
        {
            get;
            set;
        }
    }
}
