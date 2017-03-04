using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Host : User
    {
        public Host()
        {
            Characters = new HashSet<CharacterInHost>();
            CoreMemories = new HashSet<CoreMemory>();
            Stats = new HashSet<Stats>();
        }

        public virtual ICollection<CharacterInHost> Characters
        {
            get;
            set;
        }

        public CharacterInHost CurrentCharacter
        {
            get
            {
                return Characters.Last();
            }
        }

        public virtual ICollection<CoreMemory> CoreMemories
        {
            get;
            set;
        }

        public virtual ICollection<Stats> Stats
        {
            get;
            set;
        }

        public bool Hidden
        {
            get;
            set;
        }
    }
}
