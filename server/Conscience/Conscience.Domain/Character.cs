using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class Character : IdentityEntity
    {
        public Character()
        {
            Memories = new HashSet<Memory>();
            Triggers = new HashSet<Trigger>();
            Plots = new HashSet<CharacterInPlot>();
            Relations = new HashSet<CharacterRelation>();
        }
        
        public string Name
        {
            get;
            set;
        }
        
        public int Age
        {
            get;
            set;
        }

        public Genders Gender
        {
            get;
            set;
        }

        public string Story
        {
            get;
            set;
        }

        public string NarrativeFunction
        {
            get;
            set;
        }

        public virtual CharacterInHost CurrentHost
        {
            get
            {
                var host = Hosts.OrderByDescending(h => h.AssignedOn).FirstOrDefault();
                if (host != null && host.Host.CurrentCharacter.Character == this)
                    return host;
                else
                    return null;
            }
        }

        public virtual ICollection<CharacterInHost> Hosts
        {
            get;
            set;
        }

        public virtual ICollection<Memory> Memories
        {
            get;
            set;
        }
        
        public virtual ICollection<Trigger> Triggers
        {
            get;
            set;
        }

        public virtual ICollection<CharacterInPlot> Plots
        {
            get;
            set;
        }

        public virtual ICollection<CharacterRelation> Relations
        {
            get;
            set;
        }
    }

    public enum Genders
    {
        Male,
        Female,
        NonGender
    }
}
