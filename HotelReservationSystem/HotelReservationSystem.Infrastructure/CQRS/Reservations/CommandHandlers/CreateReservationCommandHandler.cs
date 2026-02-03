using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Exceptions;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;
using HotelReservationSystem.Infrastructure.Repositories; 
using Microsoft.AspNetCore.Http.HttpResults;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for creating a new reservation 
/// </summary>
public sealed class CreateReservationCommandHandler : ICommandHandler<CreateReservationCommand, string>
{
    private readonly IReservationRepository reservationRepository;
    private readonly IRoomRepository roomRepository;
    private readonly IGuestRepository guestRepository;
    private readonly IReservationQueue reservationQueue;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        IGuestRepository guestRepository,
        IReservationQueue reservationQueue)
    {
        this.reservationRepository = reservationRepository;
        this.roomRepository = roomRepository;
        this.guestRepository = guestRepository;
        this.reservationQueue = reservationQueue;
    }

    /// <summary>
    /// Handles the command to create a new reservation by enqueuing it for background processing
    /// </summary>
    public async Task<string> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken = default)
    {
        Room? room = await roomRepository.GetByIdAsync(command.RoomId, cancellationToken);
        if (room == null || !room.IsAvailable) throw new NotFoundException("Room not found or unavailable");

        int nights = (command.DepartureDate - command.ArrivalDate).Days;
        decimal basePrice = nights * room.PricePerNight;

        decimal multiplier = command.DiscountCode?.ToUpper() switch
        {
            "SUMMER10" => 0.9m,
            "WINTER15" => 0.85m,
            _ => 1.0M
        };
        decimal finalPrice = basePrice * multiplier;

        Guest? guest = await this.guestRepository.GetByEmailAsync(command.GuestEmail, cancellationToken);
        if (guest == null)
        {
            guest = new Guest(
                command.GuestFirstName, 
                command.GuestLastName, 
                command.GuestEmail, 
                command.GuestPhoneNumber
            );
            await this.guestRepository.CreateAsync(guest, cancellationToken); 
        }

        var reservation = new Reservation(
            command.ArrivalDate,
            command.DepartureDate,
            command.NumberOfGuests,
            finalPrice,
            command.AdditionalRequests ?? string.Empty,
            ReservationStatus.Pending,
            string.Empty,
            command.RoomId,
            guest.Id
        );

        return await reservationRepository.CreateAsync(reservation, cancellationToken);
    }
}

