using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Projects
{
	public class ProjectCreateRequest : Request, IRequest<CommandResponse>
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "{0} must be minimum {2} maximum {1} characters.")]
        public string Name { get; set; }

        [StringLength(4000, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Description { get; set; }

        [StringLength(400, ErrorMessage = "{0} must be maximum {1} characters.")]
        public string Url { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "{0} must be a positive decimal number.")]
        public double? Version { get; set; }

        public List<int> TagIds { get; set; }
    }

    public class ProjectCreateHandler : ProjectsDbHandler, IRequestHandler<ProjectCreateRequest, CommandResponse>
    {
        public ProjectCreateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(ProjectCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Projects.Any(p => p.Name == request.Name))
                return Error("Project with the same name exists!");
            var project = new Project()
            {
                Description = request.Description,
                Name = request.Name,
                Url = request.Url,
                Version = request.Version,
                TagIds = request.TagIds
            };
            _db.Projects.Add(project);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Project created successfully.", project.Id);
        }
    }
}
