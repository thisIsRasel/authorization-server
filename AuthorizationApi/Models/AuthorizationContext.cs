
using Microsoft.EntityFrameworkCore;

namespace AuthorizationApi.Models
{
    public class AuthorizationContext: DbContext
    {
        public AuthorizationContext(DbContextOptions<AuthorizationContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<ClientDetail> ClientDetails { get; set; }
        
        public DbSet<UserRoleMap> UserRoleMaps { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; } 
    }
}
