using Lacromis.Models;
using Lacromis.Models.Account;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lacromis.DAL
{
    public class AppDbContext: IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            

        }
       public DbSet<Garbage> garbages { get; set; }
      public  DbSet<Catagory> catagories { get; set; }
      public  DbSet<Metal> metals { get; set; }
    }
}
