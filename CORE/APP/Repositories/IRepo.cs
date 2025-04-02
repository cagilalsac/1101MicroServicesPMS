using CORE.APP.Domain;
using System.Linq.Expressions;

namespace CORE.APP.Repositories
{
    public interface IRepo<TEntity> : IDisposable where TEntity : Entity, new()
    {
        public IQueryable<TEntity> Query(bool noTracking = true);
        public Task Create(TEntity entity, bool save = true);
        public Task Update(TEntity entity, bool save = true);
        public Task Delete(int id, bool save = true);
        public Task Delete(Expression<Func<TEntity, bool>> predicate, bool save = false);
        public Task<int> Save(CancellationToken cancellationToken = default);

        public List<TEntity> GetList()
        {
            return Query().ToList();
        }

        public TEntity GetItem(int id)
        {
            return Query().SingleOrDefault(entity => entity.Id == id);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return Query().Any(predicate);
        }
    }
}
