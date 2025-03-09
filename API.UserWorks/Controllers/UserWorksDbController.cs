using APP.UserWorks.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.UserWorks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserWorksDbController : ControllerBase
    {
        private readonly UserWorksDb _db;

        public UserWorksDbController(UserWorksDb db)
        {
            _db = db;
        }

        [HttpGet("Seed")]
        public IActionResult Seed()
        {
            var userWorks = _db.UserWorks.ToList();
            _db.UserWorks.RemoveRange(userWorks);

            if (userWorks.Any())
            {
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('UserWorks', RESEED, 0)");
            }

            _db.UserWorks.Add(new UserWork()
            {
                UserId = 1,
                WorkId = 1,
                EndDate = DateTime.Now.AddMonths(-9),
                Percentage = 1
            });
            _db.UserWorks.Add(new UserWork()
            {
                UserId = 1,
                WorkId = 2,
                EndDate = DateTime.Now.AddMonths(-2),
                Percentage = 0.9
            });

            _db.UserWorks.Add(new UserWork()
            {
                UserId = 2,
                WorkId = 3,
                EndDate = DateTime.Now.AddMonths(-19),
                Percentage = 1
            });
            _db.UserWorks.Add(new UserWork()
            {
                UserId = 2,
                WorkId = 4,
                EndDate = DateTime.Now.AddMonths(-17),
                Percentage = 0.5
            });
            //_db.UserWorks.Add(new UserWork()
            //{
            //    UserId = 2,
            //    WorkId = 5,
            //    Percentage = 0.25
            //});

            _db.SaveChanges();

            return Ok("Database seed successful.");
        }
    }
}
