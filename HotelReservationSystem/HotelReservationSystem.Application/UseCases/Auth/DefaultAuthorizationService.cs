using System.Security.Claims;
using HotelReservationSystem.Application.Interfaces.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationSystem.Application.UseCases.Auth;

/// <summary>
/// Default implementation of IAuthorizationService
/// </summary>
public class DefaultAuthorizationService : IAuthorizationService
{
    private readonly IEnumerable<IAuthorizationHandler> handlers;
    private readonly IServiceProvider serviceProvider;

    public DefaultAuthorizationService(
        IEnumerable<IAuthorizationHandler> handlers,
        IServiceProvider serviceProvider)
    {
        this.handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task<AuthorizationResult> AuthorizeAsync(
        ClaimsPrincipal user,
        object? resource,
        IEnumerable<IAuthorizationRequirement> requirements)
    {
        if (requirements == null)
            throw new ArgumentNullException(nameof(requirements));

        var context = new AuthorizationHandlerContext(requirements, user, resource);

        foreach (var handler in this.handlers)
        {
            await handler.HandleAsync(context);

            if (context.HasFailed)
            {
                return AuthorizationResult.Failed("Authorization failed");
            }
        }

        return context.HasSucceeded
            ? AuthorizationResult.Success()
            : AuthorizationResult.Failed("Authorization requirements not met");
    }

    public async Task<AuthorizationResult> AuthorizeAsync(
        ClaimsPrincipal user,
        object? resource,
        string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
            throw new ArgumentNullException(nameof(policyName));

         IAuthorizationService aspNetAuthService = this.serviceProvider.GetRequiredService<IAuthorizationService>();

         AuthorizationResult result = await aspNetAuthService.AuthorizeAsync(user, resource, policyName);

        return result.Succeeded
            ? AuthorizationResult.Success()
            : AuthorizationResult.Failed($"Policy '{policyName}' authorization failed");
    }
}