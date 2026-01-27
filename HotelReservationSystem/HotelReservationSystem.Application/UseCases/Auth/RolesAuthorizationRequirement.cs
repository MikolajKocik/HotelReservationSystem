using System.Security.Claims;
using HotelReservationSystem.Application.UseCases.Auth;
using HotelReservationSystem.Application.Interfaces.Auth;

namespace HotelReservationSystem.Application.UseCases.Auth;

/// <summary>
/// Represents a requirement that the user must have one of the specified roles
/// </summary>
public class RolesAuthorizationRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the allowed roles
    /// </summary>
    public IEnumerable<string> AllowedRoles { get; }

    public RolesAuthorizationRequirement(IEnumerable<string> allowedRoles)
    {
        AllowedRoles = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
    }

    public RolesAuthorizationRequirement(params string[] allowedRoles)
        : this((IEnumerable<string>)allowedRoles)
    {
    }
}

/// <summary>
/// Authorization handler for role-based requirements
/// </summary>
public class RolesAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var roleRequirements = context.Requirements.OfType<RolesAuthorizationRequirement>();

        foreach (var requirement in roleRequirements)
        {
            if (requirement.AllowedRoles.Any(role => context.User.IsInRole(role)))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}