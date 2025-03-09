using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Works
{
	public class WorkUpdateRequest : Request, IRequest<CommandResponse>
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DueDate { get; set; }

        public int? ProjectId { get; set; }
    }

    public class WorkUpdateHandler : ProjectsDbHandler, IRequestHandler<WorkUpdateRequest, CommandResponse>
    {
        public WorkUpdateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(WorkUpdateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Works.Any(w => w.Id != request.Id && w.Name == request.Name
                && (w.ProjectId ?? 0) == (request.ProjectId ?? 0)))
                return Error("Work with the same name exists for the project!");
            if (request.StartDate.Date > request.DueDate.Date)
                return Error("Start date must be before or equal to due date!");
            var work = _db.Works.SingleOrDefault(w => w.Id == request.Id);
            if (work is null)
                return Error("Work not found!");
            work.Description = request.Description?.Trim();
            work.DueDate = request.DueDate;
            work.Name = request.Name?.Trim();
            work.ProjectId = request.ProjectId;
            work.StartDate = request.StartDate;
            _db.Works.Update(work);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Work updated successfully.", work.Id);
        }
    }
}
