using APP.UserWorks.Domain;
using CORE.APP.Features;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace APP.UserWorks.Features.UserWorks
{
    public class UserWorkDeleteRequest : Request, IRequest<CommandResponse>
    {
    }

    public class UserWorkDeleteHandler : UserWorksDbHandler, IRequestHandler<UserWorkDeleteRequest, CommandResponse>
    {
        public UserWorkDeleteHandler(UserWorksDb db, HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(db, httpClient, httpContextAccessor)
        {
        }

        public async Task<CommandResponse> Handle(UserWorkDeleteRequest request, CancellationToken cancellationToken)
        {
            var userWork = await _db.UserWorks.SingleOrDefaultAsync(uw => uw.Id == request.Id, cancellationToken);
            if (userWork is null)
                return Error("User work not found!");
            _db.UserWorks.Remove(userWork);
            await _db.SaveChangesAsync(cancellationToken);
            return Success("User work deleted successfully.", userWork.Id);
        }
    }
}
