﻿using AuthorizationApi.Constants;
using Domain.Exceptions;
using AuthorizationApi.Models;
using Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthorizationApi.Services
{
    public class IdentityService
    {
        private readonly AuthorizationDbContext authorizationContext;
        private readonly TokenService tokenService;

        public IdentityService(AuthorizationDbContext authorizationContext, TokenService tokenService)
        {
            this.authorizationContext = authorizationContext;
            this.tokenService = tokenService;
        }

        public dynamic GenerateToken(TokenPayload payload)
        {
            if (!IsInputValidForDifferentGrantType(payload))
            {
                throw new InvalidRequestException();
            }

            string grantType = payload.GrantType?.ToLower() ?? null;

            if (grantType == ApiConstant.GrantTypeAuthenticateSite)
            { 
                return GetTokenForAuthenticateSiteGrantType();
            }

            if(grantType == ApiConstant.GrantTypePassword)
            {
                return GetTokenForPasswordGrantType(payload);
            }

            if(grantType == ApiConstant.GrantTypeRefreshToken)
            {
                return GetTokenForRefreshTokenGrantType(payload);
            }

            throw new UnauthorizedException();
        }

        private JObject GetTokenForAuthenticateSiteGrantType()
        {
            var obj = new JObject()
            {
                ["token_type"] = ApiConstant.TokenTypeBearer,
                ["expires_in"] = 5 * 60
            };
            
            UserDetails userDetails = GetDefaultAnonymousUserDetails();
            obj.Add("access_token", tokenService.CreateAccessToken(userDetails));
            obj.Add("refresh_token", CreateRefreshToken(userDetails));
            return obj;
        }

        private JObject GetTokenForPasswordGrantType(TokenPayload payload)
        {
            var obj = new JObject()
            {
                ["token_type"] = ApiConstant.TokenTypeBearer,
                ["expires_in"] = 5 * 60
            };

            UserDetails userDetails = GetUserDetailsByUsernameAndPassword(payload.Username, payload.Password);
            obj.Add("access_token", tokenService.CreateAccessToken(userDetails));
            obj.Add("refresh_token", CreateRefreshToken(userDetails));
            return obj;
        }

        private JObject GetTokenForRefreshTokenGrantType(TokenPayload payload)
        {
            var obj = new JObject()
            {
                ["token_type"] = ApiConstant.TokenTypeBearer,
                ["expires_in"] = 5 * 60
            };

            RefreshToken refreshToken = FindRefreshToken(payload.RefreshToken);
            if(refreshToken == null)
            {
                throw new InvalidRefreshTokenException();
            }
        
            if(refreshToken.CreatedAt.AddMinutes(10) < DateTime.Now)
            {
                throw new ExpiredRefreshTokenException();
            }

            UserDetails userDetails = GetUserDetailsByUsername(refreshToken.UserId);
            if(userDetails == null)
            {
                userDetails = GetDefaultAnonymousUserDetails();
            }

            obj.Add("access_token", tokenService.CreateAccessToken(userDetails));

            return obj;
        }

        private string CreateRefreshToken(UserDetails userDetails)
        {
            string refreshToken = tokenService.CreateRefreshToken();
            authorizationContext.RefreshTokens.Add(new RefreshToken()
            {
                Token = refreshToken,
                CreatedAt = DateTime.Now,
                UserId = userDetails.Username
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
            string grantType = payload.GrantType?.ToLower() ?? null;

            if(grantType == ApiConstant.GrantTypeAuthenticateSite)
            {
                return true;
            }
            else if(grantType == ApiConstant.GrantTypePassword && payload.Username != null && payload.Password != null)
            {
                return true;
            }
            else if(grantType == ApiConstant.GrantTypeRefreshToken && payload.RefreshToken != null)
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

        private UserDetails GetDefaultAnonymousUserDetails()
        {
            return new UserDetails()
            {
                Username = "default@gmail.com",
                Roles = new string[] { "anonymous" }
            };
        }

        private UserDetails GetUserDetailsByUsernameAndPassword(string userName, string password)
        {
            UserDetails userDetails = null;
            var user = authorizationContext.Users.Where<User>(u => u.Username == userName && u.Password == password).FirstOrDefault();
            if(user != null)
            {
                userDetails = new UserDetails
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Roles = GetRolesByUserId(user.Id)
                };
            }
            return userDetails;
        }

        private UserDetails GetUserDetailsByUsername(string userName)
        {
            UserDetails userDetails = null;
            var user = authorizationContext.Users.Where<User>(u => u.Username == userName).FirstOrDefault();
            if (user != null)
            {
                userDetails = new UserDetails
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password,
                    Roles = GetRolesByUserId(user.Id)
                };
            }
            return userDetails;
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