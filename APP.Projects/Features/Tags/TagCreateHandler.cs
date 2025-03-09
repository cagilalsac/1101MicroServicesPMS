using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Tags
{
	public class TagCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
    }

    public class TagCreateHandler : ProjectsDbHandler, IRequestHandler<TagCreateRequest, CommandResponse>
    {
        public TagCreateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(TagCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Tags.Any(t => t.Name == request.Name))
                return Error("Tag with the same name exists!");
            Tag tag = new Tag()
            {
                Name = request.Name?.Trim()
            };
            _db.Tags.Add(tag);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Tag created successfully.", tag.Id);
        }
    }
}
