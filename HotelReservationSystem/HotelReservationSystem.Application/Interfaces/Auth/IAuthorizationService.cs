using System.Security.Claims;
using HotelReservationSystem.Application.UseCases.Auth;

namespace HotelReservationSystem.Application.Interfaces.Auth;

public interface IAuthorizationService
{
    /// <summary>
    /// Authorizes a user against a set of requirements for a given resource
    /// </summary>
    /// <param name="user"></param>
    /// <param name="resource"></param>
    /// <param name="requirements"></param>
    /// <returns></returns>
    Task<AuthorizationResult> AuthorizeAsync (ClaimsPrincipal user, object? resource, 
        IEnumerable<IAuthorizationRequirement> requirements);

    /// <summary>
    /// Authorizes a user against a policy for a given resource
    /// </summary>
    /// <param name="user"></param>
    /// <param name="resource"></param>
    /// <param name="policyName"></param>
    /// <returns></returns>
    Task<AuthorizationResult> AuthorizeAsync (ClaimsPrincipal user, object? resource, string policyName);
}
