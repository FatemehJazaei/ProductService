using System.Security.Claims;
using System.Security.Claims;
using ProductService.Application.Common.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {

                var userId = _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.NameIdentifier); 
                return userId != null ? Guid.Parse(userId) : (Guid?)null;
            }
        }
    }
}
