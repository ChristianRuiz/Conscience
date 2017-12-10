using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class CharacterRelation : IdentityEntity
    {
        public virtual Character ParentCharacter
        {
            get;
            set;
        }

        public int ParentCharacterId
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

        public override string ToString()
        {
            return Character != null ? Character.Name : Description;
        }
    }
}
