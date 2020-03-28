using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        dynamic GenerateToken(TokenPayload payload);

        bool IsValidAccessToken(string authorizationHeader);
    }
}
