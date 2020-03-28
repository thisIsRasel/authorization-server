using Application.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AuthorizationApi.Controllers
{
    [Route("identity/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IdentityService identityService;

        public IdentityController(IdentityService identityService)
        {
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
            Request.Headers.TryGetValue("Authorization", out var authorizationHeaders);
            return identityService.IsValidAccessToken(authorizationHeaders);
        }
    }
}
