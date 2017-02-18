using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Host : User
    {
        public List<CharacterInHost> Characters
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

        public List<CoreMemory> CoreMemories
        {
            get;
            set;
        }
    }
}
