using CORE.APP.Domain;
using CORE.APP.Repositories;
using System.Globalization;

namespace CORE.APP.Features
{
    public abstract class RepoHandler<TEntity> : Handler, IDisposable where TEntity : Entity, new()
    {
        protected readonly IRepo<TEntity> _repo;

        protected RepoHandler(IRepo<TEntity> repo, CultureInfo cultureInfo = default) : base(cultureInfo ?? new CultureInfo("en-US"))
        {
            _repo = repo;
        }

        public void Dispose()
        {
            _repo.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
