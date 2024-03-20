using HospitalManagement;
using HospitalManagement.Security;
using HospitalManagement_Models.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace HospitalManagement
{
    public class TokenManager
    {
        private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
        /// <summary>
        /// Generate Token for User Only
        /// </summary>
        /// <param name="oInput"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }
        public static string GenerateToken_User(SystemUsersModel oInput)
        {            
            byte[] key = Convert.FromBase64String(Secret);
            var identity = new ClaimsIdentity(new[]{
                new Claim("UserId", Convert.ToString(oInput.intId)),
                new Claim(ClaimTypes.Name, Convert.ToString(oInput.strUserName)),                
                new Claim(ClaimTypes.Email, Convert.ToString(oInput.strEmail)),
                new Claim("RoleId", Convert.ToString(oInput.intUserType)),
            });
            var configuration = GetConfiguration();            
            Microsoft.IdentityModel.Tokens.SymmetricSecurityKey securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);
            Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor descriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(System.Convert.ToDouble(configuration.GetSection("TokenExpire").Value)),
                SigningCredentials = new SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            // Dim oPrinciples = TokenManager.GetTokenClaims(token.EncodedPayload.ToString)
            return "Bearer " + handler.WriteToken(token);
        }
        /// <summary>
        /// Used to Decrypt User Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static UserModel GetTokenClaims_User(string token)
        {
            UserModel oRetval = new UserModel();
            try
            {
                string sExpiredTime = null;
                var principal = GetPrincipal(token);
                DateTime expiryTime = default(DateTime);
               
                ClaimsIdentity identity = null/* TODO Change to default(_) if this is not a reference type */;


                try
                {
                    identity = (ClaimsIdentity)principal.Identity;
                }
                catch (Exception)
                {
                    oRetval.intStatus = -1;                    
                }
                if (oRetval.intStatus != -1)
                {
                    var expClaim = principal.Claims.First(x => x.Type == "exp").Value;

                    var handler = new JwtSecurityTokenHandler();
                    var tokenRead = handler.ReadToken(token) as JwtSecurityToken;
                    var tokenExpiryDate = tokenRead.ValidTo;
                    expiryTime = tokenExpiryDate;

                    // Get the claims value from token                    
                    var intUserId = System.Convert.ToString(principal.Claims.First(x => x.Type == "UserId").Value);
                    var strName = identity.FindFirst(ClaimTypes.Name).Value;
                    var strEmail = identity.FindFirst(ClaimTypes.Email).Value;
                    var intRoleId = System.Convert.ToString(principal.Claims.First(x => x.Type == "RoleId").Value);
                    oRetval.intId = Convert.ToInt32(intUserId);
                    oRetval.strName = strName;
                    oRetval.strEmail = strEmail;
                    oRetval.intRoleId = Convert.ToInt32(intRoleId);
                    // Throw exception if token is expired.
                    if (tokenExpiryDate < DateTime.UtcNow)
                        oRetval.intStatus = -1;
                    if (Convert.ToInt32(intUserId) == 0)
                        oRetval.intStatus = -1;
                }                                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oRetval;
        }

        /// <summary>
        /// Used to Decrypt Student Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static TokenClaims GetTokenClaims(string token)
        {
            TokenClaims oRetval = new TokenClaims();
            try
            {
                string sExpiredTime = null;
                var principal = GetPrincipal(token);
                DateTime expiryTime = default(DateTime);

                //oRetval.iStatus = 1;
                //if (principal == null)
                //    oRetval.iStatus = -1;
                ClaimsIdentity identity = null/* TODO Change to default(_) if this is not a reference type */;


                identity = (ClaimsIdentity)principal.Identity;
                var expClaim = principal.Claims.First(x => x.Type == "exp").Value;

                var handler = new JwtSecurityTokenHandler();
                var tokenRead = handler.ReadToken(token) as JwtSecurityToken;
                var tokenExpiryDate = tokenRead.ValidTo;
                expiryTime = tokenExpiryDate;

                // Get the claims value from token
                var sRole = identity.FindFirst(ClaimTypes.Role).Value;
                var sUserName = identity.FindFirst(ClaimTypes.Name).Value;
                var sEmail = identity.FindFirst(ClaimTypes.Email).Value;
                var iUniversityId = System.Convert.ToInt32(principal.Claims.First(x => x.Type == "UniversityId").Value);
                var iLocationId = System.Convert.ToInt32(principal.Claims.First(x => x.Type == "LocationId").Value);
                var iUserId = System.Convert.ToInt32(principal.Claims.First(x => x.Type == "UserId").Value);
                var iRoleId = System.Convert.ToInt32(principal.Claims.First(x => x.Type == "RoleId").Value);
                var LocationName = principal.Claims.First(x => x.Type == "LocationName").Value;
                var NewsUserId = System.Convert.ToInt32(principal.Claims.First(x => x.Type == "NewsUserId").Value);
                var FinanceUserId = System.Convert.ToInt32(principal.Claims.First(x => x.Type == "FinanceUserId").Value);
                var EnrollmentNo = principal.Claims.First(x => x.Type == "EnrollmentNo").Value;
                var ApplicationNo = System.Convert.ToInt32(principal.Claims.First(x => x.Type == "ApplicationNo").Value);

                {
                    var withBlock = oRetval;
                    withBlock.sUserType = sRole;
                    withBlock.iUniversityId = iUniversityId;
                    withBlock.iLocationId = iLocationId;
                    withBlock.sUserName = sUserName;
                    withBlock.sEmail = sEmail;
                    withBlock.sExpiryTime = expiryTime;
                    withBlock.UserId = iUserId;
                    withBlock.iRoleId = iRoleId;
                    withBlock.LocationName = LocationName;
                    withBlock.NewsUserId = NewsUserId;
                    withBlock.FinanceUserId = FinanceUserId;
                    withBlock.EnrollmentNo = EnrollmentNo;
                    withBlock.ApplicationNo = ApplicationNo;
                }

                // Throw exception if token is expired.
                //if (tokenExpiryDate < DateTime.UtcNow)
                //    oRetval.iStatus = -1;
            }
            catch (Exception ex)
            {
            }
            return oRetval;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null/* TODO Change to default(_) if this is not a reference type */;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)
                };
                Microsoft.IdentityModel.Tokens.SecurityToken securityToken;
                /* TODO ERROR: Skipped WarningDirectiveTrivia */
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                /* TODO ERROR: Skipped WarningDirectiveTrivia */
                return principal;
            }
            catch
            {
                return null/* TODO Change to default(_) if this is not a reference type */;
            }
        }
    }
}