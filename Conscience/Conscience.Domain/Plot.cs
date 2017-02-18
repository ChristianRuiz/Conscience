using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Plot
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

        public List<CharacterInPlot> Characters
        {
            get;
            set;
        }
    }
}
