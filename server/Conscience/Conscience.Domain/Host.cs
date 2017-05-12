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
            Stats = new HashSet<Stats>();
            foreach (var statName in Enum.GetNames(typeof(StatNames)))
                Stats.Add(new Stats { Name = statName, Value = 10 });
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
                return Characters.OrderByDescending(c => c.AssignedOn).FirstOrDefault();
            }
        }

        public virtual CoreMemory CoreMemory1
        {
            get;
            set;
        }

        public virtual CoreMemory CoreMemory2
        {
            get;
            set;
        }

        public virtual CoreMemory CoreMemory3
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
