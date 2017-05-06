using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Conscience.Mobile.Hosts.Core.Services;
using MvvmCross.Platform;

namespace Conscience.Mobile.Hosts.Android.Services
{
    [Service]
    public class HostsServiceImplementation : Service
    {
        private HostsService _service;

        public override void OnCreate()
        {
            base.OnCreate();

            _service = Mvx.Resolve<HostsService>();
        }
        
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            _service.Start();

            return StartCommandResult.StickyCompatibility;
        }

        public override void OnRebind(Intent intent)
        {
            _service.Reconnect();

            base.OnRebind(intent);
        }

        public override void OnDestroy()
        {
            _service.Stop();

            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return new HostsServiceBinder(this);
        }

        public class HostsServiceBinder : Binder
        {
            HostsServiceImplementation service;

            public HostsServiceBinder(HostsServiceImplementation service)
            {
                this.service = service;
            }

            public HostsServiceImplementation GetHostsService()
            {
                return service;
            }
        }
    }
}