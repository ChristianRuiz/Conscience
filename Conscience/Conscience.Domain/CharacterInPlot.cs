using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class CharacterInPlot
    {
        public int Id
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public virtual Plot Plot
        {
            get;
            set;
        }
    }
}
