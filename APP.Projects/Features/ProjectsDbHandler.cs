using APP.Projects.Domain;
using CORE.APP.Features;
using System.Globalization;

namespace APP.Projects.Features
{
    public abstract class ProjectsDbHandler : Handler
    {
        protected readonly ProjectsDb _db;

        protected ProjectsDbHandler(ProjectsDb db) : base(new CultureInfo("en-US"))
        {
            _db = db;
        }
    }
}
