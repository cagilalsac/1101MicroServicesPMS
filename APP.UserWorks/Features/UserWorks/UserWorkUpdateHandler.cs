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
    public class UserWorkUpdateRequest : Request, IRequest<CommandResponse>
    {
        public int UserId { get; set; }

        public int WorkId { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        [Range(0, 1)]
        public double Percentage { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class UserWorkUpdateHandler : UserWorksDbHandler, IRequestHandler<UserWorkUpdateRequest, CommandResponse>
    {
        public UserWorkUpdateHandler(UserWorksDb db, HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
            : base(db, httpClient, httpContextAccessor)
        {
        }

        public async Task<CommandResponse> Handle(UserWorkUpdateRequest request, CancellationToken cancellationToken)
        {
            if (!await _db.UserWorks.AnyAsync(uw => uw.Id != request.Id && uw.UserId == request.UserId && uw.WorkId == request.WorkId))
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
                var userWork = await _db.UserWorks.SingleOrDefaultAsync(uw => uw.Id == request.Id, cancellationToken);
                if (userWork is null)
                    return Error("User work not found!");
                userWork.UserId = request.UserId;
                userWork.WorkId = request.WorkId;
                userWork.Url = request.Url?.Trim();
                userWork.Percentage = request.Percentage;
                userWork.EndDate = request.EndDate;
                _db.UserWorks.Update(userWork);
                await _db.SaveChangesAsync(cancellationToken);
                return Success("User work updated successfully.", userWork.Id);
            }
            return Error("User work exists!");
        }
    }
}
