using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Host
    {
        public Host()
        {
            Notifications = new HashSet<Notification>();
            Characters = new HashSet<CharacterInHost>();
            CoreMemories = new HashSet<CoreMemory>();
            Stats = new HashSet<Stats>();
        }

        public int Id
        {
            get;
            set;
        }

        public virtual ICollection<Notification> Notifications
        {
            get;
            set;
        }

        public virtual Account Account
        {
            get;
            set;
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
