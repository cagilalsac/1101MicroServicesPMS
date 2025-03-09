using APP.Projects.Domain;
using CORE.APP.Features;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Features.Works
{
    public class WorkCreateRequest : Request, IRequest<CommandResponse>
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

    public class WorkCreateHandler : ProjectsDbHandler, IRequestHandler<WorkCreateRequest, CommandResponse>
    {
        public WorkCreateHandler(ProjectsDb db) : base(db)
        {
        }

        public async Task<CommandResponse> Handle(WorkCreateRequest request, CancellationToken cancellationToken)
        {
            if (_db.Works.Any(w => w.Name == request.Name && (w.ProjectId ?? 0) == (request.ProjectId ?? 0)))
                return Error("Work with the same name exists for the project!");
            if (request.StartDate.Date > request.DueDate.Date)
                return Error("Start date must be before or equal to due date!");
            var work = new Work()
            {
                Description = request.Description?.Trim(),
                DueDate = request.DueDate,
                Name = request.Name?.Trim(),
                ProjectId = request.ProjectId,
                StartDate = request.StartDate
            };
            _db.Works.Add(work);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("Work created successfully.", work.Id);
        }
    }
}
