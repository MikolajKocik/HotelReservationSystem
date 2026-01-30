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
    public async Task<IQueryable<Room>> GetAllAsync()
        => await Task.FromResult(this.context.Rooms
            .AsNoTracking()
            .Include(r => r.Reservations));


    /// <summary>
    /// Gets a room by its unique identifier
    /// </summary>
    public async Task<Room?> GetByIdAsync(int id)
        => await this.context.Rooms
            .Include(r => r.Reservations)
            .FirstOrDefaultAsync(r => r.Id == id);


    /// <summary>
    /// Gets available rooms for a date range
    /// </summary>
    public async Task<IQueryable<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to)
        => await Task.FromResult(this.context.Rooms
            .AsNoTracking()
            .Where(r => r.IsAvailable &&
                       !r.Reservations.Any(res =>
                           (res.Status == ReservationStatus.Confirmed || res.Status == ReservationStatus.Pending) &&
                           res.ArrivalDate < to && res.DepartureDate > from)));


    /// <summary>
    /// Creates a new room
    /// </summary>
    public async Task<int> CreateAsync(Room room)
    {
        this.context.Rooms.Add(room);
        await this.context.SaveChangesAsync();
        return room.Id;
    }

    /// <summary>
    /// Updates an existing room
    /// </summary>
    public async Task UpdateAsync(Room room)
    {
        this.context.Rooms.Update(room);
        await this.context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a room
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        Room? room = await GetByIdAsync(id);
        if (room != null)
        {
            this.context.Rooms.Remove(room);
            await this.context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Toggles room availability status
    /// </summary>
    public async Task ToggleAvailabilityAsync(int id)
    {
        Room? room = await GetByIdAsync(id);
        if (room != null)
        {
            room.SetAvailability(!room.IsAvailable);
            await UpdateAsync(room);
        }
    }
}

