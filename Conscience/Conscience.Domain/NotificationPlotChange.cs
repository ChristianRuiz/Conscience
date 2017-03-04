using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class NotificationPlotChange : Notification
    {
        public NotificationPlotChange()
        {
            Characters = new HashSet<Character>();
        }

        public Plot Plot
        {
            get;
            set;
        }
        
        public virtual ICollection<Character> Characters
        {
            get;
            set;
        }   
    }
}
