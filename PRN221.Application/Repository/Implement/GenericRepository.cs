﻿
using LinqKit;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PRN221.Application.Repository.Implement
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DbContext dbContext;
        protected readonly DbSet<T> dbSet;
        public GenericRepository(FucarRentingManagementContext context)
        {
            dbContext = context;
            dbSet = context.Set<T>();
        }

        #region Create
        /// <summary>
        /// Add an entity to DbSet, need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Create(T entity)
        {
            dbSet.Add(entity);
        }
        /// <summary>
        /// Add a list of entities to DbSet, need to call SaveChanges to save to database
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public void Create(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Delete immediately by condition filter without SaveChanges action 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> filter)
        {
            var query = dbSet.AsQueryable();
            query = query.Where(filter);
            return query.ExecuteDelete();
        }
        /// <summary>
        /// Update IsDeleted to true by condition filter without SaveChanges action
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public async Task SoftDeleteAsync(Func<T, bool> filter)
        //{
        //    //T _entity = await GetByIdAsync(id);
        //    //if (_entity != null)
        //    //{
        //    //    _entity.is_deleted = true;
        //    //    await UpdateAsync(_entity);
        //    //}

        //    var result = await dbSet.Where(filter)
        //                            .AsQueryable()
        //                            .ExecuteUpdateAsync(setter => setter.SetProperty(e => e.IsDeleted, false));
        //}
        #endregion Delete
        #region Update
        /// <summary>
        ///  Change stated of entity to Modified (mark this entity will update), need to call SaveChanges to save to database
        ///  
        /// Update(t => t.Name == "ABC", setter => setter.SetProperty(i => i.Name, "CCC")
        ///                                                  .SetProperty(i => i.Age, "18"))
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            dbContext.Attach(entity).State = EntityState.Modified;
        }
        /// <summary>
        /// reposiopry.Update(a => a.Name = "ABC", setter => setter.SetProperty(i => i.Age, 18).SetProperty(i => i.Name = "CCC"))
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="setPropertyCalls"></param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>>? filter, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setPropertyCalls)
        {
            var query = dbSet.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);

            }
            return query.ExecuteUpdate(setPropertyCalls);
        }
        #endregion Update

        #region Retrieve
        /// <summary>
        /// Get an entity is active by id and match orther condition filter, this function is AsNoTracking 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="predicate">can null</param>
        /// <param name="includes"></param>
        /// <returns></returns>
        //public async Task<T> GetById(Guid id, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes)
        //{
        //    Expression<Func<T, bool>> isNotDeleteCondition = p => p.IsDeleted == false && p.Id == id;

        //    if (filter == null)
        //    {
        //        filter = isNotDeleteCondition;
        //    }
        //    else
        //    {
        //        filter = PredicateBuilder.And(isNotDeleteCondition, filter);
        //    }

        //    if (includes == null)
        //    {
        //        return await dbSet.AsNoTracking().Where(filter).SingleOrDefaultAsync();
        //    }
        //    else
        //    {
        //        var query = dbSet.AsNoTracking().Where(filter);
        //        return await query.Includes(includes).SingleOrDefaultAsync();
        //    }
        //}
        /// <summary>
        /// Get all entities are active and match condition filter, this function is AsNoTracking
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public IEnumerable<T> GetWithCondition(Expression<Func<T, T>> selector, bool isTracking = true, Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[]? includes)
        {
            //Expression<Func<T, bool>> isNotDeleteCondition = p => p.IsDeleted == false;
            var query = dbSet.AsQueryable();

            if (isTracking)
            {
                query = query.AsNoTracking();
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includes == null)
            {
                return query.Select(selector).ToList();
            }
            else
            {
                return query.Includes(includes).Select(selector).ToList();
            }
        }

        //public async Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams)
        //{
        //    PagedList<T> pagedRequests = new PagedList<T>();
        //    if (pagingParams.PageSize <= 0 || pagingParams.PageNumber <= 0)
        //    {
        //        throw new ArgumentException("Page number or page size must be greater than 0");
        //    }
        //    else
        //    {
        //        await pagedRequests.LoadData(dataQuery.Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreatedAt), pagingParams.PageNumber, pagingParams.PageSize);
        //    }

        //    return pagedRequests;

        //}

        //public async Task<PagedList<T>> GetWithPaging(IQueryable<T> dataQuery, QueryParameters pagingParams, Expression<Func<T, bool>> filter)
        //{
        //    PagedList<T> pagedRequests = new PagedList<T>();

        //    if (pagingParams.PageSize <= 0 || pagingParams.PageNumber <= 0)
        //    {
        //        throw new ArgumentException("Page number or page size must be greater than 0");
        //    }
        //    else
        //    {
        //        await pagedRequests.LoadData(dataQuery.Where(c => c.IsDeleted == false).OrderByDescending(c => c.CreatedAt), pagingParams.PageNumber, pagingParams.PageSize, filter);
        //    }
        //    return pagedRequests;
        //}
        #endregion Retrieve
        /// <summary>
        /// Function save changes to database (Excute command to Db like: update, create, delete)
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return dbContext.SaveChanges();
        }

        //public async Task<IList<T>> WhereAsync(Expression<Func<T, bool>> filter, params string[] navigationProperties)
        //{
        //    List<T> list;
        //    var query = dbSet.AsQueryable();
        //    foreach (var property in navigationProperties)
        //    {
        //        query = query.Include(property);
        //    }
        //    list = await query.Where(filter).AsNoTracking().ToListAsync();
        //    return list;
    }
}
