using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Exceptions;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;
using HotelReservationSystem.Infrastructure.Repositories; 

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
        Room? room = await this.roomRepository.GetByIdAsync(command.RoomId);
        if (room == null)
        {
            throw new NotFoundException("Room does not exist");
        }

        if (!room.IsAvailable)
        {
            throw new InvalidOperationDomainException("Room is not available");
        }

        IQueryable<Reservation> conflictingReservations = await this.reservationRepository.GetByRoomAndDateRangeAsync(
            command.RoomId, command.ArrivalDate, command.DepartureDate);

        if (conflictingReservations.Any())
        {
            throw new InvalidOperationDomainException("Room is already reserved for the selected dates");
        }

        if ((command.DepartureDate - command.ArrivalDate).Days <= 0)
            throw new ArgumentException("Invalid reservation dates: departure must be after arrival");

        Guest? guest = await this.guestRepository.GetByEmailAsync(command.GuestEmail);
        if (guest == null)
        {
            guest = new Guest(command.GuestFirstName, command.GuestLastName, command.GuestEmail, command.GuestPhoneNumber);
            await this.guestRepository.CreateAsync(guest);
        }

        int numberOfNights = (command.DepartureDate - command.ArrivalDate).Days;
        decimal totalPrice = numberOfNights * room.PricePerNight;

        var reservation = new Reservation(
            command.ArrivalDate,
            command.DepartureDate,
            1,
            totalPrice,
            "",
            ReservationStatus.Pending,
            "",
            command.RoomId,
            guest.Id,
            null
        );

        string reservationId = await this.reservationRepository.CreateAsync(reservation);
        return reservationId;
    }
}

