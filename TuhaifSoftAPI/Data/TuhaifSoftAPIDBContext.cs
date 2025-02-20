using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TuhaifSoftAPI.Models;

namespace TuhaifSoftAPI.Data
{
    public class TuhaifSoftAPIDBContext : IdentityDbContext<Users>
    {
        public TuhaifSoftAPIDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Students> Students { get; set; }
        // public DbSet<Users> Users { get; set; }

        public DbSet<RefreshTokens> RefreshTokens { get; set; }
        public DbSet<Major> Majors { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

        }
   
    }
}
