using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.DAL
{
    public class AuthorizationContext : IdentityDbContext<ApplicationUser>
    {
        public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options)
        {

        }
    }
}