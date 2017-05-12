using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class CharacterRelation
    {
        public int Id
        {
            get;
            set;
        }

        public virtual Character Character
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
