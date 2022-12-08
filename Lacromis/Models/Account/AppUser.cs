using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace Lacromis.Models.Account
{
    public class AppUser: IdentityUser
    {
         public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
