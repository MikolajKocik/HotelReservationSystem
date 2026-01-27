using HotelReservationSystem.Application.CQRS.Abstractions.Commands;
using HotelReservationSystem.Application.CQRS.Reservations.Commands;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Infrastructure.CQRS.Reservations.CommandHandlers;

/// <summary>
/// Handler for creating a new reservation
/// </summary>
public class CreateReservationCommandHandler : ICommandHandler<CreateReservationCommand, string>
{
    private readonly IReservationRepository reservationRepository;
    private readonly IRoomRepository roomRepository;
    private readonly IGuestRepository guestRepository;

    public CreateReservationCommandHandler(
        IReservationRepository reservationRepository,
        IRoomRepository roomRepository,
        IGuestRepository guestRepository)
    {
        this.reservationRepository = reservationRepository;
        this.roomRepository = roomRepository;
        this.guestRepository = guestRepository;
    }

    /// <summary>
    /// Handles the command to create a new reservation
    /// </summary>
    public async Task<string> HandleAsync(CreateReservationCommand command, CancellationToken cancellationToken = default)
    {
        Room? room = await roomRepository.GetByIdAsync(command.RoomId);
        if (room == null)
        {
            throw new Exception("Room does not exist");
        }

        if (!room.IsAvailable)
        {
            throw new Exception("Room is not available");
        }

        IQueryable<Reservation> conflictingReservations = await reservationRepository.GetByRoomAndDateRangeAsync(
            command.RoomId, command.ArrivalDate, command.DepartureDate);

        if (conflictingReservations.Any())
        {
            throw new Exception("Room is already reserved for the selected dates");
        }

        Guest? guest = await guestRepository.GetByEmailAsync(command.GuestEmail);
        if (guest == null)
        {
            guest = new Guest(command.GuestFirstName, command.GuestLastName, command.GuestEmail, command.GuestPhoneNumber);
            await guestRepository.CreateAsync(guest);
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

        return await reservationRepository.CreateAsync(reservation);
    }
}