using CORE.APP.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CORE.APP.Repositories
{
    public class Repo<TEntity> : IRepo<TEntity> where TEntity : Entity, new()
    {
        private readonly IDb _db;

        public Repo(IDb db)
        {
            _db = db;
        }

        public virtual IQueryable<TEntity> Query(bool noTracking = true)
        {
            return noTracking ? _db.Set<TEntity>().AsNoTracking() : _db.Set<TEntity>();
        }

        public virtual async Task Create(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Add(entity);
            if (save)
                await Save();
        }

        public virtual async Task Update(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                await Save();
        }

        public virtual async Task Delete(int id, bool save = true)
        {
            var entity = _db.Set<TEntity>().SingleOrDefault(entity => entity.Id == id);
            _db.Set<TEntity>().Remove(entity);
            if (save)
                await Save();
        }

        public virtual async Task Delete(Expression<Func<TEntity, bool>> predicate, bool save = false)
        {
            _db.Set<TEntity>().RemoveRange(_db.Set<TEntity>().Where(predicate));
            if (save)
                await Save();
        }

        public virtual async Task<int> Save(CancellationToken cancellationToken = default)
        {
            return await _db.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
