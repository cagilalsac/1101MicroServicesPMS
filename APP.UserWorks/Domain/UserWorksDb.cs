using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APP.UserWorks.Domain
{
    public class UserWorksDb : DbContext
    {
        public DbSet<UserWork> UserWorks { get; set; }

        public UserWorksDb(DbContextOptions options) : base(options)
        {
        }
    }

    public class UserWorksDbFactory : IDesignTimeDbContextFactory<UserWorksDb>
    {
        public UserWorksDb CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserWorksDb>();
            //optionsBuilder.UseSqlServer("server=(localdb)\\mssqllocaldb;database=PMSUserWorksDB;trusted_connection=true;");
            optionsBuilder.UseSqlServer("server=127.0.0.1,1433;database=PMSUserWorksDB;user id=sa;password=Cagil123!;trustservercertificate=true;");
            return new UserWorksDb(optionsBuilder.Options);
        }
    }
}
