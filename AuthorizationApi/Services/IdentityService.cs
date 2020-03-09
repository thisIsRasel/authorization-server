using AuthorizationApi.Models;
using Newtonsoft.Json.Linq;
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

        public dynamic GenerateToken(TokenPayload payload)
        {
            if (!IsInputValidForDifferentGrantType(payload))
            {
                throw new Exception("Invalid Request");
            }

            string grantType = payload.GrantType != null ? payload.GrantType.ToLower() : null;

            if (grantType == "authenticate_site")
            { 
                return GetTokenForAuthenticateSiteGrantType();
            }

            if(grantType == "password")
            {
                return GetTokenForPasswordGrantType(payload);
            }

            if(grantType == "refresh_token")
            {
                return GetTokenForRefreshTokenGrantType(payload);
            }

            throw new Exception("Unauthorized");
        }

        private JObject GetTokenForAuthenticateSiteGrantType()
        {
            var obj = new JObject();
            obj.Add("token_type", "Bearer");
            obj.Add("expires_in", 5 * 60);

            User user = GetDefaultAnonymousUser();
            obj.Add("access_token", tokenService.CreateAccessToken(user));
            obj.Add("refresh_token", CreateRefreshToken(user));
            return obj;
        }

        private JObject GetTokenForPasswordGrantType(TokenPayload payload)
        {
            var obj = new JObject();
            obj.Add("token_type", "Bearer");
            obj.Add("expires_in", 5 * 60);

            User user = GetUserByUsernameAndPassword(payload.Username, payload.Password);
            obj.Add("access_token", tokenService.CreateAccessToken(user));
            obj.Add("refresh_token", CreateRefreshToken(user));
            return obj;
        }

        private JObject GetTokenForRefreshTokenGrantType(TokenPayload payload)
        {
            var obj = new JObject();
            obj.Add("token_type", "Bearer");
            obj.Add("expires_in", 5 * 60);

            RefreshToken refreshToken = FindRefreshToken(payload.RefreshToken);
            if(refreshToken == null)
            {
                throw new Exception("Invalid refresh token");
            }

            var currentTime = DateTime.Now;
            var createdAt = refreshToken.CreatedAt.AddMinutes(10);

            if(refreshToken.CreatedAt.AddMinutes(10) < DateTime.Now)
            {
                throw new Exception("Refresh token expired");
            }

            User user = GetUserByUsername(refreshToken.UserId);
            if(user == null)
            {
                user = GetDefaultAnonymousUser();
            }

            obj.Add("access_token", tokenService.CreateAccessToken(user));

            return obj;
        }

        private string CreateRefreshToken(User user)
        {
            string refreshToken = tokenService.CreateRefreshToken();
            authorizationContext.RefreshTokens.Add(new RefreshToken()
            {
                Token = refreshToken,
                CreatedAt = DateTime.Now,
                UserId = user.Username
            });
            authorizationContext.SaveChanges();

            return refreshToken;
        }

        private RefreshToken FindRefreshToken(string refreshToken)
        {
            RefreshToken token = authorizationContext.RefreshTokens.Where<RefreshToken>(r => r.Token == refreshToken).FirstOrDefault();
            return token;
        }

        private bool IsInputValidForDifferentGrantType(TokenPayload payload)
        {
            string grantType = payload.GrantType != null ? payload.GrantType.ToLower() : null;

            if(grantType == "authenticate_site")
            {
                return true;
            }
            else if(grantType == "password" && payload.Username != null && payload.Password != null)
            {
                return true;
            }
            else if(grantType == "refresh_token" && payload.RefreshToken != null)
            {
                return true;
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

        private User GetUserByUsernameAndPassword(string userName, string password)
        {
            var user = authorizationContext.Users.Where<User>(u => u.Username == userName && u.Password == password).FirstOrDefault();
            if(user != null)
            {
                user.Roles = GetRolesByUserId(user.Id);
            }
            return user;
        }

        private User GetUserByUsername(string userName)
        {
            var user = authorizationContext.Users.Where<User>(u => u.Username == userName).FirstOrDefault();
            if (user != null)
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
