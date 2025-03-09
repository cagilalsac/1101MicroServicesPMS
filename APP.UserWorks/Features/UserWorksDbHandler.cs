using APP.UserWorks.Domain;
using CORE.APP.Features;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net.Http.Headers;

namespace APP.UserWorks.Features
{
    public abstract class UserWorksDbHandler : Handler
    {
        protected readonly string _usersGatewayUri = "https://localhost:7280/api/Users";
        protected readonly string _worksGatewayUri = "https://localhost:7280/api/Works";

        protected readonly UserWorksDb _db;
        protected readonly HttpClient _httpClient;

        protected readonly string _scheme;
        protected readonly string _token;

        public UserWorksDbHandler(UserWorksDb db, HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : base(new CultureInfo("en-US"))
        {
            _db = db;
            _httpClient = httpClient;
            _scheme = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[0];
            _token = httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_scheme, _token);
        }
    }
}
