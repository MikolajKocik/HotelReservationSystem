using HotelReservationSystem.Application.UseCases.Auth;
namespace HotelReservationSystem.Application.Interfaces.Auth;

public interface IAuthorizationHandler
{
    /// <summary>
    /// Handles the authorization context
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task HandleAsync(AuthorizationHandlerContext context); 
}
