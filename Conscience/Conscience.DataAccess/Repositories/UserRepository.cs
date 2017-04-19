using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<User> DbSet
        {
            get
            {
                return _context.Users;
            }
        }

        public IQueryable<Host> GetAllHosts(Account currentUser)
        {
            var hosts = GetAll().OfType<Host>();
            if (!currentUser.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()
                                        || r.Name == RoleTypes.CompanyAdmin.ToString()))
                hosts = hosts.Where(h => !h.Hidden);
            return hosts;
        }

        public IQueryable<Employee> GetAllEmployees()
        {
            return GetAll().OfType<Employee>();
        }

        public Employee AddEmployee(int accountId)
        {
            var account = _context.Accounts.First(a => a.Id == accountId);
            var employee = new Employee
            {
                Account = account
            };
            DbSet.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public Host AddHost(int accountId)
        {
            var account = _context.Accounts.First(a => a.Id == accountId);
            var employee = new Host
            {
                Account = account
            };
            DbSet.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public void UserDisconnected(User user)
        {
            if (user.Device != null)
            {
                user.Device.Online = false;
                _context.SaveChanges();
            }
        }

        public User GetById(int userId)
        {
            var user = DbSet.Include(u => u.Account).FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new ArgumentException("There is no user with id: " + userId);
            return user;
        }

        public void UpdateDevice(User user, string deviceId)
        {
            if (user.Device == null)
            {
                user.Device = new Device();
                _context.Devices.Add(user.Device);
            }

            user.Device.DeviceId = deviceId;
            user.Device.Online = true;
            user.Device.LastConnection = DateTime.Now;
            _context.SaveChanges();
        }

        public User GetByAccountId(int accountId)
        {
            return DbSet.FirstOrDefault(u => u.Account.Id == accountId);
        }

        public User UpdateLocations(int userId, List<Location> locations)
        {
            var user = GetById(userId);
            user.Device.Locations.AddRange(locations);
            user.Device.LastConnection = DateTime.Now;
            _context.SaveChanges();
            return user;
        }
    }
}
