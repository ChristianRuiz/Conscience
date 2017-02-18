using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class PlotEvent
    {
        public int Id
        {
            get;
            set;
        }

        public int Hour
        {
            get;
            set;
        }

        public int Minute
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public List<Character> Characters
        {
            get;
            set;
        }
    }
}
