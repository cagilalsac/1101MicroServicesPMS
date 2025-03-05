using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Projects.Features.Tags
{
    public class TagQueryRequest : Request, IRequest<IQueryable<TagQueryResponse>>
    {
    }

    public class TagQueryResponse : QueryResponse
    {
        public string Name { get; set; }
    }

    public class TagQueryHandler : ProjectsDbHandler, IRequestHandler<TagQueryRequest, IQueryable<TagQueryResponse>>
    {
        public TagQueryHandler(ProjectsDb db) : base(db)
        {
        }

        public Task<IQueryable<TagQueryResponse>> Handle(TagQueryRequest request, CancellationToken cancellationToken)
        {
            IQueryable<TagQueryResponse> query = _db.Tags.OrderBy(t => t.Name).Select(t => new TagQueryResponse()
            {
                Id = t.Id,
                Name = t.Name
            });
            return Task.FromResult(query);
        }
    }
}
