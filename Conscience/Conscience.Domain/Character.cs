using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Character
    {
        public int Id
        {
            get;
            set;
        }

        public List<Memory> Memories
        {
            get;
            set;
        }

        public List<Trigger> Triggers
        {
            get;
            set;
        }

        public List<CharacterInPlot> Plots
        {
            get;
            set;
        }

        public List<PlotEvent> PlotEvents
        {
            get;
            set;
        }
    }
}
