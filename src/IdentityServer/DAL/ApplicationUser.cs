using Microsoft.AspNetCore.Identity;

namespace IdentityServer.DAL
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string Token { get; init; }
    }
}