using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class NotificationPlotChange : Notification
    {
        public Plot Plot
        {
            get;
            set;
        }
        
        public List<Character> Characters
        {
            get;
            set;
        }   
    }
}
