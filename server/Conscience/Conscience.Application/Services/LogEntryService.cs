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
        private readonly EmployeeRepository _employeesRepo;
        private readonly IUsersIdentityService _userService;

        public LogEntryService(LogEntryRepository logRepo, IUsersIdentityService userService, EmployeeRepository employeesRepo)
        {
            _logRepo = logRepo;
            _userService = userService;
            _employeesRepo = employeesRepo;
        }

        public void Log(string text)
        {
            Log(null, text);
        }

        public void Log(Host host, string text)
        {
            var employee = _userService.CurrentUser.Employee != null ? _employeesRepo.GetById(_userService.CurrentUser.Employee.Id) : null;
            _logRepo.Add(host, employee, text);
        }
    }
}
