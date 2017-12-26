using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class CharacterInPlot : IdentityEntity
    {
        public string Description
        {
            get;
            set;
        }

        public int PlotId
        {
            get;
            set;
        }

        public virtual Plot Plot
        {
            get;
            set;
        }

        public int CharacterId
        {
            get;
            set;
        }

        public virtual Character Character
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Character.Name;
        }
    }
}
