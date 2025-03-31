using HotelReservationSystem.Models.Domain;

namespace HotelReservationSystem.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAvailableRoomsAsync(DateTime from, DateTime to);
        Task<Room> GetByIdAsync(int id);
        Task UpdateAsync(Room room);
        Task<IEnumerable<Room>> GetAll();

    }
}
