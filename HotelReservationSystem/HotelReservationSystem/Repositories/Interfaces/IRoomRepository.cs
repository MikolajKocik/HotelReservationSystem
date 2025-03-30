using HotelReservationSystem.Models.Domain;

namespace HotelReservationSystem.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAvailableRooms(DateTime from, DateTime to);
        Task<Room> GetByIdAsync(int id);
    }
}
