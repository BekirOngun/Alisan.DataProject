using System;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace IKYS.Dal.Infrastructure
{
    public abstract class BaseRepository<T, K>
        where T : ObjectContext
        where K : class
    {
        protected T _context;

        protected ObjectSet<K> _objectSet;

        public BaseRepository(T context)
        {
            _context = context ?? throw new ArgumentException("Entity container can not be null");
            _objectSet = _context.CreateObjectSet<K>();
        }

        #region BaseMethods

        public K[] GetAll()
        {
            return _objectSet.ToArray();
        }

        public virtual void Insert(K entityToAdd)
        {
            _objectSet.AddObject(entityToAdd);
        }

        public void Attach(K entityToAdd)
        {
            _objectSet.Attach(entityToAdd);
        }

        public K[] FindBy(Expression<Func<K, bool>> predicate)
        {
            return _objectSet.Where(predicate).ToArray();
        }

        public IQueryable<K> FindByQuery(Expression<Func<K, bool>> predicate)
        {
            return _objectSet.Where(predicate);
        }

        public K FindByFirst(Expression<Func<K, bool>> predicate)
        {
            return _objectSet.Where(predicate).FirstOrDefault();
        }

        public bool Exists(Expression<Func<K, bool>> predicate)
        {
            return FindByQuery(predicate).Any();
        }

        public int Count(Expression<Func<K, bool>> predicate)
        {
            return FindByQuery(predicate).Count();
        }

        public int TotalPage(Expression<Func<K, bool>> predicate, int PageSize)
        {
            return FindByQuery(predicate).Count() / PageSize;
        }

        public int Sum(Expression<Func<K, bool>> predicate, Expression<Func<K, int>> selector)
        {
            return FindByQuery(predicate).Sum(selector);
        }

        public void Delete(K entity)
        {
            //_objectSet.Attach(entity);

            _context.ObjectStateManager.ChangeObjectState(entity, EntityState.Deleted);
            _context.DeleteObject(entity);
        }

        public string ToTraceString(IQueryable query)
        {
            var objectQuery = (ObjectQuery)query;

            if (objectQuery != null)
            {
                return objectQuery.ToTraceString();
            }

            return null;
        }

        #endregion

    }
}
