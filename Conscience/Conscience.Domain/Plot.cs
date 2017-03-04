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
