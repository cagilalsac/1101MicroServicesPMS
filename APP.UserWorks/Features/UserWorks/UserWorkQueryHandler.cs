using APP.UserWorks.Domain;
using APP.UserWorks.Features.Users;
using APP.UserWorks.Features.Works;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace APP.UserWorks.Features.UserWorks
{
    public class UserWorkQueryRequest : Request, IRequest<List<UserWorkQueryResponse>>
    {
    }

    public class UserWorkQueryResponse : QueryResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int WorkId { get; set; }
        public string WorkName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Url { get; set; }
        public double Percentage { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class UserWorkQueryHandler : UserWorksDbHandler, IRequestHandler<UserWorkQueryRequest, List<UserWorkQueryResponse>>
    {
        public UserWorkQueryHandler(UserWorksDb db, HttpClient httpClient, IHttpContextAccessor httpContextAccessor) 
            : base(db, httpClient, httpContextAccessor)
        {
        }

        public async Task<List<UserWorkQueryResponse>> Handle(UserWorkQueryRequest request, CancellationToken cancellationToken)
        {
            var userWorkQuery = _db.UserWorks.Select(uw => new UserWorkQueryResponse()
                {
                    Id = uw.Id,
                    UserId = uw.UserId,
                    WorkId = uw.WorkId,
                    Url = uw.Url,
                    Percentage = uw.Percentage,
                    EndDate = uw.EndDate,
                });
            if (request.Id > 0)
                userWorkQuery = userWorkQuery.Where(uw => uw.Id == request.Id);
            var userWorkList = await userWorkQuery.ToListAsync(cancellationToken);
            if (userWorkList.Any())
            {
                var userHttpResponseMessage = await _httpClient.GetAsync(_usersGatewayUri, cancellationToken);
                if (userHttpResponseMessage.IsSuccessStatusCode)
                {
                    var userList = await userHttpResponseMessage.Content.ReadFromJsonAsync<List<UserQueryResponse>>(cancellationToken);
                    var workHttpResponseMessage = await _httpClient.GetAsync(_worksGatewayUri, cancellationToken);
                    if (workHttpResponseMessage.IsSuccessStatusCode)
                    {
                        var workList = await workHttpResponseMessage.Content.ReadFromJsonAsync<List<WorkQueryResponse>>(cancellationToken);
                        foreach (var userWorkItem in userWorkList)
                        {
                            var userItem = userList.Single(u => u.Id == userWorkItem.UserId);
                            var workItem = workList.Single(w => w.Id == userWorkItem.WorkId);
                            userWorkItem.UserName = userItem.UserName;
                            userWorkItem.WorkName = workItem.Name;
                            userWorkItem.ProjectId = workItem.ProjectId;
                            userWorkItem.ProjectName = workItem.ProjectName;
                            userWorkItem.DueDate = workItem.DueDate;
                            userWorkItem.StartDate = workItem.StartDate;
                        }
                    }
                }
            }
            return userWorkList;
        }
    }
}
