using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;


namespace PRN221.Application.Repository.Interface
{
    public interface IGenericRepository<T>
    {
        int SaveChanges();
        void Create(T entity);
        void Create(IEnumerable<T> entities);
        int Delete(Expression<Func<T, bool>> filter);
        int Update(Expression<Func<T, bool>>? predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
        void Update(T entity);
        IEnumerable<T> GetWithCondition(Expression<Func<T, T>> selector, bool isTracking = true, Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includes);
    }
}
