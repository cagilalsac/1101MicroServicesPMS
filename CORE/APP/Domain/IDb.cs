using Microsoft.EntityFrameworkCore;

namespace CORE.APP.Domain
{
    public interface IDb : IDisposable
    {
        public DbSet<TEntity> Set<TEntity>() where TEntity : class;
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
