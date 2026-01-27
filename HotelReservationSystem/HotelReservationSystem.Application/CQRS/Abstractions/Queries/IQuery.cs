namespace HotelReservationSystem.Application.CQRS.Abstractions.Queries;

/// <summary>
/// Marker interface for queries that return a result of type T
/// </summary>
/// <typeparam name="TResult">The type of result the query returns</typeparam>
public interface IQuery<out TResult>
{
}
