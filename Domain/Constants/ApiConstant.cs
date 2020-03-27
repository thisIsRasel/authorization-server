using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class ApiConstant
    {
        public const string GrantTypeAuthenticateSite = "authenticate_site";
        public const string GrantTypePassword = "password";
        public const string GrantTypeRefreshToken = "refresh_token";
        public const string TokenTypeBearer = "Bearer";
    }
}
