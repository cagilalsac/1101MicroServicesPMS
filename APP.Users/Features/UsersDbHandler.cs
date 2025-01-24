using APP.Users.Domain;
using CORE.APP.Features;
using System.Globalization;

namespace APP.Users.Features
{
    public abstract class UsersDbHandler : Handler
    {
        protected readonly UsersDb _db;

        protected UsersDbHandler(UsersDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }
    }
}
