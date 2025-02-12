using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Projects
{
    public class ProjectQueryRequest : Request, IRequest<IQueryable<ProjectQueryResponse>>
    {
    }

    public class ProjectQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public double? Version { get; set; }
        public string VersionF { get; set; }
        public List<int> TagIds { get; set; }
        public string Tags { get; set; }
    }

    public class ProjectQueryHandler : ProjectsDbHandler, IRequestHandler<ProjectQueryRequest, IQueryable<ProjectQueryResponse>>
    {
        public ProjectQueryHandler(ProjectsDb db) : base(db)
        {
        }

        public Task<IQueryable<ProjectQueryResponse>> Handle(ProjectQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _db.Projects.Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag)
                .OrderBy(p => p.Name).ThenByDescending(p => p.Version).Select(p => new ProjectQueryResponse()
            {
                Description = p.Description,
                Id = p.Id,
                Name = p.Name,
                TagIds = p.TagIds,
                Url = p.Url,
                Version = p.Version,
                VersionF = p.Version.HasValue ? "v" + p.Version.Value.ToString("N1") : null,
                Tags = string.Join(", ", p.ProjectTags.Select(pt => pt.Tag.Name))
            });
            return Task.FromResult(query);
        }
    }
}
