using HotelReservationSystem.Infrastructure.Data;
using HotelReservationSystem.Core.Domain.Entities;
using HotelReservationSystem.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Core.Domain.Enums;

namespace HotelReservationSystem.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for room entities
/// </summary>
public sealed class RoomRepository : IRoomRepository
{
    private readonly HotelDbContext context;

    public RoomRepository(HotelDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Gets all rooms with related data
    /// </summary>
    public async Task<IQueryable<Room>> GetAllAsync(CancellationToken cancellationToken = default)
        => await Task.FromResult(this.context.Rooms
            .AsNoTracking()
            .Include(r => r.Reservations));


    /// <summary>
    /// Gets a room by its unique identifier
    /// </summary>
    public async Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await this.context.Rooms
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);


    /// <summary>
    /// Gets available rooms for a date range
    /// </summary>
    public async Task<IQueryable<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to, CancellationToken cancellationToken = default)
        => await Task.FromResult(this.context.Rooms
            .AsNoTracking()
            .Where(r => r.IsAvailable &&
                       !r.Reservations.Any(res =>
                           (res.Status == ReservationStatus.Confirmed || res.Status == ReservationStatus.Pending) &&
                           res.ArrivalDate < to && res.DepartureDate > from)));

    /// <summary>
    /// Gets rooms by type with availability status for date range
    /// </summary>
    public async Task<IQueryable<Room>> GetRoomsByTypeAsync(string? roomType, CancellationToken cancellationToken = default)
    {
        var query = this.context.Rooms
            .AsNoTracking()
            .Include(r => r.Reservations);

        if (!string.IsNullOrEmpty(roomType) && Enum.TryParse<RoomType>(roomType, ignoreCase: true, out var parsed))
        {
            return await Task.FromResult(query.Where(r => r.Type == parsed));
        }

        return await Task.FromResult(query);
    }

    /// <summary>
    /// Creates a new room
    /// </summary>
    public async Task<int> CreateAsync(Room room, CancellationToken cancellationToken = default)
    {
        this.context.Rooms.Add(room);
        await this.context.SaveChangesAsync(cancellationToken);
        return room.Id;
    }

    /// <summary>
    /// Updates an existing room
    /// </summary>
    public async Task UpdateAsync(Room room, CancellationToken cancellationToken = default)
    {
        this.context.Rooms.Update(room);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a room
    /// </summary>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        Room? room = await GetByIdAsync(id, cancellationToken);
        if (room != null)
        {
            this.context.Rooms.Remove(room);
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Toggles room availability status
    /// </summary>
    public async Task ToggleAvailabilityAsync(int id, CancellationToken cancellationToken = default)
    {
        Room? room = await GetByIdAsync(id, cancellationToken);
        if (room != null)
        {
            room.SetAvailability(!room.IsAvailable);
            await UpdateAsync(room, cancellationToken);
        }
    }
}

