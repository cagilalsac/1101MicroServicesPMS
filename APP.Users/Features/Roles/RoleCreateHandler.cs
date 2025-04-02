using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Roles
{
    public class RoleCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
    }

    public class RoleCreateHandler : RepoHandler<Role>, IRequestHandler<RoleCreateRequest, CommandResponse>
    {
        public RoleCreateHandler(IRepo<Role> repo) : base(repo)
        {
        }

        public async Task<CommandResponse> Handle(RoleCreateRequest request, CancellationToken cancellationToken)
        {
            if (_repo.Exists(r => r.Name == request.Name))
                return Error("Role with the same name exists!");
            var role = new Role()
            {
                Name = request.Name?.Trim()
            };
            await _repo.Create(role);
            return Success("Role created successfully.", role.Id);
        }
    }
}
