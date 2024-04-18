using COMMSFinalProject.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace COMMSFinalProject.Models
{
    public class TokenMaker : ITokenMaker
    {
        private readonly IHttpContextAccessor _httpContext;

        public TokenMaker(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public int GetUserId()
        {
            var user = _httpContext?.HttpContext?.User as ClaimsPrincipal;
            var userId = user.Claims.ElementAt(0).Value;
            var userIdInt = Convert.ToInt32(userId);
            return userIdInt;
        }
    }
}
