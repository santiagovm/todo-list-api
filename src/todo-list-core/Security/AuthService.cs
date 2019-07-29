using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Security
{
    public class AuthService : IAuthService
    {
        public AuthService(IMembershipProvider membershipProvider, IRSAKeyProvider rsaKeyProvider)
        {
            _membershipProvider = membershipProvider ?? throw new ArgumentNullException(nameof(membershipProvider));
            _rsaKeyProvider = rsaKeyProvider ?? throw new ArgumentNullException(nameof(rsaKeyProvider));
        }

        #region Implementation of IAuthService

        public async Task<string> GenerateJwtAsync(string username, string password)
        {
            if (!_membershipProvider.VerifyUserPassword(username, password))
            {
                throw new SecurityException("invalid credentials");
            }

            IEnumerable<Claim> claims = _membershipProvider.GetUserClaims(username);

            string rsaKeys = await _rsaKeyProvider.GetKeysAsync();

            if (string.IsNullOrWhiteSpace(rsaKeys))
            {
                throw new Exception("rsa keys not found");
            }

            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(rsaKeys);

                var jwt = new JwtSecurityToken(issuer: STUB_ISSUER,
                                               audience: STUB_AUDIENCE,
                                               claims,
                                               signingCredentials: new SigningCredentials(new RsaSecurityKey(rsaProvider),
                                                                                          SecurityAlgorithms.RsaSha256Signature),
                                               expires: DateTime.Now.AddDays(1));

                var tokenHandler = new JwtSecurityTokenHandler();
            
                return tokenHandler.WriteToken(jwt);
            }
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string jwtEncodedString)
        {
            string keys = await _rsaKeyProvider.GetKeysAsync();

            if (keys == null)
            {
                throw new Exception("rsa keys not found");
            }
            
            var rsaProvider = new RSACryptoServiceProvider();
            rsaProvider.FromXmlString(keys);

            var validationParameters = new TokenValidationParameters
                                       {
                                           ValidIssuer = STUB_ISSUER,
                                           ValidAudience = STUB_AUDIENCE,
                                           IssuerSigningKey = new RsaSecurityKey(rsaProvider)
                                       };

            try
            {
                return new JwtSecurityTokenHandler().ValidateToken(jwtEncodedString,
                                                                   validationParameters,
                                                                   out SecurityToken _);
            }
            catch (ArgumentException e)
            {
                Log.Error(e, "security token could not be parsed");
                return null;
            }
            catch (SecurityTokenException e)
            {
                Log.Error(e, "security token is invalid");
                return null;
            }
        }

        #endregion

        private const string STUB_ISSUER = "http://foo.issuer.com";
        private const string STUB_AUDIENCE = "http://foo.site.com";

        private readonly IMembershipProvider _membershipProvider;
        private readonly IRSAKeyProvider _rsaKeyProvider;
    }
}
