using Conscience.Mobile.Hosts.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIKit;

namespace Conscience.Mobile.Hosts.iPhone.Services
{
    public class HostsServiceImplementation
    {
        private HostsService _service;
        private nint _taskId;

        public HostsServiceImplementation(HostsService service)
        {
            _service = service;
            Start();
        }

        private async void Start()
        {
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("HostsService", OnExpiration);

            _service.Start();

            await Task.Run(() =>
            {
                Thread.Sleep(int.MaxValue);
            });

            OnExpiration();
        }

        private void OnExpiration()
        {
            _service.Stop();
            Start();
        }
    }
}
