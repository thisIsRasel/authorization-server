using AuthorizationApi.Models;
using System;
using System.Collections.Generic;
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
                User user = null;
                switch(payload.GrantType.ToLower())
                {
                    case "authenticate_site":
                        user = GetDefaultAnonymousUser();
                        break;

                    case "password":
                        user = GetUser(payload.Username, payload.Password);
                        break;

                    case "refresh_token":
                        break;
                }

                if(user != null)
                {
                    return tokenService.CreateAccessToken(user);
                }

                throw new Exception("Unauthorized");
            }

            throw new Exception("Invalid request");
        }

        private bool IsValidRequestForGrantType(TokenPayload payload)
        {
            switch(payload.GrantType.ToLower())
            {
                case "authenticate_site":
                    return true;

                case "password":
                    if(payload.Username != null && payload.Password != null)
                    {
                        return true;
                    }

                    break;

                case "refresh_token":
                    if(payload.RefreshToken != null)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }

        private bool IsAuthenticClient(string clientId, string clientSecret)
        {
            var client = authorizationContext.ClientDetails.Where<ClientDetail>(client => client.ClientId == clientId && client.ClientSecret == clientSecret).FirstOrDefault();
            return client != null ? true : false;
        }

        private User GetDefaultAnonymousUser()
        {
            return new User()
            {
                Username = "default@gmail.com",
                Roles = new string[] { "anonymous" }
            };
        }

        private User GetUser(string userName, string password)
        {
            var user = authorizationContext.Users.Where<User>(u => u.Username == userName && u.Password == password).FirstOrDefault();
            if(user != null)
            {
                user.Roles = GetRolesByUserId(user.Id);
            }
            return user;
        }

        private string[] GetRolesByUserId(long userId)
        {
            List<string> roles = new List<string>();
            var results = authorizationContext.UserRoleMaps.Where<UserRoleMap>(userRole => userRole.UserId == userId).ToList();
            results.ForEach(item =>
            {
                roles.Add(item.Role);
            });

            return roles.ToArray();
        }
    }
}
