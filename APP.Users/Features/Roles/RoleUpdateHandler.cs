using APP.Users.Domain;
using CORE.APP.Features;
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

    public class RoleUpdateHandler : UsersDbHandler, IRequestHandler<RoleUpdateRequest, CommandResponse>
    {
        public RoleUpdateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(RoleUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Roles.Any(r => r.Id != request.Id && r.Name == request.Name))
                return Error("Role with the same name exists!");
            var role = _db.Roles.Find(request.Id);
            if (role is null)
                return Error("Role not found!");
            role.Name = request.Name?.Trim();
            _db.Roles.Update(role);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Role updated successfully.", role.Id);
        }
    }
}
