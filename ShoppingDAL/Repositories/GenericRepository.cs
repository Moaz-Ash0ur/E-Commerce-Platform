using Humanizer;
using Microsoft.EntityFrameworkCore;
using ShoppingDAL.Data;
using System.Linq.Expressions;

namespace ShoppingDAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ShoppingDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ShoppingDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T? GetById(int id) => _dbSet.Find(id);

        public bool Add(T entity)
        {
            _dbSet.Add(entity);
            return Save();          
        }

        public bool Update(T entity)
        {
            _dbSet.Update(entity);
            return Save();
        }

        public bool Delete(T entity)
        {
            _dbSet.Remove(entity);
            return Save();
        }

        public IQueryable<T> GetAllAsQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public T GetFirst(Expression<Func<T, bool>> filterExpression)
        {
            try
            {
                var entity = _dbSet.Where(filterExpression).AsNoTracking().FirstOrDefault();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> GetList(Expression<Func<T, bool>> filterExpression)
        {
            try
            {
                var entity = _dbSet.Where(filterExpression).AsNoTracking().ToList();
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Save()
        {
           return _context.SaveChanges() > 0;
        }
    }





}
