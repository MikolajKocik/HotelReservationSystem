using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.Reservation.UseCases;
using System.Collections.Concurrent;
using HotelReservationSystem.Application.Interfaces;

namespace HotelReservationSystem.Infrastructure.Repositories;

public sealed class ReservationQueue : IReservationQueue
{
    private readonly ConcurrentQueue<ReservationQueueItem> queue = new();
    private readonly SemaphoreSlim signal = new(0);

    /// <summary>
    /// Enqueues a new reservation creation command for processing
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public Task<string> EnqueueAsync(CreateReservationCommand command)
    {
        var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var item = new ReservationQueueItem { 
            Command = command, 
            Tcs = tcs 
        };

        this.queue.Enqueue(item);
        this.signal.Release();

        return tcs.Task;
    }

    /// <summary>
    /// Dequeues a reservation creation command for processing
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ReservationQueueItem?> DequeueAsync(CancellationToken cancellationToken)
    {
        try
        {
            await this.signal.WaitAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return null;
        }

        if (queue.TryDequeue(out var item))
            return item;

        return null;
    }
}
