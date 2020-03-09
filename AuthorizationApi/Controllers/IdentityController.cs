using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthorizationApi.Controllers
{
    [Route("identity/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly TokenService tokenService;
        private readonly IdentityService identityService;

        public IdentityController(TokenService tokenService, IdentityService identityService)
        {
            this.tokenService = tokenService;
            this.identityService = identityService;
        }

        [HttpPost]
        public dynamic Token([FromBody] TokenPayload payload)
        {
            var token = identityService.GenerateToken(payload);
            return token;
        }

        [HttpPost]
        public bool VerifyToken()
        {
            return tokenService.VerifyAccessToken();
        }
    }
}
