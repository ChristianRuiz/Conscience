using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Audio
    {
        public int Id
        {
            get;
            set;
        }

        public bool IsEmbeded
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }
    }
}
