using APP.UserWorks.Domain;
using APP.UserWorks.Features.Users;
using APP.UserWorks.Features.Works;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;

namespace APP.UserWorks.Features.UserWorks
{
    public class UserWorkCreateRequest : Request, IRequest<CommandResponse>
    {
        public int UserId { get; set; }

        public int WorkId { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        [Range(0, 1)]
        public double Percentage { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class UserWorkCreateHandler : UserWorksDbHandler, IRequestHandler<UserWorkCreateRequest, CommandResponse>
    {
        public UserWorkCreateHandler(UserWorksDb db, HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
            : base(db, httpClient, httpContextAccessor)
        {
        }

        public async Task<CommandResponse> Handle(UserWorkCreateRequest request, CancellationToken cancellationToken)
        {
            if (!await _db.UserWorks.AnyAsync(uw => uw.UserId == request.UserId && uw.WorkId == request.WorkId))
            {
                var userHttpResponseMessage = await _httpClient.GetAsync($"{_usersGatewayUri}/{request.UserId}", cancellationToken);
                if ((int)userHttpResponseMessage.StatusCode == StatusCodes.Status500InternalServerError)
                    return Error("An exception occured while fetching the user!");
                if (userHttpResponseMessage.StatusCode == (HttpStatusCode)StatusCodes.Status204NoContent)
                    return Error("User not found!");
                var user = await userHttpResponseMessage.Content.ReadFromJsonAsync<UserQueryResponse>(cancellationToken);
                var workHttpResponseMessage = await _httpClient.GetAsync($"{_worksGatewayUri}/{request.WorkId}", cancellationToken);
                if ((int)workHttpResponseMessage.StatusCode == StatusCodes.Status500InternalServerError)
                    return Error("An exception occured while fetching the work!");
                if ((int)workHttpResponseMessage.StatusCode == StatusCodes.Status204NoContent)
                    return Error("Work not found!");
                var work = await workHttpResponseMessage.Content.ReadFromJsonAsync<WorkQueryResponse>(cancellationToken);
                if (request.EndDate.HasValue && work.StartDate > request.EndDate)
                    return Error("Start date must be before or equal to end date!");
                var userWork = new UserWork()
                {
                    UserId = request.UserId,
                    WorkId = request.WorkId,
                    Url = request.Url?.Trim(),
                    Percentage = request.Percentage,
                    EndDate = request.EndDate
                };
                _db.UserWorks.Add(userWork);
                await _db.SaveChangesAsync(cancellationToken);
                return Success("User work created successfully.", userWork.Id);
            }
            return Error("User work exists!");
        }
    }
}
