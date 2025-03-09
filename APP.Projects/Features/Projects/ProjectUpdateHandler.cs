using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Projects
{
	public class ProjectUpdateRequest : Request, IRequest<CommandResponse>
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

    public class ProjectUpdateHandler : ProjectsDbHandler, IRequestHandler<ProjectUpdateRequest, CommandResponse>
    {
        public ProjectUpdateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(ProjectUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Projects.Any(p => p.Id != request.Id && p.Name == request.Name))
                return Error("Project with the same name exists!");
            var project = _db.Projects.Include(p => p.ProjectTags).SingleOrDefault(p => p.Id == request.Id);
            if (project is null)
                return Error("Project not found!");
            _db.ProjectTags.RemoveRange(project.ProjectTags);
            project.Description = request.Description?.Trim();
            project.Name = request.Name?.Trim();
            project.Url = request.Url?.Trim();
            project.Version = request.Version;
            project.TagIds = request.TagIds;
            _db.Projects.Update(project);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Project updated successfully.", project.Id);
        }
    }
}
