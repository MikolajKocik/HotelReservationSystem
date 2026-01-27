namespace HotelReservationSystem.Application.CQRS.Abstractions.Commands;

/// <summary>
/// Marker interface for commands that don't return a result
/// </summary>
public interface ICommand
{
}

/// <summary>
/// Marker interface for commands that return a result of type T
/// </summary>
/// <typeparam name="TResult">The type of result the command returns</typeparam>
public interface ICommand<out TResult>
{
}
