using Microsoft.AspNetCore.Authorization;
using ProductService.Application.Common.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Api.Authorization
{
    public class OwnerAuthorizationHandler
        : AuthorizationHandler<OwnerRequirement, Product>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IApplicationDbContext _dbContext;

        public OwnerAuthorizationHandler(
            ICurrentUserService currentUserService,
            IApplicationDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnerRequirement requirement,
            Product resource)
        {
            if (_currentUserService.UserId == Guid.Empty)
                return;

            if (resource.CreatedByUserId == _currentUserService.UserId)
            {
                context.Succeed(requirement);
            }

            await Task.CompletedTask;
        }
    }
}
