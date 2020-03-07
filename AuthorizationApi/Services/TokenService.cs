﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AuthorizationApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationApi.Services
{

    
    public class TokenService
    {

        private string PrivateKey = @"-----BEGIN PRIVATE KEY-----
            MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCF31d4r6s4bwI/
            Pab/xnW262TWJ44DpS5fkiZdK9+Zs22XMM8m75pNXCF9BDD+ktu/K8aNmPvqIV3h
            u1npS+fPrDtC9AcA7d6IIgWBqVnZV1/OqjgsSQ5w7CyjCHq5iEJYjqShwHTmv/Un
            VcYI1aXUbTR3B5YUB9wBIkxftACSsaHbYIeckdev1aQ7hqv5rOERmyB11GUMVzga
            yojkEJtLnNVOOqrgrZs9iLm45wYjdsIYNicrMRuFC8FSe9qvMJ5f/lAKD+KAV89p
            VKTizhqD1XwFngITLA1YKFyI36MZyUVR5sn6Wk+sAZ0S/OKCDOErbfKBvTsltceE
            M0zshiX9AgMBAAECggEAH5UKCMHArgaaTBsaW7sRKD8uQcuBGGEOqKNNA6wHCzam
            QwXTyQa7q0Zx7BFiEZzJ1LbaF6uguqJ2iHtvgbdTj829CVWSlWiImCsWi3lNZhXT
            12iW6fOuQXumKCwm78AyjrvQIQzLR2i/yDDXEFx+y7ebqf0TLdSQ1X+m7ukDPZfq
            1WT49ViV2W7jrNbAVHdPlYdNAYR2aCIa3DSzWAK0KLBfBhdMeqSyUapmcuELhWyF
            +GO1UuwOo6+Z1cKLKS56I8Udy1KstO5rMRg/3TfDsJL1xauVNYcjRb5Bv9cWLmcN
            ERGvTstvAUDY7fNSxCuXYQFaInmeLoi9EPrrGZK+LQKBgQDoFdWYVdYMglxcwIlH
            LLE36IVLB8IcCmAoBNSJ87ROH4wJnIoBwPY2ncBVOIRSM7q0BObaWxz1HuT9Cnni
            i+EFzYIyeu9ls21lemcsFch6PmX70goTlbPUP1FMwjSGODan4ks3Hvkbh6m/SRdE
            386yDhLMy2Mz+JeAVp01tZRhDwKBgQCTqsFjLPGvoAqa6209JCYyaSH6cIweYMh4
            JXdejGJCa2xqKTtjkMlgHUuk8JbrZbVGvNVwgD9OjlEQMVmxmzY9nb1o+Pkc0kt9
            dodT2kp5kfA3HxtG6CmLMUmEeUeu1Hi+c0EpcS6sMO357HclmSFUzo/dcL8vbPul
            UFMlbfwwMwKBgQDetiU0fBpeArLzZufVXzPo/L78k0NYeTRw8sGqaCz8UxWlG4zr
            xen/2z34CbYg0/c0TQfjPk/5gV4o3Oa92ZFkaiOooYQdyiMOoHllOroZZMuk7Zgx
            1xSsdt5jlHGm40a/sE6RZK2UF4lzQIaN11+YSh0MXZijBMkNpRBTvB3ZTQKBgQCR
            j27WLkWHk8EgeRXTgUDNzGicEDlu8IRsOrJyVvu88VuLQl2yhdSblAUu76I1JKsO
            sbHMFf9RlmQ+DAeYVaGgF0/QnCwjxwB3ClHXzIRBViRukZE30j6xYMJRfaO1qOfl
            BIOlMFaLkifjhr7kJxB+IqgZ5rvvKgTdLlLnwIUplQKBgA7TDti4eZmniy9ByIXR
            MqCKd7WLDwWObaN5G1/lD/kcrIAgGsDLhB+EcB8dwAHs8hbLhRVbWcGtPvViao6f
            JiPilB2a1FPu68kw97rNpoeA9+DqYXq3UApXJMX5w3H5TrJsElhMjhjPuRQnbBqh
            g71HzyoHH2IA/bBYQxWXBtM5
            -----END PRIVATE KEY-----";

        private string PublicKey = @"-----BEGIN PUBLIC KEY-----
            MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhd9XeK+rOG8CPz2m/8Z1
            tutk1ieOA6UuX5ImXSvfmbNtlzDPJu+aTVwhfQQw/pLbvyvGjZj76iFd4btZ6Uvn
            z6w7QvQHAO3eiCIFgalZ2Vdfzqo4LEkOcOwsowh6uYhCWI6kocB05r/1J1XGCNWl
            1G00dweWFAfcASJMX7QAkrGh22CHnJHXr9WkO4ar+azhEZsgddRlDFc4GsqI5BCb
            S5zVTjqq4K2bPYi5uOcGI3bCGDYnKzEbhQvBUnvarzCeX/5QCg/igFfPaVSk4s4a
            g9V8BZ4CEywNWChciN+jGclFUebJ+lpPrAGdEvziggzhK23ygb07JbXHhDNM7IYl
            /QIDAQAB
            -----END PUBLIC KEY-----";

        public string CreateAccessToken(User user)
        {

            var privateKeyBytes = Convert.FromBase64String(PrivateKey.Replace("-----BEGIN PRIVATE KEY-----", "").Replace("-----END PRIVATE KEY-----", ""));
            var rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

            var key = new RsaSecurityKey(rsa);

            var handler = new JwtSecurityTokenHandler();
            var now = DateTime.UtcNow;

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: "me",
                audience: "you",
                notBefore: now,
                expires: now.AddMinutes(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
                claims: GetClaims(user)
            );

            return handler.WriteToken(jwtSecurityToken);
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        private dynamic GetClaims(User user)
        {
            List<Claim> claimList = new List<Claim>();
            claimList.Add(new Claim("email", user.Username));
            foreach (string role in user.Roles)
            {
                claimList.Add(new Claim("role", role));
            }

            return claimList;
        }

        public bool VerifyAccessToken()
        {
            string token = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InJhc2VsQGdtYWlsLmNvbSIsInJvbGUiOlsiYWRtaW4iLCJjdXN0b21lciJdLCJuYmYiOjE1ODM1NjI2OTMsImV4cCI6MTU4MzU2Mjk5MywiaXNzIjoibWUiLCJhdWQiOiJ5b3UifQ.d_gEpWGAWrzc75tO-AxLL8rKLQa_s7oyxuMLHIGyNbcIxZBAzLTj8Fe5pHxPDakVFyaKLIDzC1f5cvPTovweb1XjcHksh0SxROFBTUXLRVOI2kHPIE837OLVP2xDBWU5UP1dyMU8iOc4G1rsrrgf2ZcwUCry3eJjaHd-celMK9pEmqiIG8riaWn-7yUliWmfqac6vjY4Cpj6U606ESMzWtCTtCZ4Zczo_zGMgIvvMiynk_Hzjwkx1RSrYvvJ3N0YLcl6YTNIFDN33deKzy-7jR-GmGubYP1b85QeZ3RMh2-O7_uu1eRUywot0CRPaYyh5PIvQ_XSfUbJKtMx_JMbDA";
            var handler = new JwtSecurityTokenHandler();

            var publicKeyBytes = Convert.FromBase64String(PublicKey.Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", ""));
            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
            var key = new RsaSecurityKey(rsa);

            SecurityToken validatedToken = null;
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = key
            };

            try
            {
                handler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                JwtSecurityToken validatedJwt = validatedToken as JwtSecurityToken;
            } 
            catch(Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
