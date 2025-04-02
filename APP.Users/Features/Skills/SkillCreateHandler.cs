using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace APP.Users.Features.Skills
{
    public class SkillCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(125)]
        public string Name { get; set; }
    }

    public class SkillCreateHandler : RepoHandler<Skill>, IRequestHandler<SkillCreateRequest, CommandResponse>
    {
        public SkillCreateHandler(IRepo<Skill> repo, CultureInfo cultureInfo = null) : base(repo, cultureInfo)
        {
        }

        public async Task<CommandResponse> Handle(SkillCreateRequest request, CancellationToken cancellationToken)
        {
            if (_repo.Exists(s => s.Name == request.Name))
                return Error("Skill with the same name exists!");
            var skill = new Skill()
            {
                Name = request.Name?.Trim()
            };
            await _repo.Create(skill);
            return Success("Skill created successfully.", skill.Id);
        }
    }
}
