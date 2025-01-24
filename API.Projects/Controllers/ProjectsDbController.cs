#nullable disable
using APP.Projects.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//Generated from Custom Template.
namespace API.Projects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsDbController : ControllerBase
    {
        private readonly ProjectsDb _db;

        public ProjectsDbController(ProjectsDb projectsDb)
        {
            _db = projectsDb;
        }

        [HttpGet("Seed")]
        public IActionResult Seed()
        {
            var projectTags = _db.ProjectTags.ToList();
            _db.ProjectTags.RemoveRange(projectTags);
            var tags = _db.Tags.ToList();
            _db.Tags.RemoveRange(tags);
            var userWorks = _db.UserWorks.ToList();
            _db.UserWorks.RemoveRange(userWorks);
            var works = _db.Works.ToList();
            _db.Works.RemoveRange(works);
            var projects = _db.Projects.ToList();
            _db.Projects.RemoveRange(projects);

            if (projectTags.Any() || tags.Any() || userWorks.Any() || works.Any() || projects.Any())
            {
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('ProjectTags', RESEED, 0)");
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Tags', RESEED, 0)");
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('UserWorks', RESEED, 0)");
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Works', RESEED, 0)");
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Projects', RESEED, 0)");
            }

            _db.Tags.Add(new Tag()
            {
                Name = "C#"
            });
            _db.Tags.Add(new Tag()
            {
                Name = "Object Oriented Programming"
            });
            _db.Tags.Add(new Tag()
            {
                Name = "ASP.NET"
            });
            _db.Tags.Add(new Tag()
            {
                Name = "Entity Framework"
            });
            _db.Tags.Add(new Tag()
            {
                Name = "Microservices"
            });
            _db.Tags.Add(new Tag()
            {
                Name = "MVC"
            });
            _db.Tags.Add(new Tag()
            {
                Name = "Clean Architecture"
            });
            _db.Tags.Add(new Tag()
            {
                Name = "N-Layered Architecture"
            });

            _db.SaveChanges();

            _db.Projects.Add(new Project()
            {
                Description = "Bilkent University Computer Technology and Information Systems CTIS 465 Lecture",
                Name = "Bilkent CTIS 465 Spring 2025",
                _ProjectTags = new List<ProjectTag>()
                {
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "C#").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "Object Oriented Programming").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "ASP.NET").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "Entity Framework").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "Microservices").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "Clean Architecture").Id
                    }
                }
            });
            _db.Projects.Add(new Project()
            {
                Description = "Bilkent University Computer Technology and Information Systems CTIS 479 Lecture",
                Name = "Bilkent CTIS 479 Fall 2025",
                _ProjectTags = new List<ProjectTag>()
                {
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "C#").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "Object Oriented Programming").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "ASP.NET").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "Entity Framework").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "MVC").Id
                    },
                    new ProjectTag()
                    {
                        TagId = _db.Tags.SingleOrDefault(t => t.Name == "N-Layered Architecture").Id
                    }
                }
            });

            _db.SaveChanges();

            return Ok("Database seed successful.");
        }
    }
}
