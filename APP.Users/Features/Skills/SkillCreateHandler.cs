using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Skills
{
    public class SkillCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(125)]
        public string Name { get; set; }
    }

    public class SkillCreateHandler : UsersDbHandler, IRequestHandler<SkillCreateRequest, CommandResponse>
    {
        public SkillCreateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(SkillCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Skills.Any(s => s.Name == request.Name))
                return Error("Skill with the same name exists!");
            var skill = new Skill()
            {
                Name = request.Name?.Trim()
            };
            _db.Skills.Add(skill);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Skill created successfully.", skill.Id);
        }
    }
}
