using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Stats
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Value
        {
            get;
            set;
        }

        public virtual Host Host
        {
            get;
            set;
        }
    }
}
