using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingDAL.Repositories
{

    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        bool Add(T entity);
        //make Generic T class inhert from Base Table when i am extend Project
        //bool Add(T entity, out int Id);
        //bool ChangeStatus(int ID, string userId, int status = 1);
        bool Update(T entity);
        bool Delete(T entity);
        bool Save();
        public IQueryable<T> GetAllAsQueryable();
        T GetFirst(Expression<Func<T, bool>> filterExpression);
        List<T> GetList(Expression<Func<T, bool>> filterExpression);
    }


}
