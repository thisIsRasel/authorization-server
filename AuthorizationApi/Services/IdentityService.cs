using AuthorizationApi.Models;
using System.Linq;

namespace AuthorizationApi.Services
{
    public class IdentityService
    {
        private readonly AuthorizationContext authorizationContext;
        private readonly TokenService tokenService;

        public IdentityService(AuthorizationContext authorizationContext, TokenService tokenService)
        {
            this.authorizationContext = authorizationContext;
            this.tokenService = tokenService;
        }

        public string GenerateToken(TokenPayload payload)
        {
            if(IsValidRequestForGrantType(payload))
            {
                GetUser(payload.Username, payload.Password);
            }

            return "INVALID";
        }

        private bool IsValidRequestForGrantType(TokenPayload payload)
        {
            switch(payload.GrantType.ToLower())
            {
                case "password":
                    if(payload.Username != null && payload.Password != null)
                    {
                        return true;
                    }
                    break;

                default:
                    break;
            }

            return false;
        }

        private bool IsAuthenticClient(TokenPayload payload)
        {
            var clients = authorizationContext.ClientDetails.Where<ClientDetail>(client => client.ClientId == payload.ClientId && client.ClientSecret == payload.ClientSecret).ToList();
            return clients.Count > 0 ? true : false;
        }

        private void GetUser(string userName, string password)
        {
            var user = authorizationContext.Users.Where<User>(user => user.Username == userName && user.Password == password).First();
        }

        private void GetRolesByUserId(long userId)
        {
            var roles = authorizationContext.UserRoleMaps.Where<UserRoleMap>(userRole => userRole.UserId == userId).ToList();
            
        }
    }
}
