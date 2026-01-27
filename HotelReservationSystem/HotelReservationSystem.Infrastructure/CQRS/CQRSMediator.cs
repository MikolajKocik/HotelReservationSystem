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
        TResult result = await (Task<TResult>)method!.Invoke(handler, new object[] { query, cancellationToken })!;
        
        return result;
    }

    public async Task SendAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        Type handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
        object handler = serviceProvider.GetRequiredService(handlerType);
        
        MethodInfo? method = handlerType.GetMethod("HandleAsync");
        await (Task)method!.Invoke(handler, new object[] { command, cancellationToken })!;
    }

    public async Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
    {
        Type handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        object handler = serviceProvider.GetRequiredService(handlerType);
        
        MethodInfo? method = handlerType.GetMethod("HandleAsync");
        TResult result = await (Task<TResult>)method!.Invoke(handler, new object[] { command, cancellationToken })!;
        
        return result;
    }
}
