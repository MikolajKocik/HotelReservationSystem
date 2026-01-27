using System.Security.Claims;
using HotelReservationSystem.Application.Interfaces.Auth;

namespace HotelReservationSystem.Application.UseCases.Auth;

/// <summary>
/// Contains information about the current authorization operation
/// </summary>
public class AuthorizationHandlerContext
{
    /// <summary>
    /// Gets the user being authorized
    /// </summary>
    public ClaimsPrincipal User { get; }

    /// <summary>
    /// Gets the resource being authorized
    /// </summary>
    public object? Resource { get; }

    /// <summary>
    /// Gets the requirements being evaluated
    /// </summary>
    public IEnumerable<IAuthorizationRequirement> Requirements { get; }

    /// <summary>
    /// Gets or sets whether the authorization has succeeded
    /// </summary>
    public bool HasSucceeded { get; set; }

    /// <summary>
    /// Gets or sets whether the authorization has failed
    /// </summary>
    public bool HasFailed => !HasSucceeded;

    /// <summary>
    /// Gets the pending requirements that have not been evaluated yet
    /// </summary>
    public IEnumerable<IAuthorizationRequirement> PendingRequirements { get; private set; }

    public AuthorizationHandlerContext(
        IEnumerable<IAuthorizationRequirement> requirements,
        ClaimsPrincipal user,
        object? resource)
    {
        Requirements = requirements ?? throw new ArgumentNullException(nameof(requirements));
        User = user ?? throw new ArgumentNullException(nameof(user));
        Resource = resource;
        PendingRequirements = requirements;
    }

    /// <summary>
    /// Marks the specified requirement as succeeded
    /// </summary>
    public void Succeed(IAuthorizationRequirement requirement)
    {
        if (requirement == null)
            throw new ArgumentNullException(nameof(requirement));

        var pending = PendingRequirements.ToList();
        pending.Remove(requirement);
        PendingRequirements = pending;

        if (!pending.Any())
        {
            HasSucceeded = true;
        }
    }

    /// <summary>
    /// Marks the authorization as failed
    /// </summary>
    public void Fail()
    {
        HasSucceeded = false;
    }
}