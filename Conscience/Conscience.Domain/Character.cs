using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Character
    {
        public Character()
        {
            Memories = new HashSet<Memory>();
            Triggers = new HashSet<Trigger>();
            Plots = new HashSet<CharacterInPlot>();
            PlotEvents = new HashSet<PlotEvent>();
        }

        public int Id
        {
            get;
            set;
        }

        public int Description
        {
            get;
            set;
        }

        public virtual ICollection<Memory> Memories
        {
            get;
            set;
        }

        public virtual ICollection<Trigger> Triggers
        {
            get;
            set;
        }

        public virtual ICollection<CharacterInPlot> Plots
        {
            get;
            set;
        }

        public virtual ICollection<PlotEvent> PlotEvents
        {
            get;
            set;
        }
    }
}
