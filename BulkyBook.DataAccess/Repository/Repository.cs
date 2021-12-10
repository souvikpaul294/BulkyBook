using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(string? includeColumns = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeColumns))
            {
                foreach (var column in includeColumns.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(column);//Include() is used to get the corresponding data from the foreign table and it works only on IQueryable objects
                }
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeColumns = null)
        {
            IQueryable<T> query = dbSet;
            if (!string.IsNullOrEmpty(includeColumns))
            {
                foreach (var column in includeColumns.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(column);//Include() is used to get the corresponding data from the foreign table and it works only on IQueryable objects
                }
            }
            return query.FirstOrDefault(filter);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
