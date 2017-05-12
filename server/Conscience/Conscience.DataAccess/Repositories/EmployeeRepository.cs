using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class EmployeeRepository : BaseRepository<Employee>
    {
        public EmployeeRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Employee> DbSet
        {
            get
            {
                return _context.Employees;
            }
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
        
        public Employee GetById(int userId)
        {
            var employee = DbSet.OfType<Employee>().Include(u => u.Account).FirstOrDefault(u => u.Id == userId);
            if (employee == null)
                throw new ArgumentException("There is no employee with id: " + userId);
            return employee;
        }
        
        public Employee GetByAccountId(int accountId)
        {
            return DbSet.FirstOrDefault(u => u.Account.Id == accountId);
        }
    }
}
