using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APP.Users.Features.Skills
{
    public class SkillUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(125)]
        public string Name { get; set; }
    }

    public class SkillUpdateHandler : RepoHandler<Skill>, IRequestHandler<SkillUpdateRequest, CommandResponse>
    {
        public SkillUpdateHandler(IRepo<Skill> repo, CultureInfo cultureInfo = null) : base(repo, cultureInfo)
        {
        }

        public async Task<CommandResponse> Handle(SkillUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_repo.Exists(s => s.Id != request.Id && s.Name == request.Name))
                return Error("Skill with the same name exists!");
            var skill = _repo.GetItem(request.Id);
            if (skill is null)
                return Error("Skill not found!");
            skill.Name = request.Name?.Trim();
            await _repo.Update(skill);
            return Success("Skill updated successfully.", skill.Id);
        }
    }
}
