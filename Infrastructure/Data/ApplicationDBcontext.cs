using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDBcontext : IdentityDbContext<UserI>//Inherit from IdentityDbContext
    {
        public ApplicationDBcontext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }//Pass up to Db context
        public DbSet<User> User { get; set; }// set table
        public DbSet<Hobby> Hobby { get; set; }// set table
    }
}
