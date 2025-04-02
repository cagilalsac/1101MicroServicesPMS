using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Users.Features.Roles
{
    public class RoleDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class RoleDeleteHandler : RepoHandler<Role>, IRequestHandler<RoleDeleteRequest, CommandResponse>
    {
        public RoleDeleteHandler(IRepo<Role> repo) : base(repo)
        {
        }

        public async Task<CommandResponse> Handle(RoleDeleteRequest request, CancellationToken cancellationToken)
        {
            var role = _repo.Query().Include(r => r.Users).SingleOrDefault(r => r.Id == request.Id);
            if (role is null)
                return Error("Role not found!");
            if (role.Users.Count > 0)
                return Error("Role can't be deleted becuase it has relational users!");
            await _repo.Delete(role.Id);
            return Success("Role deleted successfully", role.Id);
        }
    }
}
