using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Mobile.Hosts.Core.Services
{
    public class AppState
    {
#if DEBUG
        public readonly string ServerUrl = "http://192.168.1.13/";
#else
        public readonly string ServerUrl = "http://consciencelarp.azurewebsites.net/";
#endif

        public AppState()
        {
            CookieContainer = new CookieContainer();
        }

        public CookieContainer CookieContainer
        {
            get;
        }

        public Account CurrentUser
        {
            get;
            set;
        }
    }
}
