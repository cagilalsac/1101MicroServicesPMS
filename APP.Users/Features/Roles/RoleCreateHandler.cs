using APP.Users.Domain;
using CORE.APP.Features;
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

    public class RoleCreateHandler : UsersDbHandler, IRequestHandler<RoleCreateRequest, CommandResponse>
    {
        public RoleCreateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Roles.Any(r => r.Name == request.Name))
                return Error("Role with the same name exists!");
            var role = new Role()
            {
                Name = request.Name
            };
            _db.Roles.Add(role);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Role created successfully.", role.Id);
        }
    }
}
