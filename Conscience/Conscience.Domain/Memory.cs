using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Memory
    {
        public int Id
        {
            get;
            set;
        }

        public int Name
        {
            get;
            set;
        }

        public int Description
        {
            get;
            set;
        }

        public Audio Audio
        {
            get;
            set;
        }
    }
}
