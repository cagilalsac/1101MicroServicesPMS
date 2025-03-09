using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Users
{
    public class UserCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Password { get; set; }

        public bool IsActive { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        public DateTime? RegistrationDate { get; set; }

        public int RoleId { get; set; }

        public List<int> SkillIds { get; set; }
    }

    public class UserCreateHandler : UsersDbHandler, IRequestHandler<UserCreateRequest, CommandResponse>
    {
        public UserCreateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(UserCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Users.Any(u => u.UserName == request.UserName || (u.Name == request.Name && u.Surname == request.Surname)))
                return Error("User with the same user name or full name exists!");
            var user = new User()
            {
                IsActive = request.IsActive,
                Name = request.Name?.Trim(),
                Password = request.Password,
                RoleId = request.RoleId,
                Surname = request.Surname?.Trim(),
                UserName = request.UserName,
                RegistrationDate = request.RegistrationDate,
                SkillIds = request.SkillIds
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("User created successfully.", user.Id);
        }
    }
}
