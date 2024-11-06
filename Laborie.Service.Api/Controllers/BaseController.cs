using System.IdentityModel.Tokens.Jwt;
using Laborie.Service.Shared.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Laborie.Service.Api.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetUserFromToken()
        {
            try
            {
                var token = Request.Headers[HeaderNames.Authorization].ToString();
                if (string.IsNullOrEmpty(token) || !token.StartsWith(AppConstants.AuthorizationHeaderPrefix))
                    throw new UnauthorizedAccessException("Not found user");

                token = token.Replace(AppConstants.AuthorizationHeaderPrefix, string.Empty);
                // Giải mã token
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

                // Lấy UserId hoặc các claim khác từ token
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == AppConstants.AuthorizationSub)?.Value;
                if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("Not found user");
                
                return userId;
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Not found user");
            }
        }
    }
}