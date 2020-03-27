using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationApi
{
    public class AuthorizationDbContext: DbContext, IAuthorizationDbContext
    {
        public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<ClientDetail> ClientDetails { get; set; }
        
        public DbSet<UserRoleMap> UserRoleMaps { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; } 
    }
}
