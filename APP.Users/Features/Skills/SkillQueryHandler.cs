using APP.Users.Domain;
using CORE.APP.Features;
using CORE.APP.Repositories;
using MediatR;
using System.Globalization;

namespace APP.Users.Features.Skills
{
    public class SkillQueryRequest : Request, IRequest<IQueryable<SkillQueryResponse>>
    {
    }

    public class SkillQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    public class SkillQueryHandler : RepoHandler<Skill>, IRequestHandler<SkillQueryRequest, IQueryable<SkillQueryResponse>>
    {
        public SkillQueryHandler(IRepo<Skill> repo, CultureInfo cultureInfo = null) : base(repo, cultureInfo)
        {
        }

        public Task<IQueryable<SkillQueryResponse>> Handle(SkillQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _repo.Query().OrderBy(s => s.Name).Select(s => new SkillQueryResponse()
            {
                Id = s.Id,
                Name = s.Name
            });
            return Task.FromResult(query);
        }
    }
}
