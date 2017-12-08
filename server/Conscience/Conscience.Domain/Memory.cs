using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Memory : IdentityEntity
    {
        public Character Character
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
