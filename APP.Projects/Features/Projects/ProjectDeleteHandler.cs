using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Features.Projects
{
	public class ProjectDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class ProjectDeleteHandler : ProjectsDbHandler, IRequestHandler<ProjectDeleteRequest, CommandResponse>
    {
        public ProjectDeleteHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(ProjectDeleteRequest request, CancellationToken cancellationToken)
        {
            var project = _db.Projects.Include(p => p.ProjectTags).Include(p => p.Works).SingleOrDefault(p => p.Id == request.Id);
            if (project is null)
                return Error("Project not found!");
            if (project.Works.Any())
                return Error("Project can't be deleted becuase it has relational works!");
            _db.ProjectTags.RemoveRange(project.ProjectTags);
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Project deleted successfully", project.Id);
        }
    }
}
