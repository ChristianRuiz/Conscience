﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Domain
{
    public class NotificationAudio : Notification
    {
        public Audio Audio
        {
            get;
            set;
        }
    }
}
