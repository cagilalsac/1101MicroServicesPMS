using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Tags
{
	public class TagDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class TagDeleteHandler : ProjectsDbHandler, IRequestHandler<TagDeleteRequest, CommandResponse>
    {
        public TagDeleteHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagDeleteRequest request, CancellationToken cancellationToken)
        {
            Tag tag = _db.Tags.Include(t => t.ProjectTags).SingleOrDefault(t => t.Id == request.Id);
            if (tag is null)
                return Error("Tag not found!");
            _db.ProjectTags.RemoveRange(tag.ProjectTags);
            _db.Tags.Remove(tag);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Tag deleted successfully", tag.Id);
        }
    }
}
