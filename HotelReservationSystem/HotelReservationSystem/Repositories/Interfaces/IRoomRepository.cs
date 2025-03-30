using HotelReservationSystem.Models.Domain;

namespace HotelReservationSystem.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAvailableRooms(DateTime from, DateTime to);
        Room GetById(int id);
    }
}
