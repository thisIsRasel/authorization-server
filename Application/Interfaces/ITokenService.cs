using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(UserDetails userDetails);

        string CreateRefreshToken();

        bool VerifyAccessToken(string token);
    }
}
