using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Roles
{
    public class RoleUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
    }

    public class RoleUpdateHandler : RepoHandler<Role>, IRequestHandler<RoleUpdateRequest, CommandResponse>
    {
        public RoleUpdateHandler(IRepo<Role> repo) : base(repo)
        {
        }

        public async Task<CommandResponse> Handle(RoleUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_repo.Exists(r => r.Id != request.Id && r.Name == request.Name))
                return Error("Role with the same name exists!");
            var role = _repo.GetItem(request.Id);
            if (role is null)
                return Error("Role not found!");
            role.Name = request.Name?.Trim();
            await _repo.Update(role);
            return Success("Role updated successfully.", role.Id);
        }
    }
}
