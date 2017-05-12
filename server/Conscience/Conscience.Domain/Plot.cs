using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Plot
    {
        public Plot()
        {
            Characters = new HashSet<CharacterInPlot>();
            Events = new HashSet<PlotEvent>();
        }

        public int Id
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public virtual ICollection<CharacterInPlot> Characters
        {
            get;
            set;
        }

        public virtual ICollection<PlotEvent> Events
        {
            get;
            set;
        }
    }
}
