using Conscience.DataAccess.Repositories;
using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Services
{
    public class LogEntryService
    {
        private readonly LogEntryRepository _logRepo;
        private readonly IUsersIdentityService _userService;

        public LogEntryService(LogEntryRepository logRepo, IUsersIdentityService userService)
        {
            _logRepo = logRepo;
            _userService = userService;
        }

        public void Log(string text)
        {
            _logRepo.Add(null, _userService.CurrentUser.Employee, text);
        }

        public void Log(Host host, string text)
        {
            _logRepo.Add(host, _userService.CurrentUser.Employee, text);
        }
    }
}
