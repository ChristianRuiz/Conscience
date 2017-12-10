using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class LogEntryRepository : BaseRepository<LogEntry>
    {
        public LogEntryRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<LogEntry> DbSet
        {
            get
            {
                return _context.LogEntries;
            }
        }

        public IQueryable<LogEntry> GetByEmployee(int employeeId)
        {
            return DbSet.Include(l => l.Host).Include(l => l.Employee).Where(l => l.Employee.Id == employeeId);
        }

        public IQueryable<LogEntry> GetByHost(int hostId)
        {
            return DbSet.Include(l => l.Host).Include(l => l.Employee).Where(l => l.Host.Id == hostId);
        }

        public void Add(int hostId, int employeeId, string text)
        {
            var host = _context.Hosts.First(h => h.Id == hostId);
            var employee = _context.Employees.First(e => e.Id == employeeId);
            Add(host, employee, text);
        }

        public void Add(Host host, string text)
        {
            Add(host, null, text);
        }

        public void Add(Employee employee, string text)
        {
            Add(null, employee, text);
        }

        public void Add(Host host, Employee employee, string text)
        {
            Add(new LogEntry
            {
                Host = host,
                Employee = employee != null ? _context.Employees.First(e => e.Id == employee.Id) : null,
                Description = text,
                Date = DateTime.Now
            });
        }
    }
}
