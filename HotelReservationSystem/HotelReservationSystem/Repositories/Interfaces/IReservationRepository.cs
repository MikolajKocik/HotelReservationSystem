using HotelReservationSystem.Models.Domain;

namespace HotelReservationSystem.Repositories.Interfaces
{
    public interface IReservationRepository
    {
        Task Add(Reservation reservation);
        Task<Reservation> GetById(int id);
        Task<IEnumerable<Reservation>> GetAll();
        Task Update(Reservation reservation);
    }
}
