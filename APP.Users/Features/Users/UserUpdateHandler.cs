using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APP.Users.Features.Users
{
    public class UserUpdateRequest : Request, IRequest<CommandResponse>
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

    public class UserUpdateHandler : RepoHandler<User>, IRequestHandler<UserUpdateRequest, CommandResponse>
    {
        private readonly IRepo<UserSkill> _userSkillRepo;

        public UserUpdateHandler(IRepo<User> repo, IRepo<UserSkill> userSkillRepo, CultureInfo cultureInfo = null) : base(repo, cultureInfo)
        {
            _userSkillRepo = userSkillRepo;
        }

        public async Task<CommandResponse> Handle(UserUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_repo.Exists(u => u.Id != request.Id && (u.UserName == request.UserName || (u.Name == request.Name && u.Surname == request.Surname))))
                return Error("User with the same user name or full name exists!");
            var user = _repo.GetItem(request.Id);
            if (user is null)
                return Error("User not found!");
            await _userSkillRepo.Delete(us => us.UserId == request.Id);
            user.IsActive = request.IsActive;
            user.Name = request.Name?.Trim();
            user.Password = request.Password;
            user.RoleId = request.RoleId;
            user.Surname = request.Surname?.Trim();
            user.UserName = request.UserName;
            user.RegistrationDate = request.RegistrationDate;
            user.SkillIds = request.SkillIds;
            await _repo.Update(user);
            return Success("User updated successfully.", user.Id);
        }
    }
}
