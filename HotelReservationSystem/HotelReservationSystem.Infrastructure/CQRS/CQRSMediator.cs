using HotelReservationSystem.Application.CQRS.Abstractions;
using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace HotelReservationSystem.Infrastructure.CQRS;

public class CQRSMediator : ICQRSMediator
{
    private readonly IServiceProvider serviceProvider;

    public CQRSMediator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<TResult> SendAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        Type handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        object handler = serviceProvider.GetRequiredService(handlerType);
        
        MethodInfo? method = handlerType.GetMethod("HandleAsync");
        var task = method!.Invoke(handler, new object[] { query, cancellationToken }) as Task<TResult>
            ?? throw new InvalidOperationException("Returned object is not a awaited Task<TResult>.");

        TResult result = await task;
        return result;
    }

    public async Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        Type handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        object handler = serviceProvider.GetRequiredService(handlerType);
        
        MethodInfo? method = handlerType.GetMethod("HandleAsync");
        var task = method!.Invoke(handler, new object[] { command, cancellationToken }) as Task ??
            throw new InvalidOperationException("Returned object is not a awaited Task.");

        await task;
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        object handler = serviceProvider.GetRequiredService(handlerType);
        
        MethodInfo? method = handlerType.GetMethod("HandleAsync");
        var task = method!.Invoke(handler, new object[] { command, cancellationToken }) as Task<TResult>
            ?? throw new InvalidOperationException("Returned object is not a awaited Task<TResult>.");
        
        TResult result = await task;
        return result;
    }
}
