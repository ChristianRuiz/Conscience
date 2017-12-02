using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository where T : class
    {
        protected ConscienceContext _context;

        public BaseRepository(ConscienceContext context)
        {
            _context = context;
        }

        protected abstract IDbSet<T> DbSet
        {
            get;
        }

        public IQueryable<T> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public T Add(T entity)
        {
            DbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public T Modify(T entity)
        {
            _context.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
