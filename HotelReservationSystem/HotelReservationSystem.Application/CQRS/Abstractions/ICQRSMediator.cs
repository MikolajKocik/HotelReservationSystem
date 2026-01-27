using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;

namespace HotelReservationSystem.Application.CQRS.Abstractions;

/// <summary>
/// Mediator interface for handling commands and queries
/// </summary>
public interface ICQRSMediator
{
    /// <summary>
    /// Sends a query and returns its result
    /// </summary>
    /// <typeparam name="TResult">The type of result</typeparam>
    /// <param name="query">The query to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The query result</returns>
    Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a command that doesn't return a result
    /// </summary>
    /// <param name="command">The command to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task representing the async operation</returns>
    Task SendAsync(ICommand command, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a command and returns its result
    /// </summary>
    /// <typeparam name="TResult">The type of result</typeparam>
    /// <param name="command">The command to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The command result</returns>
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);
}
