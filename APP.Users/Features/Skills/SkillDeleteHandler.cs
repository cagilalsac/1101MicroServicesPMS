using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.Globalization;

namespace APP.Users.Features.Skills
{
    public class SkillDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class SkillDeleteHandler : RepoHandler<Skill>, IRequestHandler<SkillDeleteRequest, CommandResponse>
    {
        private readonly IRepo<UserSkill> _userSkillRepo;

        public SkillDeleteHandler(IRepo<Skill> repo, IRepo<UserSkill> userSkillRepo, CultureInfo cultureInfo = null) : base(repo, cultureInfo)
        {
            _userSkillRepo = userSkillRepo;
        }

        public async Task<CommandResponse> Handle(SkillDeleteRequest request, CancellationToken cancellationToken)
        {
            await _userSkillRepo.Delete(us => us.SkillId == request.Id);
            await _repo.Delete(request.Id);
            return Success("Skill deleted successfully", request.Id);
        }
    }
}
