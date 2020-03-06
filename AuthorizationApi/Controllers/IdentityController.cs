using AuthorizationApi.Models;
using AuthorizationApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationApi.Controllers
{
    [Route("identity/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private TokenService _tokenService;
        private IdentityService _identityService;

        public IdentityController(TokenService tokenService, IdentityService identityService)
        {
            _tokenService = tokenService;
            _identityService = identityService;
        }

        [HttpPost]
        public dynamic Token([FromBody] TokenPayload payload)
        {
            _identityService.GenerateToken(payload);
            return payload;
            //return _tokenService.CreateToken();
        }

        [HttpPost]
        public bool VerifyToken()
        {
            return _tokenService.VerifyToken();
        }
    }
}
