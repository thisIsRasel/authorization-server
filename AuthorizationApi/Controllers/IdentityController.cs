using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;

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
            return identityService.GenerateToken(payload);
        }

        [HttpPost]
        public bool VerifyToken()
        {
            return tokenService.VerifyAccessToken();
        }
    }
}
