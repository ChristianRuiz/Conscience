using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class CharacterRelation : IdentityEntity
    {
        public virtual ICollection<Character> Characters
        {
            get;
            set;
        }

        //Hack: Using a collection to avoid EF exception referencing the same table on a relationship
        public Character Character
        {
            get
            {
                if (Characters == null)
                    return null;
                return Characters.FirstOrDefault();
            }
            set
            {
                if (Characters == null)
                    Characters = new HashSet<Character>();
                Characters.Clear();
                Characters.Add(value);
            }
        }

        public string Description
        {
            get;
            set;
        }
    }
}
