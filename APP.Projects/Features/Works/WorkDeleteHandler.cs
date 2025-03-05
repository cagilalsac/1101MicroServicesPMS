using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;

namespace APP.Projects.Features.Works
{
    public class WorkDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class WorkDeleteHandler : ProjectsDbHandler, IRequestHandler<WorkDeleteRequest, CommandResponse>
    {
        public WorkDeleteHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(WorkDeleteRequest request, CancellationToken cancellationToken)
        {
            var work = _db.Works.SingleOrDefault(w => w.Id == request.Id);
            if (work is null)
                return Error("Work not found!");
            _db.Works.Remove(work);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Work deleted successfully", work.Id);
        }
    }
}
