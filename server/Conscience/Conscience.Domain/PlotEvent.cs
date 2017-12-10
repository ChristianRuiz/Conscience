using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class PlotEvent : IdentityEntity
    {
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

        public string Location
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

        public override string ToString()
        {
            return Description;
        }
    }
}
