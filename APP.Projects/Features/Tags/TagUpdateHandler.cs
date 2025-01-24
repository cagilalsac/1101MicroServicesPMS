using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Tags
{
	public class TagUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
    }

    public class TagUpdateHandler : ProjectsDbHandler, IRequestHandler<TagUpdateRequest, CommandResponse>
    {
        public TagUpdateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Tags.Any(t => t.Id != request.Id && t.Name == request.Name))
                return Error("Tag with the same name exists!");
            Tag tag = _db.Tags.Find(request.Id);
            if (tag is null)
                return Error("Tag not found!");
            tag.Name = request.Name;
            _db.Tags.Update(tag);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Tag updated successfully.", tag.Id);
        }
    }
}
