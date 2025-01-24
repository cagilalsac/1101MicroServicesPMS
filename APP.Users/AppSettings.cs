using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace APP.Users
{
    public class AppSettings
    {
        public static string Audience { get; set; } = string.Empty;
        public static string Issuer { get; set; } = string.Empty;
        public static int ExpirationInMinutes { get; set; }
        public static string SecurityKey { get; set; } = string.Empty;
        public static SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
    }
}
