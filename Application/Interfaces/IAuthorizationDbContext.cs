using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthorizationDbContext
    {
        DbSet<User> Users { get; set; }

        DbSet<ClientDetail> ClientDetails { get; set; }

        DbSet<UserRoleMap> UserRoleMaps { get; set; }

        DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
