using APP.Users.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Users.Features.Skills
{
    public class SkillUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required, StringLength(125)]
        public string Name { get; set; }
    }

    public class SkillUpdateHandler : UsersDbHandler, IRequestHandler<SkillUpdateRequest, CommandResponse>
    {
        public SkillUpdateHandler(UsersDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(SkillUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Skills.Any(s => s.Id != request.Id && s.Name == request.Name))
                return Error("Skill with the same name exists!");
            var skill = _db.Skills.Find(request.Id);
            if (skill is null)
                return Error("Skill not found!");
            skill.Name = request.Name;
            _db.Skills.Update(skill);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Skill updated successfully.", skill.Id);
        }
    }
}
