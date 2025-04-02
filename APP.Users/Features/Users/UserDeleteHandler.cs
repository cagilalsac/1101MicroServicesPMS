using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.Globalization;

namespace APP.Users.Features.Users
{
    public class UserDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class UserDeleteHandler : RepoHandler<User>, IRequestHandler<UserDeleteRequest, CommandResponse>
    {
        private readonly IRepo<UserSkill> _userSkillRepo;

        public UserDeleteHandler(IRepo<User> repo, IRepo<UserSkill> userSkillRepo, CultureInfo cultureInfo = null) : base(repo, cultureInfo)
        {
            _userSkillRepo = userSkillRepo;
        }

        public async Task<CommandResponse> Handle(UserDeleteRequest request, CancellationToken cancellationToken)
        {
            var user = _repo.GetItem(request.Id);
            if (user is null)
                return Error("User not found!");
            await _userSkillRepo.Delete(us => us.UserId == request.Id);
            await _repo.Delete(request.Id);
            return Success("User deleted successfully", user.Id);
        }
    }
}
