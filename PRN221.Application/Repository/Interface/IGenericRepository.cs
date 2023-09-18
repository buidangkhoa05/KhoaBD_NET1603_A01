using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace PRN221.Application.Repository.Interface
{
    public interface IGenericRepository<T>
    {
        int SaveChanges();
        void Create(T entity);
        void Create(IEnumerable<T> entities);
        int Delete(Func<T, bool> filter);
        void Update(T entity);
        int Update(Expression<Func<T, bool>>? predicate, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls);
        IEnumerable<T> GetWithCondition(Expression<Func<T, T>> selector, Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includes);
    }
}
