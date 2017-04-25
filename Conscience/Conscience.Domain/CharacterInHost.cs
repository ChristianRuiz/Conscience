using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class CharacterInHost
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

        public DateTime AssignedOn
        {
            get;
            set;
        }
    }
}
